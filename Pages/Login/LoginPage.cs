using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Util;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.Login;

public class LoginPage : BasePage
{
    //Locators
    private ILocator UsernameInput() => this.page.Locator("input[data-path='username']");
    private ILocator PasswordInput() => this.page.Locator("input[data-path='password']");
    private ILocator LoginButton() => this.page.Locator("button[type='submit']");
    private ILocator LanguageToggleButton() => this.page.Locator("button[aria-label='Language toggle']");
    private ILocator LanguageDropdown() => this.page.Locator("div[class*='mantine-InputWrapper-root mantine-Select-root']");

    //Constructor
    public LoginPage(IPage page) : base(page) { }

    //Actions
    [AllureStep("User Sets The Application Language To [{language}] and Logs In With Username [{username}]")]
    public async Task UserLogin(string username, [Skip] string password, int languageIndex)
    {
        await this.ClickLanguageToggleButton();
        await this.SelectLanguage(languageIndex);

        await this.InputUsername(username);
        await this.InputPassword(password);
        await this.ClickLoginButton();
    }

    [AllureStep("User Inputs Username")]
    public async Task InputUsername([Skip] string username)
    {
        await this.UsernameInput().WaitForAsync();        
        await this.UsernameInput().ClickAsync();
        await this.page.Keyboard.TypeAsync(username);
    }

    [AllureStep("User Inputs Password")]
    public async Task InputPassword([Skip] string password)
    {
        await this.PasswordInput().WaitForAsync();        
        await this.PasswordInput().ClickAsync();
        await this.page.Keyboard.TypeAsync(password);
    }

    [AllureStep("User Clicks Login Button")]
    public async Task ClickLoginButton() { await this.LoginButton().ClickAsync(); }

    [AllureStep("User Clicks The Language Toggle Button")]
    public async Task ClickLanguageToggleButton()
    {
        await this.LanguageToggleButton().WaitForAsync();
        await this.LanguageToggleButton().ClickAsync();
    }

    [AllureStep("User Selects the Language [{input}]")]
    public async Task SelectLanguage(int langIndex)
    {
        await this.LanguageDropdown().ClickAsync();
        await DropDownHelper.SelectListboxOptionByIndex(this.page, langIndex);
    }
}