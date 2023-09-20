namespace Masa.Workflow.Service.Application.WorkFlow.Commands;

public class SaveWorkflowCommandValidator : AbstractValidator<SaveWorkflowCommand>
{
    public SaveWorkflowCommandValidator()
    {
        //RuleFor(cmd => cmd.Items).Must(cmd => cmd.Any()).WithMessage("The order items cannot be empty");
    }
}