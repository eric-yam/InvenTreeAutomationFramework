using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Enums;
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

    [SetUp]
    [AllureBefore("Setup test configuration and start the browser")]
    public async Task Setup()
    {
        //Added TraversePath() flag to enable DotNetEnv to also search ancestor/descendant directories for .env file
        DotNetEnv.Env.TraversePath().Load();

        //Default User Is "All Acess User" (handles warnings)
        SetUserRole(UserRoles.RolesDict[UserEnums.AllAccess]);

        //Default, Application is set to English language        
        language = Environment.GetEnvironmentVariable("LANG_ENGLISH") ?? "";

        var playwright = await Playwright.CreateAsync();

        string token = await APIRequestHelper.APIGetToken(playwright, username, password);

        //Share the authentication token between browser and api session to share the same changes 
        await SetupBrowser(playwright, token);
        await SetupAPIRequestContext(playwright, token);
        await APIRequestHelper.ChangeLanguagePatchRequest(Request, language);
    }

    [TearDown]
    [AllureAfter("Close the browser and API Request")]
    public async Task TearDown()
    {
        await Page.CloseAsync();
        await Request.DisposeAsync();
    }

    /// <summary>
    /// Sets the username and password based on the specified user role by reading from environment variables.
    /// </summary>
    /// <param name="role"></param>
    [AllureStep("User Role Is Set as [{role}] For Test Run")]
    public static void SetUserRole(string role)
    {
        //Modify string to match the .env file naming convention
        username = Environment.GetEnvironmentVariable(role.ToUpper().Replace(" ", "_") + "_USERNAME") ?? "";
        password = Environment.GetEnvironmentVariable(role.ToUpper().Replace(" ", "_") + "_PASSWORD") ?? "";
    }

    /// <summary>
    /// Sets up and launches the browser with shared authentication state using the provided token.
    /// Note: The browser context is configured to include the same authentication headers as the API request context, enabling shared session state.
    /// </summary>
    /// <param name="playwright"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [AllureStep("Setup and Launch Browser")]
    public static async Task SetupBrowser(IPlaywright playwright, string token)
    {
        /*
            By using the same Authentication Token and headers, we make both the 
            Browser Context and API Request Context the same, therefore changes
            made through API requests will be visible in the Browser Context 
            as they will both be on the same session.
            This is known as Shared Authentication State.
        */

        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions() { Headless = false });

        //Specify Context to have the same headers and token as IAPIRequestContext
        var context = await browser.NewContextAsync(
            new BrowserNewContextOptions()
            {
                ExtraHTTPHeaders = new Dictionary<string, string>
                {
                    { "Authorization", $"Token {token}"},
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" }
                }
            }
        );

        Page = await context.NewPageAsync();

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

    /// <summary>
    /// Sets up the APIRequestContext with the provided token for authenticated API requests. 
    /// This allows the test suite to make authorized API calls using the same session as the browser
    /// </summary>
    /// <param name="playwright"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [AllureStep("Setup APIRequestContext")]
    public static async Task SetupAPIRequestContext(IPlaywright playwright, string token)
    {
        //Create new API Request Context object with the Token Authentication and Re-use it as now its authorized
        Request = await playwright.APIRequest.NewContextAsync(
            new APIRequestNewContextOptions()
            {
                BaseURL = $"{Environment.GetEnvironmentVariable("BASE_URL")}",
                ExtraHTTPHeaders = new Dictionary<string, string>
                        {
                            { "Authorization", $"Token {token}"},
                            { "Content-Type", "application/json" },
                            { "Accept", "application/json" }
                        }
            }
        );
    }
}
