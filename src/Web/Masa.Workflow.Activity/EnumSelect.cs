using BlazorComponent.I18n;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;

namespace Masa.Workflow.Activity;

[Obsolete("delete this use masa stack component")]
public class EnumSelect<TValue> : MSelect<KeyValuePair<string, TValue>, TValue, TValue> where TValue : struct, Enum
{
    [Inject]
    public I18n I18N { get; set; } = default!;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        Clearable = true;
        Items = Enum.GetValues<TValue>().Select(e => new KeyValuePair<string, TValue>(e.ToString(), e)).ToList();
        ItemText = kv => I18N.T(kv.Key, true);
        ItemValue = kv => kv.Value;
        await base.SetParametersAsync(parameters);
    }
}
