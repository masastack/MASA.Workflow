namespace Masa.Workflow.Activities.Switch;

public static class Ruler
{
    public static bool IsEmpty(object obj)
    {
        if (obj is string str)
        {
            return string.IsNullOrEmpty(str);
        }
        if (obj is ICollection collection)
        {
            return collection.Count == 0;
        }
        throw new ArgumentException("input value should be string or collection");
    }

    public static bool IsTrue(object obj)
    {
        if (obj is bool _bool)
        {
            return _bool;
        }
        throw new ArgumentException("input value should be bool");
    }
}
