using System.ComponentModel.DataAnnotations;

namespace InMotion.WebHost.Models.Auth;

/// <summary>
/// Model for registering a new user.
/// </summary>
public class ForgotPasswordModel
{
    /// <summary>
    ///   Gets or sets the email address of the user.
    /// </summary>
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}