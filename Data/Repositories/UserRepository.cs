using AuthServices.Data.Context;
using AuthServices.Data.Entities;
using AuthServices.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthServices.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly DbSet<UsersEntity> _dbSet;

        public UserRepository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<UsersEntity>();
        }


        public async Task<bool> CreateAsync(UsersEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UsersEntity?> GetUserAsync(string id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UsersEntity>> GetAllUsersAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(UsersEntity entity)
        {
            try
            {
                var user = await _dbSet.FindAsync(entity.Id);
                if (user == null) return false;
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;
                user.Email = entity.Email;
                user.UserName = entity.UserName;
                _dbSet.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            try
            {
                var user = await _dbSet.FindAsync(id);
                if (user == null) return false;
                _dbSet.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
