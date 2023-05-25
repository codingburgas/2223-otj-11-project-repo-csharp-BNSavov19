using InMotion.Services.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LR.Services.Models.Auth;

/// <summary>
/// Register model for the user.
/// </summary>
public class RegisterModel
{
    /// <summary>
    /// Gets or sets first name of the user.
    /// </summary>
    [Required(ErrorMessage = "First name is required")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets last name of the user.
    /// </summary>
    [Required(ErrorMessage = "Last name is required")]
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets email of the user.
    /// </summary>
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets password of the user.
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the image for the avatar of the user.
    /// </summary>
    [Image]
    public IFormFile? AvatarImage { get; set; }
}