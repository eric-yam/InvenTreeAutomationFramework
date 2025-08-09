using System.Threading.Tasks;
using InvenTreeAutomationFramework.Util;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Components;

public class Table : BaseComponent
{
    private const string LOADING_ICON = "div[class^='mantine-datatable-loader'] span[class*='mantine-Loader-root']";
    private ILocator TableLocator() => this.page.Locator("table tbody"); //Used for wait async
    private ILocator TableRowLocators() => this.page.Locator("table tbody tr");
    private readonly Dictionary<string, TableRow> table;
    public Table(IPage page) : base(page)
    {
        this.table = new Dictionary<string, TableRow>();
    }

    public async Task InitializeTable(Func<IPage, ILocator, Task<TableRow>> InitTableRowMethod)
    {
        //Waits for backend, syncs automation with backend activity loading the table rows into table
        // APIHelper apiHelper = new APIHelper(this.page);
        await APIHelper.WaitForTableResponse(this.page);

        await this.page.WaitForSelectorAsync(LOADING_ICON, new PageWaitForSelectorOptions { State = WaitForSelectorState.Detached, Timeout = 3000 });
        await this.TableLocator().WaitForAsync();
        var tableRowList = await this.TableRowLocators().AllAsync();

        if (this.table.Count > 0)
        {
            this.table.Clear();
        }

        foreach (ILocator currentRowLocator in tableRowList)
        {
            TableRow row = await InitTableRowMethod(page, currentRowLocator);
            this.table.Add(row.GetKey(), row.GetRow());
        }
    }

    public Dictionary<string, TableRow> GetTable()
    {
        return this.table;
    }

    public TableRow GetRow(string key)
    {
        return this.table[key];
    }

    public async Task<List<string>> GetHeaderNames()
    {
        if (this.table.Count > 0)
        {
            TableRow firstRow = this.table.First().Value;
            return await firstRow.GetHeaderNames();
        }
        else
        {
            throw new Exception("Header List Was Empty Or Table Was Empty");
        }
    }

    public bool Equals(Table other)
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
            bool isEqual = true;

            foreach (string key in this.table.Keys)
            {
                if (!isEqual)
                {
                    return false;
                }
                isEqual = isEqual && this.table[key].Equals(other.table[key]);
            }

            return isEqual;
        }
    }
}