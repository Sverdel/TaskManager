using FluentValidation;
using TaskManager.Api.Models.Dto;

namespace TaskManager.Api.Models.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(user => user.UserName).NotNull().NotEmpty();
            RuleFor(user => user.Password).NotNull().NotEmpty();
        }
    }
}
