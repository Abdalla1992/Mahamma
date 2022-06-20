using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Company.Dto;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Company.Enum;
using Mahamma.Domain.Company.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.SendInvitationsFromFileCommand
{
    public class SendInvitationsFromFileCommandHandler : IRequestHandler<SendInvitationsFromFileCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly ICompanyInvitationRepository _companyInvitationRepository;
        private readonly ICompanyInvitationFileRepository _companyInvitationFileRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        #endregion

        public SendInvitationsFromFileCommandHandler(ICompanyInvitationRepository companyInvitationRepository, IHttpContextAccessor httpContext,
                                                      IMessageResourceReader messageResourceReader, ICompanyInvitationFileRepository companyInvitationFileRepository)
        {
            _companyInvitationRepository = companyInvitationRepository;
            _companyInvitationFileRepository = companyInvitationFileRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
        }

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(SendInvitationsFromFileCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            UserDto currentUser = (UserDto)_httpContext.HttpContext.Items["User"];

            try
            {
                if (request.UploadedFiles?.Count() > 0)
                {
                    // Validate excel file has email column and it is a valid file
                    var file = request.UploadedFiles[0];
                    ISheet sheet = ValidateExcelHasEmailColumn(file.FileUrl);

                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FileUploadedSuccessfullyAndMailsWillBeSent", currentUser.LanguageId);

                    await ProcessExcelSheet(sheet, currentUser, file);

                    // Run new Task to process excel file 
                    //System.Threading.Tasks.Task.Factory.StartNew(() =>
                    //{
                    //    ProcessExcelSheet(sheet, currentUser, file);
                    //});
                }
            }
            catch (Exception ex)
            {
                response.Result.ResponseData = false;
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue(ex.Message, currentUser.LanguageId);
            }
            return response;

        }

        private ISheet ValidateExcelHasEmailColumn(string fileUrl)
        {
            ISheet sheet;
            var req = WebRequest.Create(fileUrl);

            using (Stream stream = req.GetResponse().GetResponseStream())
            {
                XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
                sheet = xssWorkbook.GetSheetAt(0);
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                var emailHeaderCell = headerRow.FirstOrDefault(c => c.ToString() == "Email");

                if (cellCount == 0 || emailHeaderCell == null || emailHeaderCell.CellType == CellType.Blank)
                {
                    throw new Exception("InvalidExcellFile");
                }
            }

            return sheet;
        }

        private async Task<bool> ProcessExcelSheet(ISheet sheet, UserDto currentUser, InvitationFileDto file)
        {
            CompanyInvitationFile invitationFile = new();
            invitationFile.CreateInvitationFile(file.FileName, currentUser.CompanyId, currentUser.Id, InvitationFileStatus.New);
            _companyInvitationFileRepository.AddCompanyInvitationFile(invitationFile);
            await _companyInvitationFileRepository.UnitOfWork.SaveEntitiesAsync();

            IEnumerable<CompanyInvitationDto> invitations = GetEmailsFromExcel(sheet);
            foreach (var invitation in invitations)
            {
                if (!string.IsNullOrEmpty(invitation.Email))
                {
                    CompanyInvitation companyInvitation = new();
                    companyInvitation.CreateCompanyInvitations(currentUser.CompanyId, invitation.Email, currentUser.Id, Guid.NewGuid().ToString(),
                                                InvitationStatus.New.Id, invitation.JobTitle, invitation.Role, invitation.EmployeeType);

                    _companyInvitationRepository.AddCompanyInvitation(companyInvitation);
                }
            }

            // Update file status to be done
            _companyInvitationFileRepository.UpdateCompanyInvitationFileStatus(invitationFile.Id, InvitationFileStatus.Done);

            await _companyInvitationFileRepository.UnitOfWork.SaveEntitiesAsync();

            return true;
        }

        private IEnumerable<CompanyInvitationDto> GetEmailsFromExcel(ISheet sheet)
        {
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            int emailCellIndex, jobTitleCellIndex, roleCellIndex, empTypeCellIndex;

            // Read header cells
            var emailHeaderCell = headerRow.FirstOrDefault(c => c.ToString() == "Email");
            var jobTitleHeaderCell = headerRow.FirstOrDefault(c => c.ToString() == "Job Title");
            var roleHeaderCell = headerRow.FirstOrDefault(c => c.ToString() == "Role");
            var empTypeHeaderCell = headerRow.FirstOrDefault(c => c.ToString() == "Employee Type");

            emailCellIndex = emailHeaderCell != null ? emailHeaderCell.ColumnIndex : -1;
            jobTitleCellIndex = jobTitleHeaderCell != null ? jobTitleHeaderCell.ColumnIndex : -1;
            roleCellIndex = roleHeaderCell != null ? roleHeaderCell.ColumnIndex : -1;
            empTypeCellIndex = empTypeHeaderCell != null ? empTypeHeaderCell.ColumnIndex : -1;


            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                // Validate email cell has value
                if (row.GetCell(emailCellIndex) != null && !string.IsNullOrEmpty(row.GetCell(emailCellIndex).ToString()))
                {
                    yield return new CompanyInvitationDto()
                    {
                        Email = emailCellIndex > -1 ? row.GetCell(emailCellIndex)?.ToString() : null,
                        JobTitle = jobTitleCellIndex > -1 ? row.GetCell(jobTitleCellIndex)?.ToString() : null,
                        Role = roleCellIndex > -1 ? row.GetCell(roleCellIndex)?.ToString() : null,
                        EmployeeType = empTypeCellIndex > -1 ? row.GetCell(empTypeCellIndex)?.ToString() : null
                    };
                }
            }
        }
    }
}
