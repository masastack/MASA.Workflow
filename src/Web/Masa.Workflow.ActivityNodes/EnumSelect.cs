using BlazorComponent.I18n;

namespace Masa.Workflow.ActivityNodes;

[Obsolete("delete this use masa stack component")]
public class EnumSelect<TValue> : MSelect<KeyValuePair<string, TValue>, TValue, TValue> where TValue : struct, Enum
{
    [Inject]
    public I18n I18N { get; set; } = default!;
    
    [Parameter]
    public Func<TValue, bool> Filter { get; set; } = _ => true;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        Clearable = true;
        Items = Enum.GetValues<TValue>().Where(Filter).Select(e => new KeyValuePair<string, TValue>(e.ToString(), e)).ToList();
        ItemText = kv => I18N.T(kv.Key, true);
        ItemValue = kv => kv.Value;
        await base.SetParametersAsync(parameters);
    }
}
