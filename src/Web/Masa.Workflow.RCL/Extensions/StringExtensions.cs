namespace Masa.Workflow.RCL.Extensions;

public static class StringExtensions
{
    public static bool IsJson(this string? str, JsonValueKind rootKind = JsonValueKind.Undefined)
    {
        if (str == null)
        {
            return false;
        }

        try
        {
            using var doc = JsonDocument.Parse(str);
            return doc.RootElement.ValueKind == rootKind;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
