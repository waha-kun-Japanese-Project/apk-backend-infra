using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Shared.DTOS.Auth
{
    public class ForgetPassowrdDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string ClinetUrl { get; set; }
    }
}
