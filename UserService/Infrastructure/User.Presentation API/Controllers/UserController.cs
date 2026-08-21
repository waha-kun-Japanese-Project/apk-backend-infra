using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.ServicesAbstract;
using User.shared.DTOS;

namespace User.Presentation_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
  
    public class UserController(IUserService userService): ControllerBase
    {
        [HttpGet("details")]
        [Authorize]
        public async Task<IActionResult> GetUserDetails()
        {
            var userDetails = await userService.GetUserDetailsAsync();
            return Ok(userDetails);
        }


        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUserDetails()
        {
            var allUserDetails = await userService.GetAllUserDetailsAsync();
            return Ok(allUserDetails);
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UserUpdateRequest userUpdate)
        {
            await userService.UpdateUserDetailsAsync(userUpdate);
            return NoContent();
        }



        [HttpPut("Block/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUser(Guid userId)
        {
            await userService.BlockUserAsync(userId);
            return NoContent();
        }

        [HttpPut("Approved/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>AprrovedUser(Guid userId)
        {
            await userService.ApprovedUserAsync(userId);
            return NoContent();
        }


        [HttpDelete("Delete/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await userService.DeleteUserAsync(userId);
            return NoContent();
        }

     

      

    }
}
