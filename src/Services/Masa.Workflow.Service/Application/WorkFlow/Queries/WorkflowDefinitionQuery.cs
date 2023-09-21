namespace Masa.Workflow.Service.Application.WorkFlow.Queries;

public record WorkflowDefinitionQuery(Guid Id) : DomainQuery<WorkflowDefinition>
{
    public override WorkflowDefinition Result { get; set; } = new();
}
