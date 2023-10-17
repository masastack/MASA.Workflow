namespace Masa.Workflow.Service.Domain.Aggregates;

public class RetryPolicyValue : ValueObject
{
    /// <summary>
    /// Gets the max number of attempts for executing a given task.
    /// </summary>
    public int MaxNumberOfAttempts { get; private set; } = 3;

    /// <summary>
    /// Gets the amount of time to delay between the first and second attempt.
    /// </summary>
    public TimeSpan FirstRetryInterval { get; private set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets the exponential back-off coefficient used to determine the delay between subsequent retries.
    /// </summary>
    /// <value>
    /// Defaults to 1.0 for no back-off.
    /// </value>
    public double BackoffCoefficient { get; private set; } = 1.0;

    /// <summary>
    /// Gets the maximum time to delay between attempts.
    /// </summary>
    /// <value>
    /// Defaults to 1 hour.
    /// </value>
    public TimeSpan MaxRetryInterval { get; private set; } = TimeSpan.FromHours(1);

    /// <summary>
    /// Gets the overall timeout for retries. No further attempts will be made at executing a task after this retry
    /// timeout expires.
    /// </summary>
    /// <value>
    /// Defaults to 12 hour,Dapr Default value is <see cref="Timeout.InfiniteTimeSpan"/>.
    /// </value>
    public TimeSpan RetryTimeout { get; private set; } = TimeSpan.FromHours(12);

    protected override IEnumerable<object> GetEqualityValues()
    {
        yield return MaxNumberOfAttempts;
        yield return FirstRetryInterval;
        yield return BackoffCoefficient;
        yield return RetryTimeout;
        yield return MaxRetryInterval;
    }
}
