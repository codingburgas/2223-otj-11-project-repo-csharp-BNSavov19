using InMotion.Data.Models.Activities;

namespace InMotion.Services.Models.Auth;

/// <summary>
/// View model for the user.
/// </summary>
public class UsersVM
{
    /// <summary>
    /// Gets or sets the id of the user.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the role of the user.
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Gets or sets the image url of the user.
    /// </summary>
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