
using InMotion.Data.Models.Activities;
using InMotion.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace InMotion.Services.Implentations;

internal class MachineLearningService : IMachineLearningService
{
    private readonly IUserService userService;
    private readonly IConfiguration configuration;

    public MachineLearningService(IUserService userService, IConfiguration configuration)
    {
        this.userService = userService;
        this.configuration = configuration;
    }

    public class InputData
    {
        public List<string> columns { get; set; }
        public List<string> index { get; set; }
        public List<List<object>> data { get; set; }
    }

    public class Root
    {
        public InputData input_data { get; set; }
    }


    public async Task<string> GenerateChallengeForUser(string userId, ActivityType type)
    {
        var user = await userService.GetFullUserByIdAsync(userId);

        var rng = new Random();
        
        var handler = new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
        };

        var gender = string.Empty;

        switch (user.Gender)
        {
            case Gender.Female:
                gender = "Female";
                break;
            case Gender.Male:
                gender = "Male";
                break;
            case Gender.Non_Binary:
                gender = "Non-binary";
                break;
            default:
                gender = "None";
                break;
        }

        var disabilties = "None";

        if (user.Disabilities != null && user.Disabilities!.Count != 0)
        {
            var shuffledDisabilities = user.Disabilities.OrderBy(a => Guid.NewGuid()).ToList();

            disabilties = string.Join(", ", shuffledDisabilities.Select(a => a.Disability.ToString()));
        }

        var assistance = "None";

        if (user.Assistances != null && user.Assistances!.Count != 0)
        {
            var shuffledAssistnace = user.Assistances.OrderBy(a => Guid.NewGuid()).ToList();

            assistance = string.Join(", ", shuffledAssistnace.Select(a => a.Assistance.ToString()));
        }

        var diet = "None";

        if (user.Diets != null && user.Diets!.Count != 0)
        {
            var shuffledDiets = user.Diets.OrderBy(a => Guid.NewGuid()).ToList();

            diet = string.Join(", ", shuffledDiets.Select(d => d.Diet.ToString()));
        }

        var quitting = "None";

        if (user.Qutings != null && user.Qutings!.Count != 0)
        {
            var shuffleQuiting = user.Qutings.OrderBy(a => Guid.NewGuid()).ToList();

            quitting = string.Join(", ", shuffleQuiting.Select(q => q.Quiting.ToString()));
        }

        var likes = "None";
        
        if (user.LikesSports != null && user.LikesSports!.Count != 0)
        {
            var shuffleLikes = user.LikesSports.OrderBy(a => Guid.NewGuid()).ToList();

            likes = string.Join(", ", shuffleLikes.Select(l => l.LikesSport.ToString()));
        }

        var dislikes = "None";

        if (user.HatesSports != null && user.HatesSports!.Count != 0)
        {
            var shuffleDislikes = user.HatesSports.OrderBy(a => Guid.NewGuid()).ToList();

            dislikes = string.Join(", ", shuffleDislikes.Select(d => d.HatesSport.ToString()));
        }

        using (var client = new HttpClient(handler))
        {
            var req = new Root()
            {
                input_data = new InputData()
                {
                    columns = new () {"Age", "Gender", "Disability", "Assistance", "Diet", "Quitting", "Likes", "Dislikes" },
                    index = new () { user.Email },
                    data = new ()
                    {
                        new () 
                        {
                            (user.Age ??= rng.Next(1, 65)),
                            gender,
                            disabilties,
                            assistance,
                            diet,
                            quitting,
                            likes,
                            dislikes
                        }
                    }
                }
            };
            
            var requestBody = JsonConvert.SerializeObject(req);
            
            var apiKey = this.configuration["Azure:MachineLearning:APIKey"];
            
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("A key should be provided to invoke the endpoint");
            }
            
            client.DefaultRequestHeaders.Authorization = new ("Bearer", apiKey);
            client.BaseAddress = new Uri(configuration["Azure:MachineLearning:Endpoint"]);

            var content = new StringContent(requestBody);
            content.Headers.ContentType = new ("application/json");
            
            content.Headers.Add("azureml-model-deployment", "default");

            var response = await client.PostAsync("", content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var rootElement = result.RootElement;
                
                return rootElement.EnumerateArray().First().GetString();
            }
            
            return string.Empty;
        }
    }
}
