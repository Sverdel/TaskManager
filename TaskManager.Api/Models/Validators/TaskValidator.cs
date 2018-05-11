using FluentValidation;
using TaskManager.Api.Models.Dto;

namespace TaskManager.Api.Models.Validators
{
    public class TaskValidator : AbstractValidator<TaskDto>
    {
        public TaskValidator()
        {
            RuleFor(thes => thes.Name).NotNull().NotEmpty();
            RuleFor(thes => thes.ActualTimeCost).GreaterThan(0d);
            RuleFor(thes => thes.PlanedTimeCost).GreaterThan(0d);
            RuleFor(thes => thes.RemainingTimeCost).GreaterThan(0d);
            RuleFor(thes => thes.UserId).NotNull().NotEmpty();
        }
    }
}
