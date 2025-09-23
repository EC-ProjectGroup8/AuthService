using AuthServices.Data.Context;
using AuthServices.Data.Dto;
using AuthServices.Data.Entities;
using AuthServices.Data.Interfaces;
using AuthServices.Models;
using AuthServices.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthServices.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _repository;
        private readonly UserManager<UsersEntity> _userManager;

        public UserServices(IUserRepository repository, UserManager<UsersEntity> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<bool> CreateUser(RegisterDto model)
        {
            if (model == null) return false;
            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing != null) return false;

            var user = new UsersEntity
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            return result.Succeeded;

        }

        public async Task<UserReturnData?> GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var existing = await _userManager.FindByIdAsync(id);
            if (existing == null) return null;
            var result = new UserReturnData
            {
                Id = existing.Id,
                Email = existing.Email!,
                FirstName = existing.FirstName,
                LastName = existing.LastName
            };
            return result;

        }

        public async Task<IEnumerable<UserReturnData>> GetAllUsers()
        {
            return await _userManager.Users
                .Select(user => new UserReturnData
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!
                })
                .ToListAsync();
        }


        public async Task<bool> UpdateUser(UsersEntity entity)
        {
            if (entity == null) return false;
            var existing = await _userManager.FindByIdAsync(entity.Id);
            if (existing == null) return false;
            existing.FirstName = entity.FirstName;
            existing.LastName = entity.LastName;
            existing.Email = entity.Email;
            existing.UserName = entity.Email;
            var result = await _userManager.UpdateAsync(existing);
            if (!result.Succeeded) return false;
            else return true;
        }

        public async Task<bool> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            var existing = await _userManager.FindByIdAsync(id);
            if (existing == null) return false;
            var result = await _userManager.DeleteAsync(existing);
            return result.Succeeded;
        }


    }
}
