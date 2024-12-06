using System.ComponentModel.DataAnnotations;

namespace MinesweeperMVC.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Sex { get; set; } = string.Empty;

        [Required]
        public int Age { get; set; }

        [Required]
        public string State { get; set; } = string.Empty;

        [Required, EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}
