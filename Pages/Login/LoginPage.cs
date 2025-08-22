using Allure.NUnit.Attributes;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.Login;

public class LoginPage : BasePage
{
    //Locators
    private ILocator UsernameInput() => this.page.Locator("input[data-path='username']");
    private ILocator PasswordInput() => this.page.Locator("input[data-path='password']");
    private ILocator LoginButton() => this.page.Locator("button[type='submit']");
    private ILocator PageBackgroundImg() => this.page.Locator("div[class*='mantine-BackgroundImage-root']");

    //Constructor
    public LoginPage(IPage page) : base(page) { }

    //Actions
    [AllureStep("User Logs In With Username [{username}]")]
    public async Task UserLogin(string username, [Skip] string password)
    {
        await this.PageBackgroundImg().WaitForAsync();
        await this.InputUsername(username);
        await this.InputPassword(password);
        await this.ClickLoginButton();
    }

    [AllureStep("User Inputs Username")]
    public async Task InputUsername([Skip] string username)
    {
        await this.UsernameInput().ClickAsync();
        await this.UsernameInput().FillAsync(username);
    }

    [AllureStep("User Inputs Password")]
    public async Task InputPassword([Skip] string password)
    {
        await this.PasswordInput().ClickAsync();
        await this.PasswordInput().FillAsync(password);
    }

    [AllureStep("User Clicks Login Button")]
    public async Task ClickLoginButton() { await this.LoginButton().ClickAsync(); }
}