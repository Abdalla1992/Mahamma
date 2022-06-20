using FluentValidation;

namespace Mahamma.Document.AppService.Document.DeleteDocumentCommand
{
    public class DeleteDocumentCommandValidator : AbstractValidator<DeleteDocumentCommand>
    {
        public DeleteDocumentCommandValidator()
        {
            //RuleFor(command => command.file).NotEmpty().WithMessage("there is no file");
        }
    }
}
