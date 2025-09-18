using AuthServices.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthServices.Data.Context
{
    public class DataContext(DbContextOptions options) : IdentityDbContext<UsersEntity>(options)
    {

    }
}
