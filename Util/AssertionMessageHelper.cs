namespace InvenTreeAutomationFramework.Util;

public static class AssertionMessageHelper
{
    public static string PrintEnumerable(IEnumerable<KeyValuePair<string, string>> e)
    {
        //Note IEnumerable is an interface, Dictionary implements this interface as well
        string s = "{";

        foreach (var pair in e)
        {
            s += pair + ",";
        }
        s = s.Remove(s.LastIndexOf(s.Last()));
        s += "} \n";
        return s;
    }
}
