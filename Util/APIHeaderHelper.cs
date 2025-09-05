using System.Text.Json;

namespace InvenTreeAutomationFramework.Util;

public static class APIHeaderHelper
{
    //TODO: Revisit after adding new table
    /// <summary>
    /// Dictionary to translate table column names to API JSON keys
    /// Used to map UI table columns to their corresponding API response fields for validation
    /// Keys are the column names as displayed in the UI
    /// Values are functions that extract and format the corresponding value from a JsonElement    
    /// </summary>
    public static Dictionary<string, Func<JsonElement, string>> TranslateToAPIKey = new Dictionary<string, Func<JsonElement, string>>()
    {
        // {"IPN",                 el=>el.GetProperty("part_detail").GetProperty("IPN").ToString() ?? ""},
        // {"Completed items",     el=>el.GetProperty("completed").ToString() + " / " + el.GetProperty("quantity").ToString().Substring(0, el.GetProperty("quantity").ToString().IndexOf('.')) ?? ""}, //factor out
        // {"Target Date",         el=>el.GetProperty("target_date").ToString().Equals("") ? "-" : el.GetProperty("target_date").ToString()}, //factor out        
        // { "Completion Date",     el=>el.GetProperty("completion_date").ToString().Equals("") ? "-" : el.GetProperty("completion_date").ToString()}, // factor out
        // { "Issued by",           el=>el.GetProperty("issued_by_detail").GetProperty("username").ToString() + el.GetProperty("issued_by_detail").GetProperty("first_name").ToString() + " " + el.GetProperty("issued_by_detail").GetProperty("last_name").ToString() ?? ""},
        
        { "Reference",           el => el.GetProperty("reference").ToString()},
        { "Part",                el => el.GetProperty("part_name").ToString()},
        { "IPN",                 el => SafeGetProperty(el.GetProperty("part_detail"), "IPN")},
        { "Description",         el => el.GetProperty("title").ToString()},
        { "Completed",           el => SafeGetCompletedItems(el, "completed", "quantity")},
        { "Build Status",        el => el.GetProperty("status_text").ToString()},
        { "External",            el => el.GetProperty("external").ToString().Equals("False") ? "No" : "Yes"},
        { "Target Date",         el => SafeGetDate(el, "target_date")},
        { "Completion Date",     el => SafeGetDate(el, "completion_date")},
        { "Issued by",           el => SafeGetProperty(el.GetProperty("issued_by_detail"), "username") + SafeGetProperty(el.GetProperty("issued_by_detail"), "first_name") + " " + SafeGetProperty(el.GetProperty("issued_by_detail"), "last_name") },
        { "Responsible",         el => SafeGetProperty(el.GetProperty("responsible_detail"), "name")}

    //============================================= Order Details Page ===============================================================================
        // { "Revision",               el => SafeGetProperty(el.GetProperty("part_detail"), "revision")},
        // { "Status",                 el => el.GetProperty("status_text").ToString()},
        // { "Purchase Order",         el => "TODO: external PO"},
        // { "Can Build",              el => "Placeholder."},
        // { "Source Location",        el => "Any location"},
        // { "Issued By",              el => SafeGetProperty(el.GetProperty("issued_by_detail"), "username") + SafeGetProperty(el.GetProperty("issued_by_detail"), "first_name") + " " + SafeGetProperty(el.GetProperty("issued_by_detail"), "last_name") },
        // { "Created",                el => el.GetProperty("creation_date").ToString()},
        // { "Start Date",             el => el.GetProperty("start_date").ToString()},
        // { "Project Code",           el => el.GetProperty("project_code_label").ToString()},
        // { "Build Quantity",         el => el.GetProperty("quantity").ToString()},
        // { "Completed Outputs",      el => el.GetProperty("completed").ToString()}
    };

    /// <summary>
    /// Safely get a property from a JsonElement, returning an empty string if the property does not exist or is null.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public static string SafeGetProperty(JsonElement element, string property)
    {
        // element != null, Guard to check JsonElement? points to a JsonElement object (Does it exist in the JSON Response)
        // element.Value.ValueKind, if element exists, check it contains a non-null value ("key": "Something not null")
        if (element.ValueKind != JsonValueKind.Null && element.TryGetProperty(property, out JsonElement foundProperty))
        {
            return foundProperty.ToString();
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// Get Date property, if empty return "-"
    /// </summary>
    /// <param name="element"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public static string SafeGetDate(JsonElement element, string property)
    {
        if (element.GetProperty(property).ToString().Equals(""))
        {
            return "-";
        }
        else
        {
            return element.GetProperty(property).ToString();
        }
    }

    /// <summary>
    /// Get Completed Items in the format of "x / y" where x is the numerator and y is the denominator
    /// </summary>
    /// <param name="element"></param>
    /// <param name="numerator"></param>
    /// <param name="denom"></param>
    /// <returns></returns>
    public static string SafeGetCompletedItems(JsonElement element, string numerator, string denom)
    {
        // el.GetProperty("completed").ToString() + " / " + el.GetProperty("quantity").ToString().Substring(0, el.GetProperty("quantity").ToString().IndexOf('.'))         
        return element.GetProperty(numerator).ToString() + " / " + element.GetProperty(denom).ToString().Substring(0, element.GetProperty(denom).ToString().IndexOf('.'));
    }
}