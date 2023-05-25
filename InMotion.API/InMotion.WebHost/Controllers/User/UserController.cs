using AutoMapper;
using InMotion.Data.Models.Auth;
using InMotion.Services.Contracts;
using InMotion.Services.Models.Auth;
using InMotion.WebHost.Models;
using LR.Services.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InMotion.WebHost.Controllers.User;

/// <summary>
/// Controller for managing users.
/// </summary>
[Route("api/user")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    private readonly IAuthService authService;
    private readonly ICurrentUser currentUser;
    private readonly IMapper mapper;
    private readonly ILogger<UserController> logger;
    private readonly IFileService fileSerivce;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="userService">User Service.</param>
    /// <param name="authService">Auth Service.</param>
    /// <param name="currentUser">Current User.</param>
    /// <param name="mapper">Mapper.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="fileSerivce">File service.</param>
    public UserController(
        IUserService userService,
        IAuthService authService,
        ICurrentUser currentUser,
        IMapper mapper,
        ILogger<UserController> logger,
        IFileService fileSerivce)
    {
        this.userService = userService;
        this.authService = authService;
        this.currentUser = currentUser;
        this.mapper = mapper;
        this.logger = logger;
        this.fileSerivce = fileSerivce;
    }

    /// <summary>
    /// Gets info of the current user.
    /// </summary>
    /// <returns>Response with the result.</returns>
    [HttpGet("current")]
    public async Task<ActionResult<UsersVM>> GetUserMeAsync()
    {
        var result = await this.userService.GetUserByIdAsync(this.currentUser.UserId);

        if (result is null)
        {
            return this.NotFound();
        }

        return result;
    }

    /// <summary>
    /// Gets info of the current user.
    /// </summary>
    /// <returns>Response with the result.</returns>
    [HttpGet("currentFull")]
    public async Task<ActionResult<UsersVM>> GetUserMeFullAsync()
    {
        var result = await this.userService.GetFullUserByIdAsync(this.currentUser.UserId);

        if (result is null)
        {
            return this.NotFound();
        }

        return result;
    }

    /// <summary>
    /// Changes the password of the current user.
    /// </summary>
    /// <param name="oldPassword">Old password.</param>
    /// <param name="newPassword">New password.</param>
    /// <returns>Response with the result.</returns>
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangerUserPasswordAsync([FromForm] string oldPassword, [FromForm] string newPassword)
    {
        this.logger.LogInformation("Changing password for user with id {UserId}", this.currentUser.UserId);

        var user = await this.userService.GetUserByIdAsync(this.currentUser.UserId);

        if (!await this.authService.CheckIsPasswordCorrectAsync(user!.Email, oldPassword))
        {
            this.logger.LogWarning("Changing password for user with id {UserId} failed. Old password is incorrect.", this.currentUser.UserId);

            return this.BadRequest(new Response
            {
                Status = "password-change-failed",
                Message = "Old password is incorrect",
            });
        }

        var result = await this.userService.ChangePasswordAsync(this.currentUser.UserId, newPassword);

        if (!result.Succeeded)
        {
            this.logger.LogWarning("Changing password for user with id {UserId} failed. {Errors}", this.currentUser.UserId, result.Errors.First().Description);

            return this.BadRequest(new Response
            {
                Status = "password-change-failed",
                Message = result.Errors.FirstOrDefault()?.Description,
            });
        }

        this.logger.LogInformation("Changing password for user with id {UserId} succeeded.", this.currentUser.UserId);

        return this.Ok(new Response
        {
            Status = "password-changed-success",
            Message = "Password change successful",
        });
    }

    /// <summary>
    /// Updates the current user.
    /// </summary>
    /// <param name="model">User Update Model.</param>
    /// <returns>Response with the result.</returns>
    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync([FromForm] UserUM model)
    {
        this.logger.LogInformation("Updating user with id {UserId}", this.currentUser.UserId);

        var user = await this.userService.GetUserByIdAsync(this.currentUser.UserId);

        if (user is null)
        {
            this.logger.LogWarning("Updating user with id {UserId} failed. User not found.", this.currentUser.UserId);

            return this.NotFound(new Response
            {
                Status = "user-update-failed",
                Message = "User not found",
            });
        }

        string? imageUrl = null;

        if (model.AvatarImage is not null)
        {
            imageUrl = await this.fileSerivce.SaveImageAsync(model.AvatarImage, "useravatars");
        }

        var result = await this.userService.UpdateUserAsync(user, this.mapper.Map<RegisterModel>(model), imageUrl);

        if (!result)
        {
            this.logger.LogWarning("Updating user with id {UserId} failed.", this.currentUser.UserId);

            return this.BadRequest(new Response
            {
                Status = "update-failed",
            });
        }

        this.logger.LogInformation("Updating user with id {UserId} succeeded.", this.currentUser.UserId);

        return this.Ok(new Response
        {
            Status = "update-success",
            Message = "User updated successfully",
        });
    }

    /// <summary>
    /// Search for user.
    /// </summary>
    /// <param name="searchQuery"></param>
    /// <returns></returns>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<UsersVM>>> SearchUsersAsync(string searchQuery)
    {
        var result = await this.userService.SearchUsersAsync(searchQuery);

        return Ok(result);
    }

    /// <summary>
    /// Gets user by id.
    /// </summary>
    /// <param name="userId">Id fo the user</param>
    /// <returns>User VM</returns>
    [HttpGet]
    public async Task<ActionResult<UsersVM>> GetUserByIdAsync(string userId)
    {
        var result = await this.userService.GetUserByIdAsync(userId);

        if (result is null)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    /// <summary>
    /// Revert user avatar to original.
    /// </summary>
    /// <returns>IActionResult.</returns>
    [HttpDelete("avatar")]
    public async Task<IActionResult> RevertUserImageToDefault()
    {
        await userService.RevertAvatarImageAsync(this.currentUser.UserId);
        return this.Ok();
    }
}
