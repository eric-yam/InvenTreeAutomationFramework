using System.Text.Json;
using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Enums;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Util;

public static class APIRequestHelper
{
    [AllureStep("Get Authentication Token For API")]
    public static async Task<string> APIGetToken(IPlaywright playwright, string username, string password)
    {
        IAPIRequestContext request = await playwright.APIRequest.NewContextAsync(new()
        {
            BaseURL = $"{Environment.GetEnvironmentVariable("BASE_URL")}"
        });

        IAPIResponse debugResponse = await request.GetAsync(APIEndpoints.APIEndpointDictionary[APIHelperEnums.Token],
        new APIRequestContextOptions()
        {
            // DataObject = new { username = "admin", password = "inventree" },
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Accept", "application/json" },
                { "Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(username + ":" + password)) }
            }
        });

        JsonElement? tokenJson = await debugResponse.JsonAsync();
        string token = tokenJson?.GetProperty(APIProperty.APIPropertyDictionary[APIPropertyEnums.Token]).ToString() ?? "";
        return token;
    }

    [AllureStep("Send API PATCH Request To Set Language Of Application To [{lang}]")]
    public static async Task ChangeLanguagePatchRequest(IAPIRequestContext request, string lang)
    {
        IAPIResponse debugResponse = await request.PatchAsync(APIEndpoints.APIEndpointDictionary[APIHelperEnums.ProfileLanguage],
        new()
        {
            DataObject = new { language = lang }
        });
    }
}