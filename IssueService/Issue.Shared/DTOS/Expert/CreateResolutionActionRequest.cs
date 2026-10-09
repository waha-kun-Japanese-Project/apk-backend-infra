using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record CreateResolutionActionRequest(
        IFormFile Photo,
        string? Notes
        )
    {
    }
}
