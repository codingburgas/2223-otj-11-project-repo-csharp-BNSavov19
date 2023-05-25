using InMotion.Services.Contracts;
using InMotion.Services.Implentations;
using Microsoft.Extensions.DependencyInjection;

namespace InMotion.Services;

/// <summary>
/// Dependency Injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add Services.
    /// </summary>
    /// <param name="services">Services.</param>
    public static void AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped<IFileService, FileService>()
            .AddScoped<IActivityService, ActivityService>()
            .AddScoped<IMachineLearningService, MachineLearningService>()
            .AddScoped<ILanguageService, LanguageService>();
    }
}