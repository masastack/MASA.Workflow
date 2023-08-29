namespace Masa.Workflow.Activity.Activities.Switch;

public enum Operator
{
    [Description("==")]
    Eq,
    [Description("!=")]
    Neq,
    [Description("<")]
    Lt,
    [Description("<=")]
    Lte,
    [Description(">")]
    Gt,
    [Description(">=")]
    Gte,
    Null,
    NotNull,
    Empty,
    NotEmpty,
    [Description("类型是")]
    IsType,
    [Description("为假")]
    True,
    [Description("为真")]
    False,
    Contains,
    [Description("匹配正则表达式")]
    Regex,
    [Description("除此以外")]
    Otherwise = 99
}
