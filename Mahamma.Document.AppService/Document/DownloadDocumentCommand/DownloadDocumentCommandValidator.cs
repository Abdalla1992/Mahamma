using FluentValidation;

namespace Mahamma.Document.AppService.Document.DownloadDocumentCommand
{
    public class DownloadDocumentCommandValidator : AbstractValidator<DownloadDocumentCommand>
    {
        public DownloadDocumentCommandValidator()
        {
            //RuleFor(command => command.file).NotEmpty().WithMessage("there is no file");
        }
    }
}
