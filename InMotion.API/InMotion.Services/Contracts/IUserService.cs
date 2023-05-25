using InMotion.Services.Models.Auth;
using LR.Services.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace InMotion.Services.Contracts;

/// <summary>
/// Interface of the user service.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets a user by id.
    /// </summary>
    /// <param name="id">Id of the user.</param>
    /// <returns>User as UserVM.</returns>
    Task<UsersVM?> GetUserByIdAsync(string id);

    /// <summary>
    /// Get all of the user.
    /// </summary>
    /// <returns>List of all users.</returns>
    List<UsersVM> GetAllUsers();

    /// <summary>
    /// Gets a user's email by id.
    /// </summary>
    /// <param name="id">Id of the user.</param>
    /// <returns>Email of the user.</returns>
    Task<string> GetUserEmailByIdAsync(string id);

    /// <summary>
    /// Validates the user one time token.
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    /// <param name="token">Token.</param>
    /// <param name="type">Type.</param>
    /// <param name="purpose">Purpose of the token.</param>
    /// <returns>Is the token valid.</returns>
    Task<bool> ValidateOneTimeTokenForUserAsync(string userId, string token, string type, string purpose);

    /// <summary>
    /// Change a user password.
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    /// <param name="newPassword">New password.</param>
    /// <returns>IdentityResult.</returns>
    Task<IdentityResult> ChangePasswordAsync(string userId, string newPassword);

    /// <summary>
    /// Updates the user's info.
    /// </summary>
    /// <param name="oldUserInfo">Old info of the user.</param>
    /// <param name="newUserInfo">New info of the user.</param>
    /// <param name="newAvatarImageUrl">URl of the avatar image.</param>
    /// <returns>Was it successful.</returns>
    Task<bool> UpdateUserAsync(UsersVM oldUserInfo, RegisterModel newUserInfo, string? newAvatarImageUrl = null);

    /// <summary>
    /// Generates one time token for user.
    /// </summary>
    /// <param name="email">Email of the user.</param>
    /// <param name="type">Type of the token.</param>
    /// <param name="purpose">Purpose of the token.</param>
    /// <returns>The token.</returns>
    Task<string> GenerateOneTimeTokenForUserAsync(string email, string type, string purpose);

    /// <summary>
    /// Gets user by email.
    /// </summary>
    /// <param name="email">Email.</param>
    /// <returns>The user.</returns>
    Task<UsersVM> GetUserByEmailAsync(string email);

    /// <summary>
    /// Search for user.
    /// </summary>
    /// <param name="query">Query.</param>
    /// <returns>List with matching users.</returns>
    Task<IEnumerable<UsersVM>> SearchUsersAsync(string query);

    /// <summary>
    /// Reverts avatar image to original
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    /// <returns>Task.</returns>
    Task RevertAvatarImageAsync(string userId);
    
    Task<UsersVM> GetFullUserByIdAsync(string userId);
}