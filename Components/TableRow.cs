using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Components;

public abstract class TableRow : BaseComponent
{
    private ILocator TableRowHeaders() => this.page.Locator("table thead th:not([class*='selector-cell'])");
    protected readonly Dictionary<string, string> Row;
    public TableRow(IPage page) : base(page)
    {
        this.Row = new Dictionary<string, string>();
    }

    public async Task InitializeRow(ILocator rowLocator)
    {
        var headersList = await this.TableRowHeaders().AllAsync();
        for (int i = 0; i < headersList.Count; i++)
        {
            string? colName = await headersList[i].TextContentAsync();
            string? colVal = await rowLocator.Locator($"td:nth-child({i + 2})").TextContentAsync();
            if (colName != null && colVal != null)
            {
                this.Row.Add(colName, colVal);
            }
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

    //Abstract
    public abstract string GetKey();
}