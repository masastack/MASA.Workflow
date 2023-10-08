namespace Masa.Workflow.Core.Models;

[DebuggerTypeProxy(typeof(TypeProxy))]
public class ContextVariables : Dictionary<string, object>
{
    public void Set(string name, string? value)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(name);
        if (value != null)
        {
            _variables[name] = value;
        }
        else
        {
            _variables.TryRemove(name, out _);
        }
    }

    public bool TryGetValue(string name, [NotNullWhen(true)] out string? value)
    {
        if (_variables.TryGetValue(name, out value))
        {
            return true;
        }

        value = null;
        return false;
    }

    #region private ================================================================================

    /// <summary>
    /// Important: names are case insensitive
    /// </summary>
    private readonly ConcurrentDictionary<string, string> _variables = new(StringComparer.OrdinalIgnoreCase);

    private sealed class TypeProxy
    {
        private readonly ContextVariables _variables;

        public TypeProxy(ContextVariables variables) => _variables = variables;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<string, string>[] Items => _variables._variables.ToArray();
    }

    #endregion
}
