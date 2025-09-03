using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Enums;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Dialogs;

public class NotificationDialog : BaseDialog
{
    private const string NOTIFICATION_TITLE = "div[class*='Notification-title']";
    private const string NOTIFICATION_DESCRIPTION = "div[class*='Notification-description']";

    private ILocator NotificationBanner() => this.page.Locator("div[class*='Notification'][data-position='bottom-right'] div[id='login'], div[class*='Notification'][data-position='bottom-right'] div[id='form-success']");

    public NotificationDialog(IPage page) : base(page) { }

    [AllureStep("Verify Notification Message [{notifEnum}] is Displayed")]
    public async Task<bool> VerifyNotifMsg(NotificationEnums notifEnum)
    {
        await this.NotificationBanner().WaitForAsync();
        string? notifTitle = await NotificationBanner().Locator(NOTIFICATION_TITLE).TextContentAsync();
        string? notifDesc = await NotificationBanner().Locator(NOTIFICATION_DESCRIPTION).TextContentAsync();

        string? msg = notifTitle + " " + notifDesc;

        AllureLifecycle.Instance.UpdateStep(stepResult =>
        {
            stepResult.name = $"Notification message: [{Notifications.NotificationDictionary[notifEnum]}] is displayed";
        });

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