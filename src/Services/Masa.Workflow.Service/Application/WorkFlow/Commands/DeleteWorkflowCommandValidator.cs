namespace Masa.Workflow.Service.Application.WorkFlow.Commands;

public class DeleteWorkflowCommandValidator : AbstractValidator<DeleteWorkflowCommand>
{
    public DeleteWorkflowCommandValidator()
    {
        RuleFor(cmd => cmd.WorkflowId).NotEmpty().WithMessage("The order items cannot be empty");
    }
}