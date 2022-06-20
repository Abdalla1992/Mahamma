using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Domain.MyWork.Entity;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.ProjectActivity.Entity;
using Mahamma.Domain.ProjectAttachment.Entity;
using Mahamma.Domain.Workspace.Entity;
using Mahamma.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Context
{
    public class MahammaContext : DbContext, IUnitOfWork
    {
        #region DBSet
        public DbSet<Company> Company { get; set; }
        public DbSet<CompanyInvitation> CompanyInvitations { get; set; }
        public DbSet<CompanyInvitationFile> CompanyInvitationFile { get; set; }
        public DbSet<Workspace> Workspace { get; set; }
        public DbSet<Domain.Task.Entity.Task> Task { get; set; }
        public DbSet<Domain.TaskActivity.Entity.TaskActivity> TaskActivity { get; set; }
        public DbSet<Domain.Task.Entity.TaskMember> TaskMember { get; set; }
        public DbSet<Domain.Task.Entity.TaskComment> TaskComment { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectMember> ProjectMember { get; set; }
        public DbSet<WorkspaceMember> WorkSpaceMember { get; set; }
        public DbSet<ProjectActivity> ProjectActivity { get; set; }
        public DbSet<Meeting> Meeting { get; set; }
        public DbSet<MeetingMember> MeetingMembers { get; set; }
        public DbSet<MeetingMemberRoles> MeetingMembersRoles { get; set; }
        public DbSet<MeetingAgendaTopics> MeetingAgendaTopics { get; set; }
        public DbSet<MinuteOfMeeting> MinutesOfMeeting { get; set; }
        public DbSet<MeetingFile> MeetingFiles { get; set; }
        //public DbSet<LikeComment> LikeComment { get; set; }
        public DbSet<ProjectComment> ProjectComment { get; set; }
        public DbSet<ProjectLikeComment> ProjectLikeComment { get; set; }
        public DbSet<ProjectCharter> ProjectCharter { get; set; }
        public DbSet<ProjectRiskPlan> ProjectRiskPlan { get; set; }
        public DbSet<ProjectCommunicationPlan> ProjectCommunicationPlan { get; set; }
        public DbSet<ProjectAttachment> ProjectAttachment { get; set; }
        public DbSet<Folder> Folder { get; set; }
        public DbSet<FolderFile> FolderFile { get; set; }
        public DbSet<Note> Note { get; set; }
        #endregion

        #region CTRS
        public MahammaContext(DbContextOptions<MahammaContext> options) : base(options)
        {
        }
        #endregion

        #region Model Creation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WorkspaceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TaskEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TaskMemberEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TaskActivityEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TaskCommentsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjecMembertEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new WorkspaceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectAttachmentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectActivityEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyInvitationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyInvitationFileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MeetingEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new LikeCommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectCommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectLikeCommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectCharterEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectRiskPlanEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectCommunicationPlanEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FolderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FolderFileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NoteEntityTypeConfiguration());
            modelBuilder.BuildEnums();
        }
        #endregion

        #region Save Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync(cancellationToken) > default(int);
            return result;
        }
        #endregion

    }
}
