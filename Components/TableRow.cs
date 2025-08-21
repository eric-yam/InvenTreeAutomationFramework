using Allure.NUnit.Attributes;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Components;

public abstract class TableRow : BaseComponent
{
    private ILocator TableRowHeaders() => this.page.Locator("table thead th:not([class*='selector-cell'])");
    protected readonly Dictionary<string, string> Row;
    protected ILocator? TableRowLocator;

    public TableRow(IPage page) : base(page)
    {
        this.Row = new Dictionary<string, string>();
    }

    public async Task InitializeRow(ILocator rowLocator)
    {
        this.TableRowLocator = rowLocator;

        var headersList = await this.TableRowHeaders().AllAsync();
        for (int i = 0; i < headersList.Count; i++)
        {
            string? colName = await headersList[i].TextContentAsync();
            string? colVal = await rowLocator.Locator($"td:nth-child({i + 2})").TextContentAsync();

            if (colName != null && colVal != null)
            {
                this.Row.Add(colName, colVal);
            }

            // string? colVal;
            /*
                This if statement deciding on the locator was necessary if they decided to 
                re-add the "invalid" "valid" tag in the Issued By column.
                Currently InvenTree website has been reverted and does not include the additional
                tag in the Issued by column. Kept just in case.    
            */
            // if (await rowLocator.Locator($"td:nth-child({i + 2}) div:nth-child(3)").CountAsync() > 0)
            // {
            //     colVal = await rowLocator.Locator($"td:nth-child({i + 2}) div:nth-child(3)").TextContentAsync();
            // }
            // else
            // {
            //     colVal = await rowLocator.Locator($"td:nth-child({i + 2})").TextContentAsync();
            // }


        }
    }

    public TableRow GetRow()
    {
        return this;
    }

    public Dictionary<string, string> GetRowAsDictionary()
    {
        return Row;
    }

    public async Task<List<string>> GetHeaderNames()
    {
        List<string> resultList = new List<string>();
        var headersList = await this.TableRowHeaders().AllAsync();

        foreach (var header in headersList)
        {
            string? s = await header.TextContentAsync();
            if (s != null)
            {
                resultList.Add(s);
            }
        }
        return resultList;
    }

    [AllureStep("Verify Context TableRow Is Content Equivalent To The Expected TableRow")]
    public bool Equals(TableRow other)
    {

        if (this == other)
        {
            //Same address
            return true;
        }
        else if (other == null || this.GetType() != other.GetType())
        {
            //other is null or other is not the same class type
            return false;
        }
        else
        {
            //Verify dictionary content equal
            //Expect() returns set of differences between this and other
            //Any() returns boolean, if sequence contains elements

            if (this.Row.Count == other.Row.Count)
            {
                return !this.Row.Except(other.Row).Any();
            }
            else
            {
                return false;
            }
        }
    }

    public bool Contains(Dictionary<string, string> other)
    {
        bool doesContain = true;
        foreach (string key in this.Row.Keys)
        {
            doesContain = other[key].Contains(this.Row[key]);
            if (!doesContain)
            {
                break;
            }
        }

        return doesContain;
    }

    //Actions
    [AllureStep("User Clicks Table Row")]
    public async Task ClickRow()
    {
        if (this.TableRowLocator != null)
        {
            await this.TableRowLocator.ClickAsync();
        }
        else
        {
            throw new Exception("Table row locator was not assigned and still null.");
        }
    }

    //Abstract
    public abstract string GetKey();
}