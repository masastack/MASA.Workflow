namespace Masa.Workflow.Activity.Activities;

public class SwitchActivity : MasaWorkflowActivity<SwitchMeta, List<List<Guid>>>
{
    IRulesEngineClient _rulesEngineClient;

    public SwitchActivity(WorkflowHub workflowHub, IRulesEngineClient rulesEngineClient)
        : base(workflowHub)
    {
        _rulesEngineClient = rulesEngineClient;
    }

    public override async Task<List<List<Guid>>?> RunAsync(Msg<SwitchMeta> msg)
    {
        var _rules = new List<object>();
        int i = 0;
        foreach (var rule in msg.Meta.Rules)
        {
            _rules.Add(new
            {
                RuleName = rule.Operator.ToString(),
                //LocalParams = new List<object> {
                //    new {
                //        Name ="i",
                //        Expression=$"{i}"
                //    }
                //},
                Expression = GetExpressionString(msg.Meta.Property, rule),
                Actions = new
                {
                    OnSuccess = new
                    {
                        Name = "OutputExpression",
                        Context = new
                        {
                            Expression = $"Meta.Wires[{i}]"
                        }
                    },
                    //OnFailure = new
                    //{
                    //    Name = "",
                    //    Context = new
                    //    {

                    //    }
                    //}
                }
            });
            i++;
        }
        var ruleRaw = JsonSerializer.Serialize(new
        {
            Rules = _rules
        });
        var ruleResults = await _rulesEngineClient.ExecuteAsync(ruleRaw, msg);

        var otherwise = ruleResults.FirstOrDefault(r => r.IsValid && r.RuleName == Operator.Otherwise.ToString());
        var passResults = ruleResults.Where(r => r.IsValid && r.RuleName != Operator.Otherwise.ToString()).Select(r => r.ActionResult.Output as List<Guid>).ToList();

        if (passResults.Count > 0)
        {
            if (msg.Meta.EnforceRule == EnforceRule.FirstMatch)
            {
                return new List<List<Guid>> { passResults.First()! };
            }
            return passResults;
        }
        if (otherwise != null)
        {
            return new List<List<Guid>> { otherwise.ActionResult.Output as List<Guid> };
        }
        return null;

        string GetExpressionString(string property, Rule rule)
        {
            return rule.Operator switch
            {
                Operator.Eq => $"{property} == Convert.ChangeType({rule.Value},{property}.GetType())",
                Operator.Neq => $"{property} != Convert.ChangeType({rule.Value},{property}.GetType())",
                Operator.Lt => $"{property}.ToString() < \"{rule.Value}\"",
                Operator.Lte => $"{property}.ToString() <= \"{rule.Value}\"",
                Operator.Gt => $"{property}.ToString() > \"{rule.Value}\"",
                Operator.Gte => $"{property}.ToString() >= \"{rule.Value}\"",
                Operator.True => $"Ruler.IsTrue({property})",
                Operator.False => $"!Ruler.IsTrue({property})",
                Operator.Null => $"{property} == null",
                Operator.NotNull => $"{property} != null",
                Operator.Empty => $"Ruler.IsEmpty({property})",
                Operator.NotEmpty => $"!Ruler.IsEmpty({property})",
                Operator.IsType => $"{property}.GetType().Name.ToLower() == \"{rule.Value}\"",
                Operator.Contains => $"{property}.ToString().Contains(\"{rule.Value}\")",
                Operator.Regex => $"new Regex({rule.Value}).IsMatch({property})",
                Operator.Otherwise => "true",
                _ => throw new NotImplementedException(),
            };
        }
    }
}