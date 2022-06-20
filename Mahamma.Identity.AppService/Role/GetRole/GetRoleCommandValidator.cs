using FluentValidation;
using Mahamma.Identity.Domain.Role.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.GetRole
{
    class GetRoleCommandValidator : AbstractValidator<GetRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;
        public GetRoleCommandValidator(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;

            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Role Id is not provided");
        }


        public bool IsUniqueRole(GetRoleCommand addRoleCommand)
        {
            return (_roleRepository.GetRoleById(addRoleCommand.Id).Result) != null;
        }


    }
}
