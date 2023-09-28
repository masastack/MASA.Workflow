﻿namespace Masa.Workflow.Service.Domain.Aggregates;

public class Flow : FullAggregateRoot<Guid, Guid>
{
    private List<FlowVersion> _versions = new();
    private List<Activity> _activities = new();
    private List<FlowTask> _flowTasks = new();

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public bool Disabled { get; private set; }

    public bool IsDraft { get; private set; }

    public IReadOnlyCollection<FlowVersion> Versions => _versions;

    public IReadOnlyCollection<Activity> Activities => _activities;

    public IReadOnlyCollection<FlowTask> FlowTasks => _flowTasks;

    public Dictionary<string, object> EnvironmentVariables { get; private set; } = new();

    public Flow(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void SetStatus(WorkflowStatus status)
    {
        var flowTask = _flowTasks.LastOrDefault();
        if (status == WorkflowStatus.Running || flowTask == null)
        {
            _flowTasks.Add(new FlowTask(status, ""));
            return;
        }
        if (flowTask == null)
        {
            throw new UserFriendlyException("No Task");
        }
        flowTask.UpdateStatus(status);
    }
}
