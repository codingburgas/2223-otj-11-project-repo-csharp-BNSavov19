using InMotion.Data.Data;
using InMotion.Data.Models.Auth;
using InMotion.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using InMotion.Data.Models.Activities;

namespace InMotion.Services.Implentations;

internal class ActivityService : IActivityService
{
    private readonly ApplicationDbContext context;
    private readonly UserManager<User> userManager;

    public ActivityService(ApplicationDbContext context, UserManager<User> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    public async Task AddActivity(string userId, string challenge, ActivityType type)
    {
        var Activity = new Activity
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            Description = challenge,
            Type = type,
            DueDate = new DateOnly().AddDays(7),
            IsCompleted = false
        };

        await this.context.Activities.AddAsync(Activity);
        
        await this.context.SaveChangesAsync();
    }

    public async Task AddUserAge(string userId, int age)
    {
        var user = await userManager.FindByIdAsync(userId);

        user.Age = age;

        await userManager.UpdateAsync(user);
    }

    public async Task<string> AddUserAllergies(string userId, Allergies allergies)
    {
        var id = Guid.NewGuid().ToString();

        await this.context.UserAllergies!.AddAsync(new UserAllergies()
        {
            Id = id,
            Allergie = allergies,
            UserId = userId
        });

        await this.context.SaveChangesAsync();

        return id;
    }

    public async Task<string> AddUserAssistance(string userId, Assistance assistance)
    {
        var id = Guid.NewGuid().ToString();

        await this.context.UserAssistances!.AddAsync(new UserAssistance()
        {
            Id = id,
            Assistance = assistance,
            UserId = userId
        });

        await this.context.SaveChangesAsync();

        return id;
    }

    public async Task<string> AddUserDiet(string userId, Diet diet)
    {
        var id = Guid.NewGuid().ToString();

        await this.context.UserDiets!.AddAsync(new UserDiet()
        {
            Id = id,
            Diet = diet,
            UserId = userId
        });

        await this.context.SaveChangesAsync();

        return id;
    }

    public async Task<string> AddUserDisability(string userId, Disability disability)
    {
        var id = Guid.NewGuid().ToString();

        await this.context.UserDisabilities!.AddAsync(new UserDisability()
        {
            Id = id,
            Disability = disability,
            UserId = userId
        });

        await this.context.SaveChangesAsync();

        return id;
    }

    public async Task AddUserGender(string userId, Gender gender)
    {
        var user = await userManager.FindByIdAsync(userId);

        user.Gender = gender;

        await userManager.UpdateAsync(user);
    }

    public async Task<string> AddUserHatesSports(string userId, HatesSports hatesSports)
    {
        var id = Guid.NewGuid().ToString();

        await this.context.UserHatesSports!.AddAsync(new UserHatesSports()
        {
            Id = id,
            HatesSport = hatesSports,
            UserId = userId
        });

        await this.context.SaveChangesAsync();

        return id;
    }

    public async Task<string> AddUserLikesSports(string userId, LikesSports likesSports)
    {
        var id = Guid.NewGuid().ToString();

        await this.context.UserLikesSports!.AddAsync(new UserLikesSports()
        {
            Id = id,
            LikesSport = likesSports,
            UserId = userId
        });

        await this.context.SaveChangesAsync();

        return id;
    }

    public async Task AddUserLocation(string userId, string location)
    {
        var user = await userManager.FindByIdAsync(userId);

        user.Location = location;

        await userManager.UpdateAsync(user);
    }

    public async Task<string> AddUserMedical(string userId, Medical medical)
    {
        var id = Guid.NewGuid().ToString();

        await this.context.UserMedicals!.AddAsync(new UserMedical()
        {
            Id = id,
            Medical = medical,
            UserId = userId
        });

        await this.context.SaveChangesAsync();

        return id;
    }

    public async Task<string> AddUserQuitting(string userId, Quiting quiting)
    {
        var id = Guid.NewGuid().ToString();

        await this.context.UserQutings!.AddAsync(new UserQuting()
        {
            Id = id,
            Quiting = quiting,
            UserId = userId
        });

        await this.context.SaveChangesAsync();

        return id;
    }

    public async Task AddUserWeightLoss(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        user.IsTryingToLoseWeight = true;

        await userManager.UpdateAsync(user);
    }

    public async Task<bool> CompleteChallenge(string challengeId, string userId)
    {
        var challenges = this.context.Activities.Where(a => a.Id == challengeId && a.UserId == userId);

        if (challenges.Count() == 0)
        {
            return false;
        }

        foreach(var activity in challenges)
        {
            activity.IsCompleted = true;
        }

        await this.context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteChallenge(string challengeId, string userId)
    {
        var challenge = this.context.Activities.Where(a => a.Id == challengeId && a.UserId == userId);

        if (challenge.Count() == 0)
        {
            return false;
        }

        this.context.Activities.RemoveRange(challenge);

        await this.context.SaveChangesAsync();

        return true;
    }

    public async Task RemoveUserAge(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        user.Age = null;

        await userManager.UpdateAsync(user);
    }

    public async Task RemoveUserAllergie(string userId, string allergieId)
    {
        var allergie = this.context.UserAllergies!.Where(n => n.Id == allergieId && n.UserId == userId);

        this.context.UserAllergies!.RemoveRange(allergie);

        await this.context.SaveChangesAsync();
    }
    
    public async Task RemoveUserAssistance(string userId, string assistanceId)
    {
        var assistance = this.context.UserAssistances!.Where(n => n.Id == assistanceId && n.UserId == userId);

        this.context.UserAssistances!.RemoveRange(assistance);

        await this.context.SaveChangesAsync();
    }

    public async Task RemoveUserDiet(string userId, string dietId)
    {
        var diet = this.context.UserDiets!.Where(n => n.Id == dietId && n.UserId == userId);

        this.context.UserDiets!.RemoveRange(diet);

        await this.context.SaveChangesAsync();
    }

    public async Task RemoveUserDisability(string userId, string disabilityId)
    {
        var dissability = this.context.UserDisabilities!.Where(n => n.Id == disabilityId && n.UserId == userId);

        this.context.UserDisabilities!.RemoveRange(dissability);

        await this.context.SaveChangesAsync();
    }

    public async Task RemoveUserGender(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        user.Age = null;

        await userManager.UpdateAsync(user);
    }

    public async Task RemoveUserHatesSports(string userId, string sportId)
    {
        var sports = this.context.UserHatesSports!.Where(n => n.Id == sportId && n.UserId == userId);

        this.context.UserHatesSports!.RemoveRange(sports);

        await this.context.SaveChangesAsync();
    }

    public async Task RemoveUserLikesSport(string userId, string sportId)
    {
        var sports = this.context.UserLikesSports!.Where(n => n.Id == sportId && n.UserId == userId);

        this.context.UserLikesSports!.RemoveRange(sports);

        await this.context.SaveChangesAsync();
    }

    public async Task RemoveUserLocation(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        user.Location = null;

        await userManager.UpdateAsync(user);
    }

    public async Task RemoveUserMedical(string userId, string medicalId)
    {
        var medical = this.context.UserMedicals!.Where(n => n.Id == medicalId && n.UserId == userId);

        this.context.UserMedicals!.RemoveRange(medical);

        await this.context.SaveChangesAsync();
    }
   
    public async Task RemoveUserQuitting(string userId, string quittingId)
    {
        var quits = this.context.UserQutings!.Where(n => n.Id == quittingId && n.UserId == userId);

        this.context.UserQutings!.RemoveRange(quits);

        await this.context.SaveChangesAsync();
    }

    public async Task RemoveUserWeightLoss(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        user.IsTryingToLoseWeight = null;

        await userManager.UpdateAsync(user);
    }
}
