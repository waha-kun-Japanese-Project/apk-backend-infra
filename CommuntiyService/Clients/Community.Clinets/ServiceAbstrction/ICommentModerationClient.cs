using Communtiy.Shared.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Clinets.ServiceAbstrction;

    public interface ICommentModerationClient
    {
        Task<ModerationResponseDto?> ModerateAsync(string commentId, string text, CancellationToken ct = default);
    }
