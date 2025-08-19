using Allure.NUnit.Attributes;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.Login;

public class LoginPage : BasePage
{
    //TODO: Add language selector
    //Locators
    private ILocator UsernameInput() => this.page.Locator("input[data-path='username']");
    private ILocator PasswordInput() => this.page.Locator("input[data-path='password']");
    private ILocator LoginButton() => this.page.Locator("button[type='submit']");

    //Constructor
    public LoginPage(IPage page) : base(page) { }

    //Actions
    public async Task UserLogin(string username, string password)
    {
        await this.InputUsername(username);
        await this.InputPassword(password);
        await this.ClickLoginButton();
    }

    [AllureStep("User inputs username")]
    public async Task InputUsername(string username) { await this.UsernameInput().FillAsync(username); }

    [AllureStep("User inputs password")]
    public async Task InputPassword(string password) { await this.PasswordInput().FillAsync(password); }

    [AllureStep("User clicks login button")]
    public async Task ClickLoginButton() { await this.LoginButton().ClickAsync(); }
}