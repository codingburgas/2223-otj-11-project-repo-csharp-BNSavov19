
using InMotion.Data.Models.Activities;

namespace InMotion.Services.Contracts;

public interface IActivityService
{
    Task AddActivity(string userId, string challenge, ActivityType type);
    
    Task AddUserAge(string userId, int age);
    
    Task<string> AddUserAllergies(string userId, Allergies allergies);
    
    Task<string> AddUserAssistance(string userId, Assistance assistance);
    
    Task<string> AddUserDiet(string userId, Diet diet);
    
    Task<string> AddUserDisability(string userId, Disability disability);
    
    Task AddUserGender(string userId, Gender gender);
    
    Task<string> AddUserHatesSports(string userId, HatesSports hatesSports);
    
    Task<string> AddUserLikesSports(string userId, LikesSports likesSports);
    
    Task AddUserLocation(string userId, string location);
    
    Task<string> AddUserMedical(string userId, Medical medical);
    
    Task<string> AddUserQuitting(string userId, Quiting quiting);
    
    Task AddUserWeightLoss(string userId);

    Task<bool> CompleteChallenge(string challengeId, string userId);
    Task<bool> DeleteChallenge(string challengeId, string userId);
    Task RemoveUserAge(string userId);
    
    Task RemoveUserAllergie(string userId, string allergieId);
    
    Task RemoveUserAssistance(string userId, string assistanceId);
    
    Task RemoveUserDiet(string userId, string dietId);
    
    Task RemoveUserDisability(string userId, string disabilityId);
    
    Task RemoveUserGender(string userId);
    
    Task RemoveUserHatesSports(string userId, string sportId);
    
    Task RemoveUserLikesSport(string userId, string sportId);
    
    Task RemoveUserLocation(string userId);
    
    Task RemoveUserMedical(string userId, string medicalId);
    
    Task RemoveUserQuitting(string userId, string quittingId);
    
    Task RemoveUserWeightLoss(string userId);
}
