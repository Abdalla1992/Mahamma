using FluentValidation;

namespace Mahamma.Document.AppService.Document.UploadDocumentCommand
{
    public class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
    {
        public UploadDocumentCommandValidator()
        {
            //RuleFor(command => command.file).NotEmpty().WithMessage("there is no file");
        }
    }
}
