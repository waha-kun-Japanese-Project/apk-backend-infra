//using AutoMapper;
//using CommanLib.EventNotification.EmailEvent;
//using MassTransit;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using Dashboard.Domain.Contracts;
//using Dashboard.Domain.Entities;
//using Dashboard.shared.DTOS;
//using Dashboard.ServicesAbstract;
//using Dashboard.Shared.Result;



//namespace Dashboard.Service.Services
//{
//    internal class UserService(IUserRepo userRepo, IHttpContextAccessor httpContextAccesso, IMapper mapper, IPublishEndpoint publish) : IUserService
//    {
//        public async Task<Result> BlockUserAsync(Guid userId)
//        {
//            var user = await userRepo.GetByIdAsync(userId);
//            if (user == null)
//            {
//                return Result.Fail(Error.NotFound("user not found"));
//            }
//            user.Status = UserStatus.Suspended;
//            await userRepo.UpdateAsync(user);
//            return Result.Ok();
//        }

//        public async Task<Result> DeleteUserAsync(Guid userId)
//        {
//            var user = await userRepo.GetByIdAsync(userId);
//            if (user == null)
//            {
//                return Result.Fail(Error.NotFound("User not found"));
//            }
//            //await userRepo.DeleteAsync(user.Id);
//            return Result.Ok();

//        }

//        public async Task<Result<IEnumerable<UserDetailsResponse>>> GetAllUserDetailsAsync()
//        {
//            var users = userRepo.GetAllAsync().Result;
//            if (users == null || !users.Any())
//            {
//                Error.NotFound("No users found");
//            }

//            return mapper.Map<Result<IEnumerable<UserDetailsResponse>>>(users);

//        }

//        public async Task<Result<UserDetailsResponse>> GetUserDetailsAsync()
//        {
//            var user = GetLoggedInUserAsync().Result;
//            if (user is null)
//            {
//                return Error.NotFound();
//            }
//            return mapper.Map<Result<UserDetailsResponse>>(user);
//        }


//        public async Task<Result> UpdateUserDetailsAsync(UserUpdateRequest userUpdate)
//        {
//            var user = await GetLoggedInUserAsync();

//            if (user is null)
//                Error.NotFound("user not found");

//            mapper.Map(userUpdate, user);

//            if (user.Address != null)
//            {
//                user.Address.Village = userUpdate.Village ?? user.Address.Village;
//                user.Address.Region = userUpdate.Region ?? user.Address.Region;
//            }
//            else
//            {
//                user.Address = new Address
//                {
//                    Village = userUpdate.Village,
//                    Region = userUpdate.Region
//                };
//            }

//            await userRepo.UpdateAsync(user);
//            return Result.Ok();
//        }

//        private async Task<AppUser> GetLoggedInUserAsync()
//        {
//            var user = httpContextAccesso.HttpContext?.User;

//            if (user == null || !user.Identity!.IsAuthenticated)
//            {
//                throw new UnauthorizedAccessException("User not authenticated");
//            }

//            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (string.IsNullOrEmpty(userId))
//            {
//                throw new UnauthorizedAccessException("User Id not found in token");
//            }

//            return await userRepo.GetByIdAsync(Guid.Parse(userId)) ?? throw new Exception("User not found");
//        }

//        public async Task<Result> ApprovedUserAsync(Guid userId)
//        {
//            var user = await userRepo.GetByIdAsync(userId);
//            if (user == null)
//            {
//                Error.NotFound("user not found");
//            }
//            user.Status = UserStatus.Approved;
//            await userRepo.UpdateAsync(user);
//            await userRepo.SaveChangesAsync();
//            await publish.Publish(new AccountEvent(user.Email, user.FullName));

//            return Result.Ok();


//        }
//    }
//}
