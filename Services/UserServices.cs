using AuthServices.Data.Context;
using AuthServices.Data.Entities;
using AuthServices.Data.Interfaces;
using AuthServices.Services.Interfaces;

namespace AuthServices.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _repository;

        public UserServices(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateUser(UsersEntity entity)
        {
            if (entity == null) return false;
            return await _repository.CreateAsync(entity);
            {

            }
        }

        public async Task<UsersEntity?> GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return await _repository.GetUserAsync(id);
        }

        public async Task<List<UsersEntity>> GetAllUsers()
        {
            return await _repository.GetAllUsersAsync();
        }


        public async Task<bool> UpdateUser(UsersEntity entity)
        {
            if (entity == null) return false;
            return await _repository.UpdateUserAsync(entity);
        }

        public async Task<bool> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            return await _repository.DeleteUserAsync(id);
        }


    }
}
