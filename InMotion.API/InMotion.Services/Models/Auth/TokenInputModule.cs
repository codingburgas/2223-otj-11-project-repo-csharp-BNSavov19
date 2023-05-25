namespace InMotion.Services.Models.Auth;

/// <summary>
/// Token input model.
/// </summary>
public class TokenInputModule
{
    /// <summary>
    /// Gets or sets access token.
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets refresh token.
    /// </summary>
    public string? RefreshToken { get; set; }
}