using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Folder.AddFolder
{
    public class AddFolderCommandValidator : AbstractValidator<AddFolderCommand>
    {
        public AddFolderCommandValidator()
        {
            RuleFor(command => command.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(command => command.ProjectId).NotNull().GreaterThan(0).WithMessage("Invalid project");
        }
    }
}
