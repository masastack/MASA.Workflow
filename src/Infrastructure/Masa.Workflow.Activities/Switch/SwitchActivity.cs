using Masa.Workflow.Core.Models;

namespace Masa.Workflow.Activities.Switch;

public class SwitchActivity : MasaWorkflowActivity<SwitchMeta>
{
    IRulesEngineClient _rulesEngineClient;

    public SwitchActivity(Msg msg, IRulesEngineClient rulesEngineClient)
        : base(msg)
    {
        _rulesEngineClient = rulesEngineClient;
    }

    public override async Task<ActivityExecutionResult> RunAsync(SwitchMeta meta)
    {
        System.Console.WriteLine("SwitchActivity start run.");
        var _rules = new List<object>();
        int i = 0;
        foreach (var rule in meta.Rules)
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
                Expression = GetExpressionString(meta.Property, rule),
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
        var ruleResults = await _rulesEngineClient.ExecuteAsync(ruleRaw, _msg);
        var otherwise = ruleResults.FirstOrDefault(r => r.IsValid && r.RuleName == Operator.Otherwise.ToString());
        var passResults = ruleResults.Where(r => r.IsValid && r.RuleName != Operator.Otherwise.ToString()).Select(r => r.ActionResult.Output as List<Guid>)
            .Where(r => r != null).ToList();
        var result = new ActivityExecutionResult()
        {
            ActivityId = meta.ActivityId.ToString(),
            Status = ActivityStatus.Finished
        };
        if (passResults.Count > 0)
        {
            if (meta.SwitchMode == SwitchMode.FirstMatch)
            {
                result.Wires.Add(passResults.First()!);
            }
            else
            {
                result.Wires.AddRange(passResults);
            }
            return result;
        }
        if (otherwise != null)
        {
            result.Wires.Add(otherwise.ActionResult.Output as List<Guid>);
        }
        return result;

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