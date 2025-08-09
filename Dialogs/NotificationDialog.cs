using InvenTreeAutomationFramework.Enums;
using InvenTreeAutomationFramework.Pages;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Dialogs;

public class NotificationDialog : BasePage
{
    private const string NOTIFICATION_TITLE = "div[class*='Notification-title']";
    private const string NOTIFICATION_DESCRIPTION = "div[class*='Notification-description']";
    // private ILocator NotificationBanner() => this.page.Locator("div[class*='Notification'] div[role='alert'][data-with-icon='true']");
    private ILocator NotificationBanner() => this.page.Locator("div[class*='Notification'][data-position='bottom-right'] div[id='login'], div[class*='Notification'][data-position='bottom-right'] div[id='form-success']");

    public NotificationDialog(IPage page) : base(page) { }

    public async Task<bool> VerifyNotifMsg(NotificationEnums notifEnum)
    {
        await this.NotificationBanner().WaitForAsync();
        string? notifTitle = await NotificationBanner().Locator(NOTIFICATION_TITLE).TextContentAsync();
        string? notifDesc = await NotificationBanner().Locator(NOTIFICATION_DESCRIPTION).TextContentAsync();

        string? msg = notifTitle + " " + notifDesc;

        if (msg != null)
        {
            return msg.Equals(Notifications.NotificationDictionary[notifEnum]);
        }
        else
        {
            return false;
        }
    }
}