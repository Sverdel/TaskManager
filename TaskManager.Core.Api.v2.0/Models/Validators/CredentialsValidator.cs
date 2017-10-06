using FluentValidation;
using TaskManager.Core.Api.Models.Dto;

namespace TaskManager.Core.Api.Models.Validators
{
    public class CredentialsValidator : AbstractValidator<CredentialsDto>
    {
        public CredentialsValidator()
        {
            RuleFor(cred => cred.Name).NotNull().NotEmpty();
            RuleFor(cred => cred.Password).NotNull().NotEmpty();
        }
    }
}
