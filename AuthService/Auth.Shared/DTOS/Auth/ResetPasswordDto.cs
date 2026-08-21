using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Shared.DTOS.Auth
{
    public class ResetPasswordDto
    {
        [Required]
        public string Password { get; set; }
        [Required,Editable(false),Compare("Password",ErrorMessage ="Password and Confirmed Password do not match")]
        public string ConfemedPassword { get; set; }

        public string Email { get; set; }
        public string token { get; set; }
    }
}
