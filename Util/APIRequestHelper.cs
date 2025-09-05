using System.Text.Json;
using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Enums;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Util;

// Helper class used to execute API Requests
public static class APIRequestHelper
{
    /// <summary>
    /// Get authentication token for API requests using provided username and password.
    /// </summary>
    /// <param name="playwright"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [AllureStep("Get Authentication Token For API")]
    public static async Task<string> APIGetToken(IPlaywright playwright, string username, string password)
    {
        IAPIRequestContext request = await playwright.APIRequest.NewContextAsync(
            new APIRequestNewContextOptions()
            {
                BaseURL = $"{Environment.GetEnvironmentVariable("BASE_URL")}"
            });

        IAPIResponse debugResponse = await request.GetAsync(APIEndpoints.APIEndpointDictionary[APIHelperEnums.Token],
            new APIRequestContextOptions()
            {
                //Manually Add the Autorization Header For Token Request in Base64 Encoding
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

    /// <summary>
    /// Change the language of the application via a PATCH request to the profile language endpoint.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="lang"></param>
    /// <returns></returns>
    [AllureStep("Send API PATCH Request To Set Language Of Application To [{lang}]")]
    public static async Task ChangeLanguagePatchRequest(IAPIRequestContext request, string lang)
    {
        IAPIResponse debugResponse = await request.PatchAsync(APIEndpoints.APIEndpointDictionary[APIHelperEnums.ProfileLanguage],
        new APIRequestContextOptions()
        {
            DataObject = new { language = lang }
        });
    }
}