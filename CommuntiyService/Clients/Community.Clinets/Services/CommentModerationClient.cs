using Community.Clinets.ServiceAbstrction;
using Communtiy.Shared.Clients;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Community.Clinets.Services
{
    public class CommentModerationClient(HttpClient httpClient, ILogger<CommentModerationClient> logger)
    : ICommentModerationClient
    {
        public async Task<ModerationResponseDto?> ModerateAsync(string commentId, string text, CancellationToken ct = default)
        {
            try
            {
                var request = new ModerationRequestDto { CommentId = commentId, Text = text };

                using var response = await httpClient.PostAsJsonAsync("api/v1/moderate", request, ct);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning("Moderation service returned {StatusCode} for comment {CommentId}",
                        response.StatusCode, commentId);
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<ModerationResponseDto>(cancellationToken: ct);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                logger.LogWarning(ex, "Moderation service unreachable for comment {CommentId}", commentId);
                return null;
            }
        }
    }
}
