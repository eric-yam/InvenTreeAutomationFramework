using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Util;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Tests;

public abstract class BaseTest
{
    protected static IPage Page;
    protected static string username;
    protected static string password;
    protected static string language;

    [SetUp]
    public async Task Setup()
    {
        //Default User Is "All Acess User" (handles warnings)
        username = Environment.GetEnvironmentVariable("ALL_ACCESS_USERNAME") ?? "";
        password = Environment.GetEnvironmentVariable("ALL_ACCESS_PASSWORD") ?? "";

        //Default, Application is set to English language
        language = Environment.GetEnvironmentVariable("LANG_ENGLISH") ?? "";

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
    public async Task TearDown()
    {
        await Page.CloseAsync();
    }

    [AllureStep("User role is set as [{role}] for test run")]
    public static void SetUserRole(string role)
    {
        //Modify string to match the .env file naming convention
        username = Environment.GetEnvironmentVariable(role.ToUpper().Replace(" ", "_") + "_USERNAME") ?? "";
        password = Environment.GetEnvironmentVariable(role.ToUpper().Replace(" ", "_") + "_PASSWORD") ?? "";
    }

    [AllureStep("Set Application Language [{lang}] for test run")]
    public static void SetLanguage(string lang)
    {
        language = Environment.GetEnvironmentVariable("LANG_" + lang.ToUpper()) ?? "";
    }
}
