using InMotion.Data.Models.Auth;
using InMotion.Services.Contracts;
using LR.Services.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace InMotion.Services.Implentations;

/// <summary>
/// Authentication Service.
/// </summary>
internal class AuthService : IAuthService
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    /// <param name="userManager">User manager.</param>
    /// <param name="signInManager">SignIn Manager.</param>
    /// <param name="roleManager">Role manager.</param>
    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
    }

    /// <inheritdoc/>
    public async Task<bool> CheckIfUserExistsAsync(string? email)
    {
        return await this.userManager.FindByEmailAsync(email) != null;
    }

    /// <inheritdoc/>
    public async Task<bool> CheckIsPasswordCorrectAsync(string? email, string? password)
    {
        var user = await this.userManager.FindByEmailAsync(email);

        return !(user == null || !await this.userManager.CheckPasswordAsync(user, password));
    }

    /// <inheritdoc/>
    public async Task<Tuple<bool, string?>> CreateUserAsync(RegisterModel registerModel, string avatarImageUrl, bool isAdmin)
    {
        User user = new()
        {
            Email = registerModel.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerModel.Email,
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName,
            AvatarImageUrl = avatarImageUrl
        };

        var result = await this.userManager.CreateAsync(user, registerModel.Password);

        if (!result.Succeeded)
        {
            return new(false, result.Errors.FirstOrDefault()?.Description);
        }

        if (isAdmin)
        {
            if (!await this.roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await this.roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            if (await this.roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await this.userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
        }
        else
        {
            if (!await this.roleManager.RoleExistsAsync(UserRoles.User))
            {
                await this.roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if (await this.roleManager.RoleExistsAsync(UserRoles.User))
            {
                await this.userManager.AddToRoleAsync(user, UserRoles.User);
            }
        }

        return new(true, null);
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> ConfirmEmailAsyncAsync(string email, string token)
    {
        var user = await this.userManager.FindByEmailAsync(email);

        return await this.userManager.ConfirmEmailAsync(user, token);
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string password)
    {
        var user = await this.userManager.FindByEmailAsync(email);

        return await this.userManager.ResetPasswordAsync(user, token, password);
    }

    /// <inheritdoc/>
    public async Task<bool> CheckIsAdminAsync(string email)
    {
        var user = await this.userManager.FindByEmailAsync(email);

        var roleList = await this.userManager.GetRolesAsync(user);

        return roleList.Contains(UserRoles.Admin);
    }

    /// <inheritdoc/>
    public async Task<bool> CheckIfUserHasVerifiedEmailAsync(string? email)
    {
        var user = await this.userManager.FindByEmailAsync(email);

        return !(user == null || !await this.signInManager.CanSignInAsync(user));
    }
}
