using FluentValidation;
using Mahamma.Identity.Domain.Role.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.AddRole
{
    class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;
        public AddRoleCommandValidator(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;

            RuleFor(command => command.Name).NotEmpty().WithMessage("Role Name Is Empty");
            RuleFor(command => command).Must(IsUniqueRole).WithMessage("Role Name Is Already Exist");
        }


        public bool IsUniqueRole(AddRoleCommand addRoleCommand)
        {
            return !(_roleRepository.CheckRoleExistence(addRoleCommand.Name).Result);
        }


    }
}
