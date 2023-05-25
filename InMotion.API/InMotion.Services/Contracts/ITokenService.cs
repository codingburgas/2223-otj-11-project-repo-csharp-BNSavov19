using InMotion.Data.Models.Auth;
using InMotion.Services.Models.Auth;
using System.Security.Claims;

namespace InMotion.Services.Contracts;

/// <summary>
/// Interface of the token service.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Creates an access and refresh tokens.
    /// </summary>
    /// <param name="email">Email of the user.</param>
    /// <returns>The new tokens.</returns>
    Task<TokenModel> CreateTokensForUserAsync(string? email);

    /// <summary>
    /// Generates an email confirmation token.
    /// </summary>
    /// <param name="email">Email of the user.</param>
    /// <returns>Token.</returns>
    Task<string> GenerateEmailConfirmationTokenAsync(string? email);

    /// <summary>
    /// Create a new token from expired one.
    /// </summary>
    /// <param name="tokenModule">The previous tokens.</param>
    /// <returns>The new tokens.</returns>
    Task<TokenModel> CreateNewTokensAsync(TokenInputModule tokenModule);

    /// <summary>
    /// Generates a password reset token.
    /// </summary>
    /// <param name="email">Email of the user.</param>
    /// <returns>Token.</returns>
    Task<string> GeneratePasswordResetTokenAsync(string? email);

    /// <summary>
    /// Saves refresh token to the DB.
    /// </summary>
    /// <param name="refreshToken">Refresh token.</param>
    /// <returns>Task.</returns>
    Task SaveRefreshTokenAsync(RefreshToken refreshToken);

    /// <summary>
    /// Get Refresh token form the database.
    /// </summary>
    /// <param name="token">Refresh token.</param>
    /// <returns>The token or null.</returns>
    Task<RefreshToken?> GetRefreshTokenAsync(string? token);

    /// <summary>
    /// Deletes refresh token from the Database.
    /// </summary>
    /// <param name="refreshToken">Refresh token to be deleted.</param>
    /// <returns>Task.</returns>
    Task DeleteRefreshTokenAsync(RefreshToken refreshToken);

    /// <summary>
    /// Deprecated! Extract Claims from token.
    /// </summary>
    /// <param name="jwtToken">The JWT Token.</param>
    /// <returns>Claims.</returns>
    [Obsolete("This function is deprecated, please use ICurrentUser instead.")]
    IEnumerable<Claim> ExtractClaims(string jwtToken);
}