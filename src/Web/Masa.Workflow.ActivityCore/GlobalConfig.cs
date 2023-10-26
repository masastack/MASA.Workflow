namespace Masa.Workflow.ActivityCore;

public static class GlobalConfig
{
    private static readonly Dictionary<string, object?> s_formInputDefaults = new()
    {
        { "Filled", true },
    };
    
    private static readonly Dictionary<string, object?> s_formInputDefaults2 = new()
    {
        { "Dense", true },
        { "Filled", true },
        { "HideDetails", (StringBoolean)"auto" }
    };

    private static readonly Dictionary<string, object?> s_switchDefaults = new()
    {
        { "Class", "mt-0" }
    };

    public static readonly Dictionary<string, IDictionary<string, object?>?> ComponentDefaults = new()
    {
        { "MSelect", s_formInputDefaults },
        { "MSwitch", s_switchDefaults },
        { "MTextarea", s_formInputDefaults },
        { "MTextField", s_formInputDefaults },
    };

    public static readonly Dictionary<string, IDictionary<string, object?>?> OutlinedCardDefaults = new()
    {
        { "MSelect", s_formInputDefaults2 },
        { "MSwitch", s_switchDefaults },
        { "MTextarea", s_formInputDefaults2 },
        { "MTextField", s_formInputDefaults2 },
    };
}
