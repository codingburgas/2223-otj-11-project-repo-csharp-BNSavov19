using InMotion.Data.Models.Activities;
using InMotion.Services.Contracts;
using InMotion.Services.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace InMotion.WebHost.Controllers.Activities;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ActivityController : ControllerBase
{
    private readonly ILanguageService languageService;
    private readonly IActivityService activityService;
    private readonly ICurrentUser currentUser;
    private readonly IUserService userService;
    private readonly IMachineLearningService machineLearningService;

    public ActivityController(
        ILanguageService languageService,
        IActivityService activityService,
        ICurrentUser currentUser,
        IUserService userService,
        IMachineLearningService machineLearningService)
    {
        this.languageService = languageService;
        this.activityService = activityService;
        this.currentUser = currentUser;
        this.userService = userService;
        this.machineLearningService = machineLearningService;
    }
    
    [HttpPost]
    [Route("GetUserInterests")]

    public async Task<ActionResult<UsersVM>> GetUserInterest([FromForm] string text)
    {
        var userInterests = await this.languageService.GetUserInterest(text);
        
        await this.languageService.SaveUserInterests(userInterests, currentUser.UserId);

        var user = await this.userService.GetUserByIdAsync(currentUser.UserId);
        
        return this.Ok(user);
    }

    [HttpPost]
    [Route("GetUserChallenge")]
    public async Task<ActionResult<UsersVM>> GetUserChallenge([FromForm] ActivityType type)
    {
        var challenge = await machineLearningService.GenerateChallengeForUser(currentUser.UserId, type);

        await activityService.AddActivity(currentUser.UserId, challenge, type);

        var user = await this.userService.GetFullUserByIdAsync(currentUser.UserId);

        return this.Ok(user);
    }

    [HttpPost]
    [Route("CompleteChallenge")]
    public async Task<IActionResult> CompleteChallenge([FromForm] string challengeId)
    {
        bool isSuccessful = await activityService.CompleteChallenge(challengeId, currentUser.UserId);
       
        if (isSuccessful)
        {
            return this.Ok();
        }

        return this.BadRequest();
    }

    [HttpPost]
    [Route("DeleteChallenge")]
    public async Task<IActionResult> DeleteChallenge([FromForm] string challengeId)
    {
        bool isSuccessful = await activityService.DeleteChallenge(challengeId, currentUser.UserId);

        if (isSuccessful)
        {
            return this.Ok();
        }

        return this.BadRequest();
    }
}
