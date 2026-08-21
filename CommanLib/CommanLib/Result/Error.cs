using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommanLib.Result
{
    public class Error
    {
        public string Code { get; }


        public string Description { get; }

        public ErrorType Type { get; }

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        // Static factory methods to create errors

        public static Error Failure(string code = "General.Failure",
            string description = "A failure has occurred.")
            => new(code, description, ErrorType.Failure);

        public static Error Validation(string code = "General.Validation",
            string description = "A validation error has occurred.") =>
            new(code, description, ErrorType.Validation);

        public static Error NotFound(string code = "General.NotFound",
            string description = "A 'Not Found' error has occurred.") =>
            new(code, description, ErrorType.NotFound);

        public static Error Conflict(string code = "General.Conflict",
            string description = "A conflict error has occurred.") =>
            new(code, description, ErrorType.Conflict);

        public static Error Unauthorized(string code = "General.Unauthorized",
            string description = "An unauthorized error has occurred.") =>
            new(code, description, ErrorType.Unauthorized);
    }
}
