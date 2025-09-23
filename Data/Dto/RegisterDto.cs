using System.ComponentModel.DataAnnotations;

namespace AuthServices.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "First Name", Prompt = "First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = null!;





        [Required(ErrorMessage = "Required")]
        [Display(Name = "Last Name", Prompt = "Last Name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = null!;





        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email", Prompt = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;





        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, and one number.")]
        [Display(Name = "Password", Prompt = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;





        [Required(ErrorMessage = "Required")]
        [Display(Name = "Repeat Password", Prompt = "Repeat Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

    }
}
