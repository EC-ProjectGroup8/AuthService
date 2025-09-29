using AuthServices.Data.Dto;
using AuthServices.Data.Entities;
using AuthServices.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace AuthServices.Services.Interfaces;

public interface IUserService
{
    Task<bool> CreateUser(RegisterDto model);
    Task<UserReturnData?> GetUserById(string id);
    Task<IEnumerable<UserReturnData>> GetAllUsers();
    Task<bool> DeleteUser(string id);
    Task<bool> UpdateUser(UsersEntity entity);
    Task<UsersEntity?> GetUserByEmail(string email);
    Task<bool> UserExistsByEmailAsync(string email);
    Task RequestPasswordReset(string email);
    Task<bool> ChangeForgottenPassword(ResetPasswordRequest request);
}