using Allure.NUnit.Attributes;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.Login;

public class LoginPage : BasePage
{
    //Locators
    private ILocator UsernameInput() => this.page.Locator("input[data-path='username']");
    private ILocator PasswordInput() => this.page.Locator("input[data-path='password']");
    private ILocator LoginButton() => this.page.Locator("button[type='submit']");
    private ILocator LanguageToggleButton() => this.page.Locator("button[aria-label='Language toggle']");
    private ILocator LanguageInput() => this.page.Locator("div[class*='mantine-Input-wrapper'] input[aria-label='Select language']");

    //Constructor
    public LoginPage(IPage page) : base(page) { }

    //Actions
    [AllureStep("User sets the application language to [{language}] and logs in using with username [{username}]")]
    public async Task UserLogin(string username, [Skip] string password, string language)
    {
        await this.ClickLanguageToggleButton();
        await this.InputLanguage(language);

        await this.InputUsername(username);
        await this.InputPassword(password);
        await this.ClickLoginButton();
    }

    [AllureStep("User inputs username")]
    public async Task InputUsername(string username)
    {
        await this.UsernameInput().WaitForAsync();
        // await this.UsernameInput().FillAsync(username);
        await this.UsernameInput().ClickAsync();
        await this.page.Keyboard.TypeAsync(username);
    }

    [AllureStep("User inputs password")]
    public async Task InputPassword(string password)
    {
        await this.PasswordInput().WaitForAsync();
        // await this.PasswordInput().FillAsync(password);
        await this.PasswordInput().ClickAsync();
        await this.page.Keyboard.TypeAsync(password);
    }

    [AllureStep("User clicks login button")]
    public async Task ClickLoginButton() { await this.LoginButton().ClickAsync(); }

    [AllureStep("User clicks the language toggle button")]
    public async Task ClickLanguageToggleButton()
    {
        await this.LanguageToggleButton().WaitForAsync();
        await this.LanguageToggleButton().ClickAsync();
    }

    [AllureStep("User inputs the language {input}")]
    public async Task InputLanguage(string input)
    {
        await this.LanguageInput().WaitForAsync();
        await this.LanguageInput().FillAsync(input);
    }
}