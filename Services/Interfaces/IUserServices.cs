using AuthServices.Data.Dto;
using AuthServices.Data.Entities;
using AuthServices.Models;

namespace AuthServices.Services.Interfaces
{
    public interface IUserServices
    {
        Task<bool> CreateUser(RegisterDto model);
        Task<UserReturnData?> GetUserById(string id);
        Task<IEnumerable<UserReturnData>> GetAllUsers();
        Task<bool> DeleteUser(string id);
        Task<bool> UpdateUser(UsersEntity entity);
        Task<UsersEntity?> GetUserByEmail(string email);
        Task<bool> CheckUserByEmail(string email);
    }
}