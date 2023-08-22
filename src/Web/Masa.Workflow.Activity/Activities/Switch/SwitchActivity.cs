using Masa.Workflow.Activity.Activities.Switch;
using System.Collections;
using System.Linq.Expressions;

namespace Masa.Workflow.Activity.Activities;

public class SwitchActivity : MasaWorkflowActivity<SwitchActivityMeta, List<Guid>>
{
    public SwitchActivity(WorkflowHub workflowHub)
        : base(workflowHub)
    {
    }

    public override Task<List<Guid>> RunAsync(Msg<SwitchActivityMeta> msg)
    {
        //todo get Msg.Payload
        ConstantExpression switchValue = Expression.Constant(3);

        var switchCaseList = new List<SwitchCase>();
        int i = 0;
        foreach (var rule in msg.Meta.Rules)
        {
            var wires = msg.Meta.Wires[i] ?? new();
            switchCaseList.Add(Expression.SwitchCase(
                        Expression.Constant(wires),
                        GetExpression(rule.Key, rule.Value)
                    ));
            i++;
        }

        var switchExpr = Expression.Switch(switchValue, Expression.Constant(new List<Guid> { Guid.Empty }), switchCaseList.ToArray());
        var result = Expression.Lambda<Func<List<Guid>>>(switchExpr).Compile()();

        Expression GetExpression(Operator _operator, object? value)
        {
            Expression? expression = null;
            if (_operator == Operator.Null)
            {
                expression = Expression.Equal(switchValue, Expression.Constant(null, switchValue.Type));
            }
            else if (_operator == Operator.NotNull)
            {
                expression = Expression.NotEqual(switchValue, Expression.Constant(null, switchValue.Type));
            }
            else if (_operator == Operator.Empty)
            {
                expression = GetEmptyExpression(switchValue, value, true);
            }
            else if (_operator == Operator.NotEmpty)
            {
                expression = GetEmptyExpression(switchValue, value, false);
            }
            else if (_operator == Operator.IsType)
            {
                expression = Expression.TypeIs(switchValue, value!.GetType());
            }
            else if (_operator == Operator.True)
            {
                expression = Expression.IsTrue(switchValue);
            }
            else if (_operator == Operator.False)
            {
                expression = Expression.IsFalse(switchValue);
            }

            if (expression != null)
            {
                return expression;
            }

            var expressionType = _operator switch
            {
                Operator.Eq => ExpressionType.Equal,
                Operator.Neq => ExpressionType.NotEqual,
                Operator.Lt => ExpressionType.LessThan,
                Operator.Lte => ExpressionType.LessThanOrEqual,
                Operator.Gt => ExpressionType.GreaterThan,
                Operator.Gte => ExpressionType.GreaterThanOrEqual,
                _ => throw new ArgumentException("Invalid operator: " + _operator)
            };

            //正则

            //method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            //condition = Expression.Call(property, method, value);

            return Expression.MakeBinary(ExpressionType.Equal, switchValue, Expression.Constant(value));

            Expression GetEmptyExpression(Expression switchValue, object? value, bool isEmpty)
            {
                if (value is string)
                {
                    return isEmpty
                        ? Expression.Equal(switchValue, Expression.Constant(string.Empty, typeof(string)))
                        : Expression.NotEqual(switchValue, Expression.Constant(string.Empty, typeof(string)));
                }
                else if (value is ICollection collectionValue)
                {
                    var countValue = Expression.Property(Expression.Constant(collectionValue), "Count");
                    var zeroValue = Expression.Constant(0);
                    return isEmpty
                        ? Expression.Equal(countValue, zeroValue)
                        : Expression.NotEqual(countValue, zeroValue);
                }
                else
                {
                    throw new ArgumentException("Value should be a string or collection for operator: " + _operator);
                }
            }

        }

        return Task.FromResult(result ?? new());
    }
}