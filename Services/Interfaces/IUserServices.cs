using AuthServices.Data.Entities;

namespace AuthServices.Services.Interfaces
{
    public interface IUserServices
    {
        Task<bool> CreateUser(UsersEntity entity);
        Task<bool> DeleteUser(string id);
        Task<List<UsersEntity>> GetAllUsers();
        Task<UsersEntity?> GetUserById(string id);
        Task<bool> UpdateUser(UsersEntity entity);
    }
}