namespace Masa.Workflow.Core.Models;

public class RetryPolicy
{
    public int MaxNumberOfAttempts { get; set; } = 3;

    public TimeSpan FirstRetryInterval { get; set; } = TimeSpan.FromSeconds(5);

    public double BackoffCoefficient { get; set; } = 1.0;

    public TimeSpan MaxRetryInterval { get; set; } = TimeSpan.FromHours(1);

    public TimeSpan RetryTimeout { get; set; } = Timeout.InfiniteTimeSpan;
}
