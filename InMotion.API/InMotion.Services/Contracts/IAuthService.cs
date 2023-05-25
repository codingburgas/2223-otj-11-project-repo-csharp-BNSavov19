using LR.Services.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace InMotion.Services.Contracts;

/// <summary>
/// Interface for authentication service.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Checks if user exists in the Database.
    /// </summary>
    /// <param name="email">Email of the user.</param>
    /// <returns>Does user exists.</returns>
    Task<bool> CheckIfUserExistsAsync(string? email);

    /// <summary>
    /// Checks if had verified his email.
    /// </summary>
    /// <param name="email">Email of the user.</param>
    /// <returns>Is email verified.</returns>
    Task<bool> CheckIfUserHasVerifiedEmailAsync(string? email);

    /// <summary>
    /// Checks if user's provided password is correct.
    /// </summary>
    /// <param name="email">Email of the user.</param>
    /// <param name="password">Password of the user.</param>
    /// <returns>Is password correct.</returns>
    Task<bool> CheckIsPasswordCorrectAsync(string? email, string? password);

    /// <summary>
    /// Saves user to the database.
    /// </summary>
    /// <param name="registerModel">User info.</param>
    /// <param name="avatarImageUrl">Url if the avatar image</param>
    /// <param name="isAdmin">Role of the user.</param>
    /// <returns>Is creating successful.</returns>
    Task<Tuple<bool, string?>> CreateUserAsync(RegisterModel registerModel, string avatarImageUrl, bool isAdmin = false);

    /// <summary>
    /// Confirms the email of the user.
    /// </summary>
    /// <param name="email">Email of the user.</param>
    /// <param name="token">Token.</param>
    /// <returns>IdentityResult.</returns>
    Task<IdentityResult> ConfirmEmailAsyncAsync(string email, string token);

    /// <summary>
    /// Resets the password of the user.
    /// </summary>
    /// <param name="email">Email of the user.</param>
    /// <param name="token">Token for the password reseting.</param>
    /// <param name="password">New password for the user.</param>
    /// <returns>IdentityResult.</returns>
    Task<IdentityResult> ResetPasswordAsync(string email, string token, string password);

    /// <summary>
    /// Checks if the user is admin.
    /// </summary>
    /// <param name="email">Email of the user.</param>
    /// <returns>Is user an admin.</returns>
    Task<bool> CheckIsAdminAsync(string email);
}