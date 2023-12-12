using System.ComponentModel.DataAnnotations;

namespace StudyBuddyMVC.Models
{
    public class EditProfileViewModel
    {
        [Display(Name = "First Name"), Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name"), Required]
        public string LastName { get; set; }

        [Display(Name = "Email Address"), Required]
        public string EmailAddress { get; set; }

        [Display(Name = "Username"), Required]
        public string Username { get; set; }

        [Required, Display(Name = "Current password")]
        public string? OldPassword { get; set; }

        [Compare(nameof(ConfirmPassword)), Display(Name = "New password")]
        public string? NewPassword { get; set; }

        [Display(Name = "Confirm your password.")]
        public string? ConfirmPassword { get; set; }
    }
}
