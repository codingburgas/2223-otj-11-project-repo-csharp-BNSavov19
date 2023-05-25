using Azure.Core;
using Azure.AI.Language.Conversations;
using InMotion.Services.Contracts;
using Azure;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using InMotion.Data.Models.Activities;

namespace InMotion.Services.Implentations;

internal class LanguageService : ILanguageService
{
    private readonly ConversationAnalysisClient client;
    private readonly IActivityService activityService;

    public LanguageService(IConfiguration configuration, IActivityService activityService)
    {
        var endpoint = new Uri(configuration["Azure:CognitiveLanguageServices:Endpoint"]);
        var credential = new AzureKeyCredential(configuration["Azure:CognitiveLanguageServices:APIKey"]);

        this.client = new ConversationAnalysisClient(endpoint, credential);
        this.activityService = activityService;
    }
    
    public async Task<JsonElement> GetUserInterest(string text)
    {
        string projectName = "Understand-user-activities";
        string deploymentName = "Test";

        var data = new
        {
            analysisInput = new
            {
                conversationItem = new
                {
                    text = text,
                    id = "1",
                    participantId = "1",
                }
            },
            parameters = new
            {
                projectName,
                deploymentName,

                // Use Utf16CodeUnit for strings in .NET.
                stringIndexType = "Utf16CodeUnit",
            },
            kind = "Conversation",
        };

        var response = this.client.AnalyzeConversation(RequestContent.Create(data));

        var result = JsonDocument.Parse(response.ContentStream);
        var conversationalTaskResult = result.RootElement;

        var returnVal = conversationalTaskResult
            .GetProperty("result")
            .GetProperty("prediction")
            .GetProperty("entities");

        return returnVal;
    }

    public async Task SaveUserInterests(JsonElement userInterests, string userId)
    {
        foreach (var interest in userInterests.EnumerateArray())
        {
            var category = interest.GetProperty("category");

            switch (category.GetString())
            {
                case "Age":
                    var age = interest.GetProperty("resolutions").EnumerateArray();
                    var realage = age.First().GetProperty("value").GetInt32();
                    await this.activityService.AddUserAge(userId, realage);
                    break;
                case "Allergies_Gluten":
                    await this.activityService.AddUserAllergies(userId, Allergies.Gluten);
                    break;
                case "Allergies_Lactose":
                    await this.activityService.AddUserAllergies(userId, Allergies.Lactose);
                    break;
                case "Allergies_Peanutes":
                    await this.activityService.AddUserAllergies(userId, Allergies.Peanutes);
                    break;
                case "Assistance_ExtraTime":
                    await this.activityService.AddUserAssistance(userId, Assistance.ExtraTime);
                    break;
                case "Assistance_Guide":
                    await this.activityService.AddUserAssistance(userId, Assistance.Guide);
                    break;
                case "Assistance_MentalHealthSupport":
                    await this.activityService.AddUserAssistance(userId, Assistance.MentalHealthSupport);
                    break;
                case "Assistance_ProstheticLimb":
                    await this.activityService.AddUserAssistance(userId, Assistance.ProstheticLimb);
                    break;
                case "Assistance_SignLanguageInterpreter":
                    await this.activityService.AddUserAssistance(userId, Assistance.SignLanguageInterpreter);
                    break;
                case "Assistance_SpecialAccommodations":
                    await this.activityService.AddUserAssistance(userId, Assistance.SpecialAccommodations);
                    break;
                case "Assistance_Wheelchair":
                    await this.activityService.AddUserAssistance(userId, Assistance.Wheelchair);
                    break;
                case "Diet_Vegetarian":
                    await this.activityService.AddUserDiet(userId, Diet.Vegetarian);
                    break;
                case "Disability_ChronicIllness":
                    await this.activityService.AddUserDisability(userId, Disability.ChronicIllness);
                    break;
                case "Disability_CognitiveImpairment":
                    await this.activityService.AddUserDisability(userId, Disability.CognitiveImpairment);
                    break;
                case "Disability_HearingImpairment":
                    await this.activityService.AddUserDisability(userId, Disability.HearingImpairment);
                    break;
                case "Disability_LearningDisability":
                    await this.activityService.AddUserDisability(userId, Disability.LearningDisability);
                    break;
                case "Disability_MentalImpairment":
                    await this.activityService.AddUserDisability(userId, Disability.MentalImpairment);
                    break;
                case "Disability_MobilityImpairment":
                    await this.activityService.AddUserDisability(userId, Disability.MobilityImpairment);
                    break;
                case "Disability_PhysicalImpairment":
                    await this.activityService.AddUserDisability(userId, Disability.PhysicalImpairment);
                    break;
                case "Disability_VisualImpairment":
                    await this.activityService.AddUserDisability(userId, Disability.VisualImpairment);
                    break;
                case "Gender_Female":
                    await this.activityService.AddUserGender(userId, Gender.Female);
                    break;
                case "Gender_Male":
                    await this.activityService.AddUserGender(userId, Gender.Male);
                    break;
                case "Gender_Non_Binary":
                    await this.activityService.AddUserGender(userId, Gender.Non_Binary);
                    break;
                case "Hates_Sports_Basketball":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Basketball);
                    break;
                case "Hates_Sports_Biking":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Biking);
                    break;
                case "Hates_Sports_Cycling":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Cycling);
                    break;
                case "Hates_Sports_Football":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Football);
                    break;
                case "Hates_Sports_Golf":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Golf);
                    break;
                case "Hates_Sports_Hiking":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Hiking);
                    break;
                case "Hates_Sports_Rock_Climbing":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Rock_Climbing);
                    break;
                case "Hates_Sports_Running":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Running);
                    break;
                case "Hates_Sports_Skating":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Skating);
                    break;
                case "Hates_Sports_Skiing":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Skiing);
                    break;
                case "Hates_Sports_Surfing":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Surfing);
                    break;
                case "Hates_Sports_Swimming":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Swimming);
                    break;
                case "Hates_Sports_Tennis":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Tennis);
                    break;
                case "Hates_Sports_Volleyball":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Volleyball);
                    break;
                case "Hates_Sports_Walking":
                    await this.activityService.AddUserHatesSports(userId, HatesSports.Walking);
                    break;
                case "Likes_Sports_Basketball":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Basketball);
                    break;
                case "Likes_Sports_Biking":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Biking);
                    break;
                case "Likes_Sports_Cycling":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Cycling);
                    break;
                case "Likes_Sports_Football":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Football);
                    break;
                case "Likes_Sports_Golf":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Golf);
                    break;
                case "Likes_Sports_Hiking":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Hiking);
                    break;
                case "Likes_Sports_Rock_Climbing":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Rock_Climbing);
                    break;
                case "Likes_Sports_Running":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Running);
                    break;
                case "Likes_Sports_Skating":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Skating);
                    break;
                case "Likes_Sports_Skiing":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Skiing);
                    break;
                case "Likes_Sports_Surfing":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Surfing);
                    break;
                case "Likes_Sports_Swimming":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Swimming);
                    break;
                case "Likes_Sports_Tennis":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Tennis);
                    break;
                case "Likes_Sports_Volleyball":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Volleyball);
                    break;
                case "Likes_Sports_Walking":
                    await this.activityService.AddUserLikesSports(userId, LikesSports.Walking);
                    break;
                case "Location":
                    var location = interest.GetProperty("resolution").GetProperty("value").GetString();
                        
                    await this.activityService.AddUserLocation(userId, location);
                    break;
                case "Medical_Asthma":
                    await this.activityService.AddUserMedical(userId, Medical.Asthma);
                    break;
                case "Medical_Back":
                    await this.activityService.AddUserMedical(userId, Medical.Back);
                    break;
                case "Medical_BloodPressure":
                    await this.activityService.AddUserMedical(userId, Medical.BloodPressure);
                    break;
                case "Medical_Diabetic":
                    await this.activityService.AddUserMedical(userId, Medical.Diabetic);
                    break;
                case "Medical_Knee":
                    await this.activityService.AddUserMedical(userId, Medical.Knee);
                    break;
                case "Quitting_Smoking":
                    await this.activityService.AddUserQuitting(userId, Quiting.Smoking);
                    break;
                case "Weight_Loss":
                    await this.activityService.AddUserWeightLoss(userId);
                    break;
                default:
                    // when no case is matched
                    break;
            }
        }
    }
}
