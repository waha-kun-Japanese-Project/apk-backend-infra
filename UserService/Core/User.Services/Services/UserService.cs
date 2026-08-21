using AutoMapper;
using CommanLib.EventNotification.EmailEvent;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;


using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Contract;
using User.Domain.Entities;
using User.ServicesAbstract;
using User.shared.DTOS;


namespace User.Services.Services
{
    internal class UserService(IUserRepo userRepo, IHttpContextAccessor httpContextAccesso, IMapper mapper, IPublishEndpoint publish) : IUserService
    {
        public async Task BlockUserAsync(Guid userId)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.Status = UserStatus.Suspended;
            await userRepo.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            await userRepo.DeleteAsync(user.Id);


        }

        public Task<IEnumerable<UserDetailsResponse>> GetAllUserDetailsAsync()
        {
            var users = userRepo.GetAllAsync().Result;
            return Task.FromResult(mapper.Map<IEnumerable<UserDetailsResponse>>(users));

        }

        public Task<UserDetailsResponse> GetUserDetailsAsync()
        {
            var user = GetLoggedInUserAsync().Result;
            return Task.FromResult(mapper.Map<UserDetailsResponse>(user));
        }


        public async Task UpdateUserDetailsAsync(UserUpdateRequest userUpdate)
        {
            var user = await GetLoggedInUserAsync();

            if (user is null)
                throw new KeyNotFoundException("User not found.");

            mapper.Map(userUpdate, user);

            if (user.Address != null)
            {
                user.Address.Village = userUpdate.Village ?? user.Address.Village;
                user.Address.Region = userUpdate.Region ?? user.Address.Region;
            }
            else
            {
                user.Address = new Address
                {
                    Village = userUpdate.Village,
                    Region = userUpdate.Region
                };
            }

            await userRepo.UpdateAsync(user);
        }

        private async Task<AppUser> GetLoggedInUserAsync()
        {
            var user = httpContextAccesso.HttpContext?.User;

            if (user == null || !user.Identity!.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User Id not found in token");
            }

            return await userRepo.GetByIdAsync(Guid.Parse(userId)) ?? throw new Exception("User not found");
        }

        public async Task ApprovedUserAsync(Guid userId)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.Status = UserStatus.Approved;
            await userRepo.UpdateAsync(user);
            await userRepo.SaveChangesAsync();
            await publish.Publish(new AccountEvent(user.Email, user.FullName));



            ;
        }
    }
}
