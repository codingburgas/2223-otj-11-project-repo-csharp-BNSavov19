using InMotion.Data.Models.Activities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InMotion.Data.Models.Auth;

/// <summary>
/// User model.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Gets or sets first name of the user.
    /// </summary>
    [Required]
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets last name of the user.
    /// </summary>
    [Required]
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the image url of the user.
    /// </summary>
    [Required]
    public string? AvatarImageUrl { get; set; }

    public ICollection<Activity>? Activities { get; set; }

    public ICollection<UserAllergies>? Allergies { get; set; }

    public ICollection<UserAssistance>? Assistances { get; set; }

    public ICollection<UserDiet>? Diets { get; set; }

    public ICollection<UserDisability>? Disabilities { get; set; }

    public ICollection<UserHatesSports>? HatesSports { get; set; }

    public ICollection<UserLikesSports>? LikesSports { get; set; }

    public ICollection<UserMedical>? Medicals { get; set; }

    public ICollection<UserQuting>? Qutings { get; set; }

    public Gender Gender { get; set; }

    public string? Location { get; set; }

    public bool? IsTryingToLoseWeight { get; set; }

    public int? Age { get; set; }
}