using Application.Features.ApplicationRoles.Commands.RequestModels;
using FluentValidation;

namespace Application.Features.ApplicationRoles.Commands.Validators
{
    public class AddRoleValidator : AbstractValidator<AddRoleCommand>
    {
        public AddRoleValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}