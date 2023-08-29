namespace Masa.Workflow.Activity;

public sealed class Msg<TMeta> : DynamicObject where TMeta : new()
{
    public Guid ActivityId { get; set; }

    public dynamic Payload { get; set; } = new ExpandoObject();

    public TMeta Meta { get; set; } = new TMeta();
}

public sealed class Payload : DynamicObject
{
    Dictionary<string, object> _dictionary = new Dictionary<string, object>();

    public object this[string key]
    {
        get
        {
            if (_dictionary.TryGetValue(key, out var value))
            {
                return value;
            }
            return null;
        }
        set
        {
            _dictionary[key] = value;
        }
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        var key = binder.Name;
        return _dictionary.TryGetValue(key, out result);
    }

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        _dictionary[binder.Name] = value!;
        return true;
    }
}
