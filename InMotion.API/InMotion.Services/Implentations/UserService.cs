using AutoMapper;
using InMotion.Data.Data;
using InMotion.Data.Models.Auth;
using InMotion.Services.Contracts;
using InMotion.Services.Models.Auth;
using LR.Services.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMotion.Services.Implentations;

/// <summary>
/// User service.
/// </summary>
internal class UserService : IUserService
{
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    private readonly ApplicationDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userManager">User Manager.</param>
    /// <param name="mapper">AutoMapper.</param>
    public UserService(UserManager<User> userManager, IMapper mapper, ApplicationDbContext context)
    {
        this.userManager = userManager;
        this.mapper = mapper;
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<UsersVM?> GetUserByIdAsync(string id)
    {
        var user = await this.userManager.FindByIdAsync(id);

        if (user == null)
        {
            return null;
        }

        return this.mapper.Map<UsersVM>(user);
    }

    /// <inheritdoc/>
    public List<UsersVM> GetAllUsers()
    {
        List<UsersVM> usersVM = new();

        this.userManager.Users.ToList().ForEach(
            async user =>
            {
                usersVM.Add(this.mapper.Map<UsersVM>(user));
            });

        return usersVM;
    }

    /// <inheritdoc/>
    public async Task<string> GetUserEmailByIdAsync(string id)
    {
        var user = await this.userManager.FindByIdAsync(id);

        return await this.userManager.GetEmailAsync(user);
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> ChangePasswordAsync(string userId, string newPassword)
    {
        var user = await this.userManager.FindByIdAsync(userId);

        var token = await this.userManager.GeneratePasswordResetTokenAsync(user);

        return await this.userManager.ResetPasswordAsync(user, token, newPassword);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateUserAsync(UsersVM oldUserInfo, RegisterModel newUserInfo, string? avatarImageUrl)
    {
        var user = await this.userManager.FindByEmailAsync(oldUserInfo.Email);

        if (newUserInfo.Password is not null)
        {
            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);

            var result = await this.userManager.ResetPasswordAsync(user, token, newUserInfo.Password);

            if (!result.Succeeded)
            {
                return false;
            }
        }

        if (avatarImageUrl is not null)
        {
            user.AvatarImageUrl = avatarImageUrl;
        }

        user.FirstName = newUserInfo.FirstName;
        user.LastName = newUserInfo.LastName;
        user.Email = newUserInfo.Email;
        user.UserName = newUserInfo.Email;

        await this.userManager.UpdateAsync(user);
        return true;
    }

    /// <inheritdoc/>
    public async Task<string> GenerateOneTimeTokenForUserAsync(string email, string type, string purpose)
    {
        var user = await this.userManager.FindByEmailAsync(email);

        return await this.userManager.GenerateUserTokenAsync(user, type, purpose);
    }

    /// <inheritdoc/>
    public async Task<bool> ValidateOneTimeTokenForUserAsync(string userId, string token, string type, string purpose)
    {
        var user = await this.userManager.FindByIdAsync(userId);

        return await this.userManager.VerifyUserTokenAsync(user, type, purpose, token);
    }

    /// <inheritdoc/>
    public async Task<UsersVM> GetUserByEmailAsync(string email)
    {
        return this.mapper.Map<UsersVM>(await this.userManager.FindByEmailAsync(email));
    }

    public async Task<IEnumerable<UsersVM>> SearchUsersAsync(string query)
    {
        var users = this.userManager.Users.Where(u => u.FirstName.Contains(query) || u.LastName.Contains(query));

        var usersVM = new List<UsersVM>();

        foreach (var user in users)
        {
            usersVM.Add(this.mapper.Map<UsersVM>(user));
        }

        return usersVM;
    }

    public async Task RevertAvatarImageAsync(string userId)
    {
        var user = await this.userManager.FindByIdAsync(userId);

        user.AvatarImageUrl = $"https://avatars.dicebear.com/api/initials/{user.FirstName![0]}{user.LastName![0]}{Guid.NewGuid()}.svg".Replace("-", "");

        await this.userManager.UpdateAsync(user);
    }

    public async Task<UsersVM> GetFullUserByIdAsync(string userId)
    {
        var user = await this.context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.Activities)
            .Include(u => u.Allergies)
            .Include(u => u.Assistances)
            .Include(u => u.Diets)
            .Include(u => u.Disabilities)
            .Include(u => u.HatesSports)
            .Include(u => u.LikesSports)
            .Include(u => u.Medicals)
            .Include(u => u.Qutings)
            .FirstOrDefaultAsync();
           

        return this.mapper.Map<UsersVM>(user);
    }
}

