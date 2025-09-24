using AuthServices.Data.Dto;
using AuthServices.Data.Entities;
using AuthServices.Models;

namespace AuthServices.Services.Interfaces
{
    public interface IUserServices
    {
        Task<bool> CreateUser(RegisterDto model);
        Task<UserReturnData?> GetUserById(string id);
        Task<bool> GetUserByEmail(string email);
        Task<IEnumerable<UserReturnData>> GetAllUsers();
        Task<bool> DeleteUser(string id);
        Task<bool> UpdateUser(UsersEntity entity);
    }
}