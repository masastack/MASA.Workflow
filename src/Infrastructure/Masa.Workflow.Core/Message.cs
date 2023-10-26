namespace Masa.Workflow.Core;

public sealed class Message : DynamicObject
{
    readonly Dictionary<string, object?> _dictionary = new();

    public Message()
    {
        Id = Guid.NewGuid();
        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public Guid Id { get; set; }

    public long Timestamp { get; set; }

    public object? this[string key]
    {
        get
        {
            if (_dictionary.TryGetValue(key.ToLower(), out var value))
            {
                return value;
            }

            return null;
        }
        set
        {
            key = key.ToLower();
            _dictionary[key] = value;
        }
    }

    public dynamic Payload { get; set; } = new ExpandoObject();

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
