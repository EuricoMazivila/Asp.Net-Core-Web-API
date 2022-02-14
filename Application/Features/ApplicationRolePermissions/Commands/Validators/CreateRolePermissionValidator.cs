using Application.Features.ApplicationRolePermissions.Commands.RequestModels;
using FluentValidation;

namespace Application.Features.ApplicationRolePermissions.Commands.Validators
{
    public class CreateRolePermissionValidator : AbstractValidator<CreateRolePermissionCommand>
    {
        public CreateRolePermissionValidator()
        {
            RuleFor(x => x.ApplicationPermissionId).NotEmpty();
            RuleFor(x => x.ApplicationRoleId).NotEmpty();
            
        }
    }
}