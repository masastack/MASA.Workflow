namespace Masa.Workflow.Core.Models;

public class RetryPolicy
{
    public int MaxNumberOfAttempts { get; private set; } = 3;

    public TimeSpan FirstRetryInterval { get; private set; } = TimeSpan.FromSeconds(5);

    public double BackoffCoefficient { get; private set; } = 1.0;

    public TimeSpan MaxRetryInterval { get; private set; } = TimeSpan.FromHours(1);

    public TimeSpan RetryTimeout { get; private set; } = Timeout.InfiniteTimeSpan;
}
