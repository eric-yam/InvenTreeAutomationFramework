using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Util;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Tests;

public abstract class BaseTest
{
    protected static IPage Page;
    protected static string username;
    protected static string password;
    protected static int languageIndex;

    [SetUp]
    [AllureBefore("Setup test configuration and start the browser")]
    public async Task Setup()
    {
        //Default User Is "All Acess User" (handles warnings)
        username = Environment.GetEnvironmentVariable("ALL_ACCESS_USERNAME") ?? "";
        password = Environment.GetEnvironmentVariable("ALL_ACCESS_PASSWORD") ?? "";

        //Default, Application is set to English language
        languageIndex = Convert.ToInt32(Environment.GetEnvironmentVariable("LANG_ENGLISH"));

        var playwright = await Playwright.CreateAsync();
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

        //Added TraversePath() flag to enable DotNetEnv to also search ancestor/descendant directories for .env file
        DotNetEnv.Env.TraversePath().Load();
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

    [AllureStep("Set Application Language [{lang}] For Test Run")]
    public static void SetLanguage(string lang)
    {
        //TODO: This method doesn't work 
        // languageIndex = Environment.GetEnvironmentVariable("LANG_" + lang.ToUpper()) ?? "";
        languageIndex = Convert.ToInt32(Environment.GetEnvironmentVariable("LANG_" + lang.ToUpper()));
    }
}
