using AutoMapper;
using Community.Domain.Contracts;
using Community.ServiceAbstraction;
using Communtiy.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Service
{
    public class CommunityService (ICommentRepo commentRepo ,IMapper mapper ,ICountService countService) : IcommunityService
    {
        public async Task<CommentsResponseDto> GetCommnentsByIsuueIdAsync(Guid issueId,int page , int pageSize, CancellationToken cancellationToken = default)
        {
            var comments = await commentRepo.GetByIssueIdAsync(issueId,page,pageSize,cancellationToken);
                
            var newcount = await countService.GetCommentCountAsync(issueId);
            var result = mapper.Map<IEnumerable<CommentResponseDto>>(comments);
            return new CommentsResponseDto
            {
                Comments = result,
                Count = newcount
            };
        }
    }
}
