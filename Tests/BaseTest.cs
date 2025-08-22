using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Util;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Tests;

public abstract class BaseTest
{
    protected static IPage Page;
    protected static IAPIRequestContext Request;
    protected static string username;
    protected static string password;
    protected static string language;
    protected static string token;

    [SetUp]
    [AllureBefore("Setup test configuration and start the browser")]
    public async Task Setup()
    {
        //Added TraversePath() flag to enable DotNetEnv to also search ancestor/descendant directories for .env file
        DotNetEnv.Env.TraversePath().Load();

        //Default User Is "All Acess User" (handles warnings)
        username = Environment.GetEnvironmentVariable("ALL_ACCESS_USERNAME") ?? "";
        password = Environment.GetEnvironmentVariable("ALL_ACCESS_PASSWORD") ?? "";

        //Default, Application is set to English language
        language = Environment.GetEnvironmentVariable("LANG_ENGLISH") ?? "";

        //Assign Authentication Token
        token = Environment.GetEnvironmentVariable("TOKEN") ?? "";

        var playwright = await Playwright.CreateAsync();

        await SetupBrowser(playwright);
        await SetupAPIRequestContext(playwright);
    }

    [TearDown]
    [AllureAfter("Close the browser")]
    public async Task TearDown()
    {
        await Page.CloseAsync();
    }

    [AllureStep("User Role Is Set as [{role}] For Test Run")]
    public static void SetUserRole(string role)
    {
        //Modify string to match the .env file naming convention
        username = Environment.GetEnvironmentVariable(role.ToUpper().Replace(" ", "_") + "_USERNAME") ?? "";
        password = Environment.GetEnvironmentVariable(role.ToUpper().Replace(" ", "_") + "_PASSWORD") ?? "";
    }

    [AllureStep("Setup and Launch Browser")]
    public static async Task SetupBrowser(IPlaywright playwright)
    {
        var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
        Page = await browser.NewPageAsync();

        string? webAppUrl = TestContext.Parameters["webAppUrl"];
        if (webAppUrl != null)
        {

            await Page.GotoAsync(webAppUrl);
        }
        else
        {
            Console.Write("No Url provided in Test");
        }
    }

    [AllureStep("Setup APIRequestContext")]
    public static async Task SetupAPIRequestContext(IPlaywright playwright)
    {
        //Get Token Via Basic Authentication with username and password
        var token = await APIRequestHelper.APIGetToken(playwright, username, password);

        //Create new API Request Context object with the Token Authentication and Re-use it as now its authorized
        Request = await playwright.APIRequest.NewContextAsync(new()
        {
            BaseURL = $"{Environment.GetEnvironmentVariable("BASE_URL")}",
            ExtraHTTPHeaders = new Dictionary<string, string>
            {
                { "Authorization", $"Token {token}"}
            }
        });

        await APIRequestHelper.ChangeLanguagePatchRequest(Request, language);
    }
}
