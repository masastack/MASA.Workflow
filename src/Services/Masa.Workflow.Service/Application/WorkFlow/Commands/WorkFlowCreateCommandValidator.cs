namespace Masa.Workflow.Service.Application.Orders.Commands;

public class WorkFlowCreateCommandValidator : AbstractValidator<WorkFlowCreateCommand>
{
    public WorkFlowCreateCommandValidator()
    {
        //RuleFor(cmd => cmd.Items).Must(cmd => cmd.Any()).WithMessage("The order items cannot be empty");
    }
}