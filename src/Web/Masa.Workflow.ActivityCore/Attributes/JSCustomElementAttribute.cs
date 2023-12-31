﻿namespace Masa.Workflow.ActivityCore.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class JSCustomElementAttribute : Attribute
{
    public JSCustomElementAttribute()
    {
    }

    public JSCustomElementAttribute(string name)
    {
        Name = name;
    }

    public string? Name { get; }

    public bool IncludeNamespace { get; set; }
}
