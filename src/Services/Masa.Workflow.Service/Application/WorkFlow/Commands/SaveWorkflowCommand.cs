public record SaveWorkflowCommand(WorkflowRequest Request) : Command
{
    public Guid Id { get; set; }
}