

using System.Text.Json;

namespace InvenTreeAutomationFramework.TestDataModels;

public static class TestDataProvider
{
    public static T DeserializeResponse<T>(string jsonString)
    {
        T? dm = JsonSerializer.Deserialize<T>(jsonString);
        if (dm != null)
        {
            return dm;
        }
        else
        {
            throw new Exception("Failed to Deserialize JSON to Object.");
        }
    }

    //TODO: Need to refactor when grid row created
    public static Dictionary<string, string> DeserializeResponseToDictionary<T>(string jsonString)
    {
        Dictionary<string, string>? d = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        if (d != null)
        {
            return d;
        }
        else
        {
            throw new Exception("Failed to Deserialize JSON to Dictionary.");
        }
    }
}