using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthServices.Data.Entities
{
    public class UsersEntity : IdentityUser
    {
        [Required]
        [ProtectedPersonalData]
        public string FirstName { get; set; } = null!;
        [Required]
        [ProtectedPersonalData]
        public string LastName { get; set; } = null!;
    }
}
