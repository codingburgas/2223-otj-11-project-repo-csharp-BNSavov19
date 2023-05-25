
using InMotion.Data.Models.Activities;

namespace InMotion.Services.Contracts;

public interface IMachineLearningService
{
    Task<string> GenerateChallengeForUser(string userId, ActivityType type);
}
