using Community.Clinets.ServiceAbstrction;
using Community.Domain.Contracts;
using Community.Domain.Entities;
using Community.ServiceAbstraction;
using Communtiy.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Community.Service
{
    [Authorize]
    public class CommunityHub
        (ICommentRepo commentRepo
        ,IIssueShareRepo shareRepo
        ,ICountService CountService
        ,ICommentModerationClient moderationClient
        ,ILogger<CommunityHub> logger
        , IIssueVoteRepo voteRepo) : Hub
        
    {
        public async Task SendComment(CommunityRequestDto commentRequestDto)
        {
            var user = GetLoggedInUserId();
            var commentId = Guid.NewGuid();

            // 1. Send the comment text to the AI service
            var moderation = await moderationClient.ModerateAsync(commentId.ToString(), commentRequestDto.Text);

            // 2. Decide based on the AI's response
            if (moderation is not null &&
                (moderation.Label.Equals("Offensive", StringComparison.OrdinalIgnoreCase) ||
                 moderation.Label.Equals("Spam", StringComparison.OrdinalIgnoreCase)))
            {
                logger.LogInformation("Comment {CommentId} blocked by AI: {Label} ({Confidence:P0})",
                    commentId, moderation.Label, moderation.Confidence);

                throw new HubException($"Comment blocked: {moderation.Label} (confidence {moderation.Confidence:P0})");
            }
            if (moderation.IsFlagged)
            {
                logger.LogInformation(
                    "Comment {CommentId} blocked by AI: {Label} ({Confidence:P0})",
                    commentId,
                    moderation.Label,
                    moderation.Confidence);

                throw new HubException(
                    $"Comment blocked: {moderation.Label}");
            }

            if (moderation is null)
            {
                logger.LogWarning($"Moderation service unreachable, allowing comment {commentId} through", commentId);
                throw new HubException("Comment moderation is currently unavailable. Please try again.");
            }
            var comment = new Comment
            {
                IssueId = commentRequestDto.IssueId,
                UserId = user,
                Text = commentRequestDto.Text,
                VoiceUrl = commentRequestDto.VoiceUrl,
                CreatedAt = DateTime.UtcNow
            };
            await commentRepo.AddAsync(comment);
            var newcount = await CountService.IncreamentCommentAsync(commentRequestDto.IssueId);

            await Clients.Group(commentRequestDto.IssueId.ToString())
                .SendAsync("SendComment", new
                {
                    comment,
                    newcount
                });
        }

        public async Task<int> GetCommentCurrentCount(Guid issueId)
        {
            return(int)await CountService.GetCommentCountAsync(issueId);
        }
        public async Task ShareIssue(CommunityRequestDto RequestDto)
        {
            var user = GetLoggedInUserId();
            
            var issueShare = new IssueShared
            {
                IssueId = RequestDto.IssueId,
                UserId = user,
                CreatedAt = DateTime.UtcNow
            };
            var newCount =await CountService.IncrementShareAsync(RequestDto.IssueId);
            await shareRepo.AddAync(issueShare);

            await Clients.Group(RequestDto.IssueId.ToString())
                .SendAsync("MakeShare", new 
                {
                    issueShare,
                    newCount
                });
        }

        public async Task GetCurrentShareCount(Guid issueId)
        {
            var count = await CountService.GetShareCountAsync(issueId);
            await Clients.Group(issueId.ToString()).SendAsync("ReceiveShareCount", issueId, count);
        }
        public async Task VoteIssue(CommunityRequestDto RequestDto)
        {
            var user = GetLoggedInUserId();
            var issueVote = new IssueVote
            {
                IssueId = RequestDto.IssueId,
                UserId = user,
                CreatedAt = DateTime.UtcNow
            };
            if (await voteRepo.ExistsAsync(RequestDto.IssueId, user))
            {
                var existingVote = await voteRepo.GetAsync(RequestDto.IssueId, user);
                if (existingVote is not null)
                {
                    await voteRepo.DeleteAsync(existingVote);
                    var newCount = await CountService.DecrementVoteAsync(RequestDto.IssueId);
                    await Clients.Group(RequestDto.IssueId.ToString())
                        .SendAsync("RemoveVote", new
                        {
                            existingVote,
                            newCount
                        });
                }
            }
            else
            {
                await voteRepo.AddAsync(issueVote);
                var newCount = await CountService.IncrementVoteAsync(RequestDto.IssueId);   
                await Clients.Group(RequestDto.IssueId.ToString())
                    .SendAsync("MakeVote", new
                    {
                        issueVote,
                        newCount
                    });
            }
        }
        public async Task GetCurrentVoteCount(Guid issueId)
        {
            var count = await CountService.GetVoteCountAsync(issueId);
            await Clients.All.SendAsync("ReceiveVoteCount", issueId, count);
        }
        public override async Task OnConnectedAsync()
        {
            var user = GetLoggedInUserId();
            var issueId = Context.GetHttpContext()?.Request.Query["issueId"].ToString();
            if (!string.IsNullOrEmpty(issueId) && Guid.TryParse(issueId, out var parsedIssueId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, parsedIssueId.ToString());
            }
            await base.OnConnectedAsync();
        }
        public async Task DeleteComment(Guid commentId, Guid issueId)
        {
            var user = GetLoggedInUserId();
            var comment = await commentRepo.GetByIdAsync(commentId);
            if (comment is null || comment.UserId != user)
                throw new HubException("You are not allowed to delete this comment.");

            await commentRepo.DeleteAsync(commentId, issueId);
            var newCount = await CountService.DecrementCommentAsync(issueId);
            await Clients.Group(issueId.ToString()).SendAsync("DeleteComment", new
            {
                comment,
                newCount
            });
        }
        private Guid GetLoggedInUserId()
        {
            //var user = httpContextAccessor.Context?.User;
            //if (user is null || !user.Identity!.IsAuthenticated)
            //{
            //    throw new UnauthorizedAccessException("User not authenticated.");
            //}

            //var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedId))
            //{
            //    throw new UnauthorizedAccessException("User Id not found in token.");
            //}

            //return parsedId;
            var user = Context?.User;
            var userIdClaim =user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? Context.User?.FindFirst("sub")?.Value; // JWT often uses "sub"

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new HubException("User is not authenticated or user id is invalid.");

            return userId;
        }
    }
}
