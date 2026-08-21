using CommanLib.Result;
using Dashboard.shared.DTOS;


namespace Dashboard.ServicesAbstract
{
    public interface IUserService
    {
        Task<Result<UserDetailsResponse>> GetUserDetailsAsync();
        Task<Result<IEnumerable<UserDetailsResponse>>> GetAllUserDetailsAsync();
        Task<Result> UpdateUserDetailsAsync(UserUpdateRequest userUpdate);
        Task<Result> DeleteUserAsync(Guid userId);
        Task<Result> BlockUserAsync(Guid userId);
        Task<Result> ApprovedUserAsync(Guid userId);

    


    }
}
