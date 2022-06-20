using Mahamma.Identity.Domain.Language.Entity;
using Mahamma.Identity.Domain.Language.Enum;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.Role.Enum;
using Microsoft.EntityFrameworkCore;

namespace Mahamma.Identity.Infrastructure.Context
{
    public static class IdentityContextSeed
    {
        public static void BuildEnums(this ModelBuilder modelBuilder)
        {
            #region Permissions
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddWorkspace.Id, Permission.AddWorkspace.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.ViewWorkspace.Id, Permission.ViewWorkspace.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.UpdateWorkspace.Id, Permission.UpdateWorkspace.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.DeleteWorkspace.Id, Permission.DeleteWorkspace.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.ViewTask.Id, Permission.ViewTask.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddTask.Id, Permission.AddTask.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.UpdateTask.Id, Permission.UpdateTask.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.DeleteTask.Id, Permission.DeleteTask.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.ArchiveTask.Id, Permission.ArchiveTask.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddFile.Id, Permission.AddFile.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.ViewFile.Id, Permission.ViewFile.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.DeleteFile.Id, Permission.DeleteFile.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AssignMember.Id, Permission.AssignMember.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddComment.Id, Permission.AddComment.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddProject.Id, Permission.AddProject.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.ViewProject.Id, Permission.ViewProject.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.UpdateProject.Id, Permission.UpdateProject.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.DeleteProject.Id, Permission.DeleteProject.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.ArchiveProject.Id, Permission.ArchiveProject.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.ViewRole.Id, Permission.ViewRole.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddRole.Id, Permission.AddRole.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.UpdateRole.Id, Permission.UpdateRole.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.DeleteRole.Id, Permission.DeleteRole.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.UploadDocument.Id, Permission.UploadDocument.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.DownloadDocument.Id, Permission.DownloadDocument.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.DeleteDocument.Id, Permission.DeleteDocument.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.SubmitTask.Id, Permission.SubmitTask.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddSubTask.Id, Permission.AddSubTask.Name));
            //modelBuilder.Entity<Permission>().HasData(new Permission(Permission.ViewCharts.Id, Permission.ViewCharts.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddMeetingGeneral.Id, Permission.AddMeetingGeneral.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddMeetingWorkspace.Id, Permission.AddMeetingWorkspace.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddMeetingProject.Id, Permission.AddMeetingProject.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddMeetingTask.Id, Permission.AddMeetingTask.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.UpdateMeeting.Id, Permission.UpdateMeeting.Name));
            //modelBuilder.Entity<Permission>().HasData(new Permission(Permission.DeleteMeeting.Id, Permission.DeleteMeeting.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.ViewMeeting.Id, Permission.ViewMeeting.Name));
            //modelBuilder.Entity<Permission>().HasData(new Permission(Permission.UpdateMinuteOfMeeting.Id, Permission.UpdateMinuteOfMeeting.Name));
            //modelBuilder.Entity<Permission>().HasData(new Permission(Permission.DeleteMinuteOfMeeting.Id, Permission.DeleteMinuteOfMeeting.Name));
            //modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddMinuteOfMeeting.Id, Permission.AddMinuteOfMeeting.Name));
            //modelBuilder.Entity<Permission>().HasData(new Permission(Permission.ViewMinuteOfMeeting.Id, Permission.ViewMinuteOfMeeting.Name));
            modelBuilder.Entity<Permission>().HasData(new Permission(Permission.AddMeeting.Id, Permission.AddMeeting.Name));
            #endregion

            #region Pages
            modelBuilder.Entity<Page>().HasData(new Page(PageEnum.WorkspaceProfile.Id, PageEnum.WorkspaceProfile.Name));
            modelBuilder.Entity<Page>().HasData(new Page(PageEnum.ProjectProfile.Id, PageEnum.ProjectProfile.Name));
            modelBuilder.Entity<Page>().HasData(new Page(PageEnum.TaskProfile.Id, PageEnum.TaskProfile.Name));
            modelBuilder.Entity<Page>().HasData(new Page(PageEnum.SubtaskProfile.Id, PageEnum.SubtaskProfile.Name));
            modelBuilder.Entity<Page>().HasData(new Page(PageEnum.ManageRoles.Id, PageEnum.ManageRoles.Name));
            //modelBuilder.Entity<Page>().HasData(new Page(PageEnum.DashboardProfile.Id, PageEnum.DashboardProfile.Name));
            modelBuilder.Entity<Page>().HasData(new Page(PageEnum.MeetingProfile.Id, PageEnum.MeetingProfile.Name));
            modelBuilder.Entity<Page>().HasData(new Page(PageEnum.ManageMeetings.Id, PageEnum.ManageMeetings.Name));
            //modelBuilder.Entity<Page>().HasData(new Page(PageEnum.ManageMinutesOfMeetings.Id, PageEnum.ManageMinutesOfMeetings.Name));
            #endregion

            #region PagePermission
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(1, PageEnum.WorkspaceProfile.Id, Permission.AddWorkspace.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(2, PageEnum.WorkspaceProfile.Id, Permission.ViewWorkspace.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(3, PageEnum.WorkspaceProfile.Id, Permission.UpdateWorkspace.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(4, PageEnum.WorkspaceProfile.Id, Permission.DeleteWorkspace.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(5, PageEnum.ProjectProfile.Id, Permission.AddProject.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(6, PageEnum.ProjectProfile.Id, Permission.ViewProject.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(7, PageEnum.ProjectProfile.Id, Permission.UpdateProject.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(8, PageEnum.ProjectProfile.Id, Permission.DeleteProject.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(9, PageEnum.ProjectProfile.Id, Permission.ArchiveProject.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(10, PageEnum.TaskProfile.Id, Permission.AddTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(11, PageEnum.TaskProfile.Id, Permission.ViewTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(12, PageEnum.TaskProfile.Id, Permission.UpdateTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(13, PageEnum.TaskProfile.Id, Permission.DeleteTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(14, PageEnum.TaskProfile.Id, Permission.ArchiveTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(15, PageEnum.ProjectProfile.Id, Permission.AddFile.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(16, PageEnum.ProjectProfile.Id, Permission.DeleteFile.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(17, PageEnum.ProjectProfile.Id, Permission.ViewFile.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(18, PageEnum.ProjectProfile.Id, Permission.AssignMember.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(19, PageEnum.TaskProfile.Id, Permission.AddFile.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(20, PageEnum.TaskProfile.Id, Permission.DeleteFile.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(21, PageEnum.TaskProfile.Id, Permission.ViewFile.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(22, PageEnum.TaskProfile.Id, Permission.AssignMember.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(23, PageEnum.TaskProfile.Id, Permission.AddComment.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(24, PageEnum.SubtaskProfile.Id, Permission.AddTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(25, PageEnum.SubtaskProfile.Id, Permission.ViewTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(26, PageEnum.SubtaskProfile.Id, Permission.UpdateTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(27, PageEnum.SubtaskProfile.Id, Permission.DeleteTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(28, PageEnum.SubtaskProfile.Id, Permission.ArchiveTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(29, PageEnum.SubtaskProfile.Id, Permission.AddFile.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(30, PageEnum.SubtaskProfile.Id, Permission.DeleteFile.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(31, PageEnum.SubtaskProfile.Id, Permission.ViewFile.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(32, PageEnum.SubtaskProfile.Id, Permission.AssignMember.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(33, PageEnum.SubtaskProfile.Id, Permission.AddComment.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(34, PageEnum.ManageRoles.Id, Permission.AddRole.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(35, PageEnum.ManageRoles.Id, Permission.ViewRole.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(36, PageEnum.ManageRoles.Id, Permission.UpdateRole.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(37, PageEnum.ManageRoles.Id, Permission.DeleteRole.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(38, PageEnum.WorkspaceProfile.Id, Permission.AssignMember.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(39, PageEnum.WorkspaceProfile.Id, Permission.AddProject.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(40, PageEnum.ProjectProfile.Id, Permission.AddTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(41, PageEnum.WorkspaceProfile.Id, Permission.DeleteProject.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(42, PageEnum.WorkspaceProfile.Id, Permission.ArchiveProject.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(43, PageEnum.ProjectProfile.Id, Permission.DeleteTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(44, PageEnum.ProjectProfile.Id, Permission.ArchiveTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(45, PageEnum.WorkspaceProfile.Id, Permission.ViewProject.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(46, PageEnum.ProjectProfile.Id, Permission.ViewTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(47, PageEnum.TaskProfile.Id, Permission.AddSubTask.Id));
            //modelBuilder.Entity<PagePermission>().HasData(new PagePermission(48, PageEnum.DashboardProfile.Id, Permission.ViewCharts.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(49, PageEnum.ManageMeetings.Id, Permission.AddMeetingGeneral.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(50, PageEnum.ManageMeetings.Id, Permission.AddMeetingWorkspace.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(51, PageEnum.ManageMeetings.Id, Permission.AddMeetingProject.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(52, PageEnum.ManageMeetings.Id, Permission.AddMeetingTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(53, PageEnum.ManageMeetings.Id, Permission.UpdateMeeting.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(54, PageEnum.ManageMeetings.Id, Permission.ViewMeeting.Id));
            //modelBuilder.Entity<PagePermission>().HasData(new PagePermission(55, PageEnum.ManageMeetings.Id, Permission.DeleteMeeting.Id));
            //modelBuilder.Entity<PagePermission>().HasData(new PagePermission(56, PageEnum.MeetingProfile.Id, Permission.ViewMeeting.Id));
            //modelBuilder.Entity<PagePermission>().HasData(new PagePermission(57, PageEnum.MeetingProfile.Id, Permission.UpdateMinuteOfMeeting.Id));
            //modelBuilder.Entity<PagePermission>().HasData(new PagePermission(58, PageEnum.MeetingProfile.Id, Permission.DeleteMinuteOfMeeting.Id));
            //modelBuilder.Entity<PagePermission>().HasData(new PagePermission(59, PageEnum.ManageMinutesOfMeetings.Id, Permission.ViewMinuteOfMeeting.Id));
            //modelBuilder.Entity<PagePermission>().HasData(new PagePermission(60, PageEnum.ManageMinutesOfMeetings.Id, Permission.AddMinuteOfMeeting.Id));
            //modelBuilder.Entity<PagePermission>().HasData(new PagePermission(61, PageEnum.ManageMinutesOfMeetings.Id, Permission.UpdateMinuteOfMeeting.Id));
            //modelBuilder.Entity<PagePermission>().HasData(new PagePermission(62, PageEnum.ManageMinutesOfMeetings.Id, Permission.DeleteMinuteOfMeeting.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(63, PageEnum.ManageMeetings.Id, Permission.AddMeeting.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(64, PageEnum.TaskProfile.Id, Permission.SubmitTask.Id));
            modelBuilder.Entity<PagePermission>().HasData(new PagePermission(65, PageEnum.SubtaskProfile.Id, Permission.SubmitTask.Id));


            //ManageMeeting=> addlevles  update view delet
            //meetprofile ==> viw  updatemom deletemom
            //mangeMom  ==> add update delete view 



            #endregion

            #region Language
            modelBuilder.Entity<Language>().HasData(new Language(LanguageEnum.English.Id, LanguageEnum.English.Name, "En",false));
            modelBuilder.Entity<Language>().HasData(new Language(LanguageEnum.Arabic.Id, LanguageEnum.Arabic.Name, "Ar",true));
            #endregion

            #region PermissionLocalizaion
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(1, "Add WorkSpace", Permission.AddWorkspace.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(2, "اضافة مساحة عمل", Permission.AddWorkspace.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(3, "View Workspace", Permission.ViewWorkspace.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(4, "مشاهدة  مساحة العمل", Permission.ViewWorkspace.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(5, "Update Workspace", Permission.UpdateWorkspace.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(6, "تعديل مساحة العمل", Permission.UpdateWorkspace.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(7, "Delete Workspace", Permission.DeleteWorkspace.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(8, "مسح مساحة العمل", Permission.DeleteWorkspace.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(9, "View Task", Permission.ViewTask.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(10, "مشاهدة المهمة", Permission.ViewTask.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(11, "Add Task", Permission.AddTask.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(12, "اضافة مهمة", Permission.AddTask.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(13, "Update Task", Permission.UpdateTask.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(14, "تعديل المهمة", Permission.UpdateTask.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(15, "Delete Task", Permission.DeleteTask.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(16, "مسح المهمة", Permission.DeleteTask.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(17, "Archive Task", Permission.ArchiveTask.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(18, "ارشيف المهمة", Permission.ArchiveTask.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(19, "Add File", Permission.AddFile.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(20, "اضافة ملفات", Permission.AddFile.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(21, "View File", Permission.ViewFile.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(22, "مشاهدة ملف", Permission.ViewFile.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(23, "Delete File", Permission.DeleteFile.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(24, "مسح ملف", Permission.DeleteFile.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(25, "Assign Member", Permission.AssignMember.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(26, "اضافة عضو", Permission.AssignMember.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(27, "Add Comment", Permission.AddComment.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(28, "اضافة كومنت", Permission.AddComment.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(29, "Add Project", Permission.AddProject.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(30, "اضافة مشروع", Permission.AddProject.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(31, "View Project", Permission.ViewProject.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(32, "مشاهدة مشروع", Permission.ViewProject.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(33, "Update Project", Permission.UpdateProject.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(34, "تعديل مشروع", Permission.UpdateProject.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(35, "Delete Project", Permission.DeleteProject.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(36, "مسح مشروع", Permission.DeleteProject.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(37, "Archive Project", Permission.ArchiveProject.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(38, "ارشيف المشروع", Permission.ArchiveProject.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(39, "View Role", Permission.ViewRole.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(40, "مشاهدة دور", Permission.ViewRole.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(41, "Add Role", Permission.AddRole.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(42, "اضافة دور", Permission.AddRole.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(43, "Update Role", Permission.UpdateRole.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(44, "تعديل دور", Permission.UpdateRole.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(45, "Delete Role", Permission.DeleteRole.Id, LanguageEnum.English.Id));
            //modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(46, "مسح دور", Permission.DeleteRole.Id, LanguageEnum.Arabic.Id));
            //modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(47, "View Dashboard", Permission.ViewCharts.Id, LanguageEnum.English.Id));
            //modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(48, "عرض لوحة القيادة", Permission.ViewCharts.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(49, "Add Meeting General", Permission.AddMeetingGeneral.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(50, "اضافة اجتماع على العموم", Permission.AddMeetingGeneral.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(51, "Add Meeting Workspace", Permission.AddMeetingWorkspace.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(52, "اضافة اجتماع على مستوى مساحة العمل", Permission.AddMeetingWorkspace.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(53, "Add Meeting Project", Permission.AddMeetingProject.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(54, "اضافة اجتماع على مستوى المشروع", Permission.AddMeetingProject.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(55, "Add Meeting Task", Permission.AddMeetingTask.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(56, "اضافة اجتماع على مستوى المهمة", Permission.AddMeetingTask.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(57, "Update Meeting", Permission.UpdateMeeting.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(58, "تعديل الاجتماع", Permission.UpdateMeeting.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(59, "Delete Meeting", Permission.DeleteMeeting.Id, LanguageEnum.English.Id));
            //modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(60, "مسح الاجتماع", Permission.DeleteMeeting.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(61, "View Meeting", Permission.ViewMeeting.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(62, "مشاهدة الاجتماع", Permission.ViewMeeting.Id, LanguageEnum.Arabic.Id));
            //modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(63, "Update Minute Of Meeting", Permission.UpdateMinuteOfMeeting.Id, LanguageEnum.English.Id));
            //modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(64, "تعديل النقاط الخاصة بالاجتماع", Permission.UpdateMinuteOfMeeting.Id, LanguageEnum.Arabic.Id));
            //modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(65, "Delete Minute Of Meeting", Permission.DeleteMinuteOfMeeting.Id, LanguageEnum.English.Id));
            //modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(66, "مسح النقاط الخاصة بالاجتماع", Permission.DeleteMinuteOfMeeting.Id, LanguageEnum.Arabic.Id));
            //modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(67, "Add Minute Of Meeting", Permission.AddMinuteOfMeeting.Id, LanguageEnum.English.Id));
            //modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(68, "اضافة نقاط الخاصة بالاجتماع", Permission.AddMinuteOfMeeting.Id, LanguageEnum.Arabic.Id));
            //modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(69, "View Minute Of Meeting", Permission.ViewMinuteOfMeeting.Id, LanguageEnum.English.Id));
            //modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(70, "مشاهدة النقاط الخاصة بالاجتماع", Permission.ViewMinuteOfMeeting.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(71, "Add Meeting", Permission.AddMeeting.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(72, "اضافة اجتماع", Permission.AddMeeting.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(73, "Add Sub Task", Permission.AddSubTask.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(74, "اضافة مهمة فرعية", Permission.AddSubTask.Id, LanguageEnum.Arabic.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(75, "Submit Task", Permission.SubmitTask.Id, LanguageEnum.English.Id));
            modelBuilder.Entity<PermissionLocalization>().HasData(new PermissionLocalization(76, "إرسال المهمة", Permission.SubmitTask.Id, LanguageEnum.Arabic.Id));
            #endregion

            #region PageLocalization
            modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(1, "Workspace Profile", LanguageEnum.English.Id,PageEnum.WorkspaceProfile.Id));
            modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(2, "الصفحة الخاصة بمساحة العمل", LanguageEnum.Arabic.Id, PageEnum.WorkspaceProfile.Id));
            modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(3, "Project Profile", LanguageEnum.English.Id, PageEnum.ProjectProfile.Id));
            modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(4, "الصفحة الخاصة بالمشروع", LanguageEnum.Arabic.Id, PageEnum.ProjectProfile.Id));
            modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(5, "Task Profile", LanguageEnum.English.Id, PageEnum.TaskProfile.Id));
            modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(6, "الصفحة الخاصة بالمهمة", LanguageEnum.Arabic.Id, PageEnum.TaskProfile.Id));
            modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(7, "Subtask Profile", LanguageEnum.English.Id, PageEnum.SubtaskProfile.Id));
            modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(8, "الصفحة الخاصة بالمهمة الفرعية", LanguageEnum.Arabic.Id, PageEnum.SubtaskProfile.Id));
            modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(9, "Manage Roles", LanguageEnum.English.Id, PageEnum.ManageRoles.Id));
            //modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(10, "تنظيم الادوار", LanguageEnum.Arabic.Id, PageEnum.ManageRoles.Id));
            //modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(11, "Dashboard", LanguageEnum.Arabic.Id, PageEnum.DashboardProfile.Id));
            //modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(12, "لوحة القيادة", LanguageEnum.Arabic.Id, PageEnum.DashboardProfile.Id));
            modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(13, "Meeting Profile", LanguageEnum.English.Id, PageEnum.MeetingProfile.Id));
            modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(14, "الصفحة الخاصة بالاجتماعات", LanguageEnum.Arabic.Id, PageEnum.MeetingProfile.Id));
            modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(15, "Manage Meetings", LanguageEnum.English.Id, PageEnum.ManageMeetings.Id));
            //modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(16, "تنظيم الاجتماعات", LanguageEnum.Arabic.Id, PageEnum.ManageMeetings.Id));
            //modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(17, "Manage Minutes Of Meetings", LanguageEnum.English.Id, PageEnum.ManageMinutesOfMeetings.Id));
            //modelBuilder.Entity<PageLocalization>().HasData(new PageLocalization(18, "تنظيم النقاط الخاصة بالاجتماع", LanguageEnum.Arabic.Id, PageEnum.ManageMinutesOfMeetings.Id));
            #endregion
        }
    }
}
