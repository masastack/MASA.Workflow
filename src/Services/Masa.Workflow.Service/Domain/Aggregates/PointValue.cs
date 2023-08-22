namespace Masa.Workflow.Service.Domain.Aggregates;

public class PointValue : ValueObject
{
    public int X { get; private set; }

    public int Y { get; private set; }

    public PointValue(int x, int y)
    {
        X = x; Y = y;
    }

    protected override IEnumerable<object> GetEqualityValues()
    {
        yield return X;
        yield return Y;
    }
}
