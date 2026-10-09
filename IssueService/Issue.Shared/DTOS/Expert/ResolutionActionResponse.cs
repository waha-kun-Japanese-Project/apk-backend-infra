using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.Expert
{
    public record ResolutionActionResponse
    {
        public Guid Id { get; init; }
        public string ActionRepair { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public string FilePath { get; init; } = string.Empty;
    }
}
