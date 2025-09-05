using System.Text.Json;
using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Enums;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Util;

// Helper class manage API responses and extract relevant data for validation
public static class APIHelper
{
    public static IResponse? waitForResponseTask;
    public static JsonElement? currentResponseJson;

    /// <summary>
    /// Start waiting for a specific API endpoint response and store it in waitForResponseTask
    /// </summary>
    /// <param name="page"></param>
    /// <param name="urlEndpoint"></param>
    /// <returns></returns>
    [AllureStep("Wait For API Endpoint Response [{urlEndpoint}]")]
    public static async Task StartWaitingForResponse(IPage page, string urlEndpoint)
    {
        waitForResponseTask = await page.WaitForResponseAsync(
           response => response.Url.Contains(urlEndpoint) &&
           response.Status == 200);

        await SetResponse();
    }

    /// <summary>
    /// Start waiting for Store Network Response Specified For Table Response
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    [AllureStep("Wait For API Endpoint Response For An Inventory Table")]
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

    /// <summary>
    /// Store Network Response Specified for Item/Order Details Page
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    [AllureStep("Wait For API Endpoint Response For An Item/Order Details Page")]
    public static async Task WaitForOrderDetailsResponse(IPage page)
    {
        waitForResponseTask = await page.WaitForResponseAsync(
            response => response.Url.Contains(APIEndpoints.APIEndpointDictionary[APIHelperEnums.Root]) &&
            response.Url.Contains(APIEndpoints.COMMON_ORDER_DETAIL_PAYLOAD)
            && response.Status == 200);

        await SetResponse();
    }

    /// <summary>
    /// Store Network Response Specified for an Item/Order Details Page with Specified PK key
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pk"></param>
    /// <returns></returns>
    [AllureStep("Wait For API Endpoint Response For An Item/Order Details Page With Pk: [{pk}]")]
    public static async Task WaitForWithPKOrderDetailsResponse(IPage page, string pk)
    {
        /*
        Used to wait for a specific response for Order Details, using PK to specify
        which response to catch
            - There is a scenario where, when editing a build order with a Parent Build,
            there will be 2 similar responses in the API Network tab. Capturing the PK
            key and re-using it will help identify the one we edited.

            - 26/?part_detail=true and 29/?part_detail=true are returned, given item 26 has parent build as 29
            - further specify the response to wait for pk = 26 to wait for the correct response.
        */
        waitForResponseTask = await page.WaitForResponseAsync(
            response => response.Url.Contains(pk + "/?") && response.Url.Contains(APIEndpoints.APIEndpointDictionary[APIHelperEnums.Root]) &&
            response.Url.Contains(APIEndpoints.COMMON_ORDER_DETAIL_PAYLOAD)
            && response.Status == 200);

        await SetResponse();
    }

    /// <summary>
    /// Set class variable currentResponseJson if it is not Null
    /// </summary>
    /// <returns></returns>
    [AllureStep("Received And Set Response")]
    public static async Task SetResponse()
    {
        if (waitForResponseTask != null)
        {
            currentResponseJson = await waitForResponseTask.JsonAsync();
        }
    }

    /// <summary>
    /// Return captured API response
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [AllureStep("Get API Response")]
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
    /// <summary>
    /// Takes an API response containing a list of items and a list of table headers. 
    /// It builds a dictionary where each key is the item's primary key (pk), and the value is another dictionary 
    /// mapping each table header to its corresponding value from the API response. This allows you to easily 
    /// compare or display table data from the API in your UI tests.
    /// </summary>
    /// <param name="response"></param>
    /// <param name="tableRowHeaders"></param>
    /// <returns></returns>
    [AllureStep("Map The UI Table Header Names To The Corresponding API Results Response Values")]
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

    /// <summary>
    /// Takes an API Response from the context Item/Order Page and a list of table row headers, 
    /// then maps each header to its corresponding value in the API response using predefined translation functions. 
    /// </summary>
    /// <param name="response"></param>
    /// <param name="tableRowHeaders"></param>
    /// <returns></returns>
    [AllureStep("Map The UI Details Page Fields To The Corresponding API Details Page Response Values")]
    public static Dictionary<string, string> TranslateHeaderToAPIKey(JsonElement? response, List<string> tableRowHeaders)
    {
        Dictionary<string, string> translatedResult = new Dictionary<string, string>();

        if (response != null && response.Value.ValueKind != JsonValueKind.Null)
        {
            //Enable Translate Header To API Key without getting the result property from the Response, any api repsonse can be translated
            foreach (string header in tableRowHeaders)
            {
                translatedResult.Add(header, APIHeaderHelper.TranslateToAPIKey[header](response.Value));
            }
        }

        return translatedResult;
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

    /*
        This method takes a list of JsonElements and finds the JsonElement that has the matching property value, matching propertyValue and responsePropertyToMatch (of current JsonElement in iteration)
        Then returns the property, responsePropertyToReturn of the specified JsonElement
    */
    public static JsonElement GetPropertyInList(APIPropertyEnums responseResults, APIPropertyEnums responsePropertyToMatch, APIPropertyEnums responsePropertyToReturn, string propertyValue)
    {
        if (currentResponseJson != null && currentResponseJson.Value.ValueKind != JsonValueKind.Null)
        {
            List<JsonElement> list = currentResponseJson.Value.GetProperty(APIProperty.APIPropertyDictionary[responseResults]).EnumerateArray().ToList();

            foreach (JsonElement item in list)
            {
                if (item.GetProperty(APIProperty.APIPropertyDictionary[responsePropertyToMatch]).ToString().Equals(propertyValue))
                {
                    return item.GetProperty(APIProperty.APIPropertyDictionary[responsePropertyToReturn]);
                }
            }

            return new JsonElement(); //Return empty JsonElement object
        }
        else
        {
            throw new Exception("currentResponseJson has not been set or is null");
        }
    }
}