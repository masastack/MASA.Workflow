using System.Text;

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

public static class JsonDocumentExtensions
{
    public static string ToJsonString(this JsonDocument doc, bool indented = true)
    {
        if (indented)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
            {
                Indented = true
            });
            doc.WriteTo(writer);
            writer.Flush();
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        return doc.RootElement.GetRawText();
    }
}
