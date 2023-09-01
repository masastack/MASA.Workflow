namespace Masa.Workflow.ActivityCore;

public abstract class MetaBase
{
    public Guid ActivityId { get; } =  Guid.NewGuid();

    public List<List<Guid>> Wires { get; } = new();
}

public sealed class Msg : DynamicObject
{
    Dictionary<string, object> _dictionary = new Dictionary<string, object>();

    public dynamic Payload { get; set; } = new ExpandoObject();

    public object? this[string key]
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
            if (_dictionary[key] != value)
            {
#pragma warning disable CS8601 // 引用类型赋值可能为 null。
                _dictionary[key] = value;
#pragma warning restore CS8601 // 引用类型赋值可能为 null。
            }
        }
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        var key = binder.Name.ToLower();
        return _dictionary.TryGetValue(key, out result);
    }

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        _dictionary[binder.Name.ToLower()] = value!;
        return true;
    }

    public override IEnumerable<string> GetDynamicMemberNames()
    {
        return _dictionary.Keys;
    }
}
