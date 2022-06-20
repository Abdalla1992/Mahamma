using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Domain.Meeting.Enum;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Task.Entity;
using Mahamma.Domain.Workspace.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class MeetingEntityTypeConfiguration : IEntityTypeConfiguration<Meeting>
    {
        public void Configure(EntityTypeBuilder<Meeting> meeting)
        {
            meeting.ToTable("Meetings");
            meeting.HasKey(a => a.Id);
            meeting.Property(a => a.Title).HasColumnName("Title").IsRequired();
            meeting.Property(a => a.Duration).HasColumnName("Duration");
            meeting.Property(a => a.DurationUnitType).HasColumnName("DurationUnitType");
            meeting.Property(a => a.Date).HasColumnName("Date").HasColumnType("datetime").IsRequired();
            meeting.Property(a => a.Time).HasColumnName("Time").HasColumnType("time").IsRequired();
            meeting.Property(a => a.CompanyId).HasColumnName("CompanyId").IsRequired();
            meeting.Property(a => a.ProjectId).HasColumnName("ProjectId");
            meeting.Property(a => a.WorkSpaceId).HasColumnName("WorkSpaceId");
            meeting.Property(a => a.TaskId).HasColumnName("TaskId");
            meeting.Property(a => a.CreatorUserId).HasColumnName("CreatorUserId");
            meeting.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            meeting.Property(a => a.DeletedStatus).IsRequired();
          

            meeting.HasOne<Company>().WithMany().IsRequired(true).HasForeignKey(c => c.CompanyId);
            meeting.HasOne<Workspace>().WithMany().IsRequired(false).HasForeignKey(c => c.WorkSpaceId);
            meeting.HasOne<Project>().WithMany().IsRequired(false).HasForeignKey(c => c.ProjectId);
            meeting.HasOne<Task>().WithMany().HasForeignKey(c => c.TaskId);

            meeting.OwnsMany<MeetingMember>("Members", m =>
            {
                m.ToTable("MeetingMembers");
                m.Property<int>("Id");
                m.HasKey("Id");
                m.Property(m => m.UserId).IsRequired();
                m.Property(m => m.Attended).HasColumnName("Attended");
                m.Property(m => m.InvitationAccepted).HasColumnName("InvitationAccepted");
                m.Property(m => m.CanMakeMinuteOfMeeting).HasColumnName("CanMakeMinuteOfMeeting");
                m.Property(m => m.CreationDate).HasColumnType("datetime").IsRequired();
                m.Property(m => m.DeletedStatus).IsRequired();

                m.OwnsMany<MeetingMemberRoles>("MeetingRoles", mr =>
                {
                    mr.ToTable("MeetingMemberRoles");
                    mr.Property<int>("Id");
                    mr.HasKey("Id");
                    mr.Property(m => m.UserId).IsRequired();
                    mr.Property(m => m.MeetingRoleId).HasColumnName("MeetingRoleId");
                    mr.Property(m => m.CreationDate).HasColumnType("datetime").IsRequired();
                    mr.Property(m => m.DeletedStatus).IsRequired();

                    mr.HasOne<MeetingRole>().WithMany().IsRequired(true).HasForeignKey(c => c.MeetingRoleId);
                });
            });

            meeting.OwnsMany<MeetingAgendaTopics>("AgendaTopics", m =>
            {
                m.ToTable("MeetingAgendaTopics");
                m.Property<int>("Id");
                m.HasKey("Id");
                m.Property(m => m.Topic).HasColumnName("Topic").IsRequired();
                m.Property(m => m.DurationInMinutes).HasColumnName("DurationInMinutes").IsRequired();
                m.Property(m => m.CreationDate).HasColumnType("datetime").IsRequired();
                m.Property(m => m.DeletedStatus).IsRequired();
            });

            meeting.OwnsMany<MinuteOfMeeting>("MinutesOfMeeting", m =>
            {
                m.ToTable("MinutesOfMeetings");
                m.Property<int>("Id");
                m.HasKey("Id");
                m.Property(m => m.Description).HasColumnName("Description");
                m.Property(m => m.IsDraft).HasColumnName("IsDraft").IsRequired();
                m.Property(m => m.CreatorUserId).HasColumnName("CreatorUserId").IsRequired();
                m.Property(m => m.CreationDate).HasColumnType("datetime").IsRequired();
                m.Property(m => m.DeletedStatus).IsRequired();

                m.Property(m => m.ProjectId).HasColumnName("ProjectId");
                m.Property(m => m.TaskId).HasColumnName("TaskId");

                m.HasOne<Project>().WithMany().IsRequired(false).HasForeignKey(c => c.ProjectId);
                m.HasOne<Task>().WithMany().IsRequired(false).HasForeignKey(c => c.TaskId);
            });
            meeting.OwnsMany<MeetingFile>("MeetingFiles", m =>
            {
                m.ToTable("MeetingFiles");
                m.Property<int>("Id");
                m.HasKey("Id");
                m.Property(m => m.URL).HasColumnName("URL").IsRequired();
                m.Property(m => m.Name).HasColumnName("Name").IsRequired();
                m.Property(m => m.CreationDate).HasColumnType("datetime").IsRequired();
                m.Property(m => m.DeletedStatus).IsRequired();
            });
            meeting.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
