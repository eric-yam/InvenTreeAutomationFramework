using System.Text.Json;
using System.Threading.Tasks;
using InvenTreeAutomationFramework.Enums;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Util;

public static class APIHelper
{
    public static IResponse? waitForResponseTask;
    public static JsonElement? currentResponseJson;

    //Store Network Response
    public static async Task StartWaitingForResponse(IPage page, string urlEndpoint)
    {
        waitForResponseTask = await page.WaitForResponseAsync(
           response => response.Url.Contains(urlEndpoint) &&
           response.Status == 200);

        await SetResponse();
    }

    //Store Network Response Specified For Table Response
    public static async Task WaitForTableResponse(IPage page)
    {
        /*
        https://demo.inventree.org/api/build/?part_detail=true&limit=25&offset=0
        https://demo.inventree.org/api/order/po/?supplier_detail=true&limit=25&offset=0

        In the network response, the commonality between the endpoints for loading the table rows contains:
            - "/api/"
            - followed by a dynamically changing path depending on the table being loaded -> /build/ or /order/po/
            - "detail=true&limit=25&offset=0" 
        */

        waitForResponseTask = await page.WaitForResponseAsync(
            response => response.Url.Contains(APIEndpoints.APIEndpointDictionary[APIHelperEnums.Root]) &&
            response.Url.Contains(APIEndpoints.COMMON_TABLE_PAYLOAD)
            && response.Status == 200);

        await SetResponse();
    }

    //Get Response and set as JsonElement as Class Variable
    public static async Task SetResponse()
    {
        if (waitForResponseTask != null)
        {
            currentResponseJson = await waitForResponseTask.JsonAsync();
        }
    }

    public static JsonElement? GetResponse()
    {
        if (currentResponseJson != null)
        {
            return currentResponseJson;
        }
        else
        {
            //Handles null dereference warning
            throw new Exception("Response JSON was null.");
        }
    }

    //TODO: Revisit
    public static Dictionary<string, Dictionary<string, string>> GetResponseTableResults(JsonElement? response, List<string> tableRowHeaders)
    {
        Dictionary<string, Dictionary<string, string>> ResponseResults = new Dictionary<string, Dictionary<string, string>>();

        JsonElement.ArrayEnumerator? resultList = response?.GetProperty(APIProperty.APIPropertyDictionary[APIPropertyEnums.Results]).EnumerateArray();
        if (resultList != null)
        {
            foreach (JsonElement result in resultList)
            {
                string responseItemKey = result.GetProperty(APIProperty.APIPropertyDictionary[APIPropertyEnums.PK]).ToString() ?? "";
                Dictionary<string, string> currentResponseRow = new Dictionary<string, string>();
                foreach (string header in tableRowHeaders)
                {
                    currentResponseRow.Add(header, APIHeaderHelper.TranslateToAPIKey[header](result));
                }

                ResponseResults.Add(responseItemKey, currentResponseRow);
            }
        }
        return ResponseResults;
    }

    //Gets JsonElement property that is a list and returns it as List
    public static List<JsonElement> GetListProperty(APIPropertyEnums propertyName)
    {
        /*
            If currentResponseJson hasn't been set yet AND
            If currentResponseJson exists, it's Json Value is not null
        */
        if (currentResponseJson != null && currentResponseJson.Value.ValueKind != JsonValueKind.Null)
        {
            return currentResponseJson.Value.GetProperty(APIProperty.APIPropertyDictionary[propertyName]).EnumerateArray().ToList();
        }
        else
        {
            //Handles null dereference warning
            throw new Exception("currentResponseJson has not been set or is null");
        }
    }
}