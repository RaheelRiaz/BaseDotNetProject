using System.ComponentModel.DataAnnotations;

namespace CleanMvcApp.Web.ViewModels;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
