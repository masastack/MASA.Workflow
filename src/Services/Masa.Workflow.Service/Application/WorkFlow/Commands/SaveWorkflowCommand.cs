public record SaveWorkflowCommand(WorkflowSaveRequest Request) : Command
{
    public Guid Id { get; set; }
}