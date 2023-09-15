namespace Masa.Workflow.Core;

public static class ActivityProvider
{
    static Dictionary<string, string> _dictionary = new Dictionary<string, string>();

    public static bool TryGet(string type, out string? activity)
    {
        return _dictionary.TryGetValue(type, out activity);
    }

    public static void Set(string type, string activity)
    {
        if (_dictionary.ContainsKey(type))
        {
            throw new ArgumentException($"key {type} already exists");
        }
        _dictionary[type] = activity;
    }
}
