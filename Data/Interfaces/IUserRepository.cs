using AuthServices.Data.Entities;

namespace AuthServices.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> CreateAsync(UsersEntity entity);
        Task<bool> DeleteUserAsync(string id);
        Task<List<UsersEntity>> GetAllUsersAsync();
        Task<UsersEntity?> GetUserAsync(string id);
        Task<bool> UpdateUserAsync(UsersEntity entity);
    }
}