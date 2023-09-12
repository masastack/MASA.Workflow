namespace Masa.Workflow.Service.Application.Orders.Commands;

public class CreateWorkflowCommandValidator : AbstractValidator<CreateWorkflowCommand>
{
    public CreateWorkflowCommandValidator()
    {
        //RuleFor(cmd => cmd.Items).Must(cmd => cmd.Any()).WithMessage("The order items cannot be empty");
    }
}