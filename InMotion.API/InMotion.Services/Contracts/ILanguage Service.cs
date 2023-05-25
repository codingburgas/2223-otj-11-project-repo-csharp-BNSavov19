
using System.Text.Json;

namespace InMotion.Services.Contracts;

public interface ILanguageService
{
    Task<JsonElement> GetUserInterest(string text);

    Task SaveUserInterests(JsonElement userInterests, string userId);
}
