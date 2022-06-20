﻿// <auto-generated />
using System;
using Mahamma.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mahamma.Api.Migrations
{
    [DbContext(typeof(MahammaContext))]
    [Migration("20211213003727_AddMinuteOfMeeting")]
    partial class AddMinuteOfMeeting
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mahamma.Domain.Company.Entity.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompanySize")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CompanySize");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<long>("CreatorUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatorUserId");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<string>("Descreption")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FolderPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("Mahamma.Domain.Company.Entity.CompanyInvitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int")
                        .HasColumnName("CompanyId");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Email");

                    b.Property<string>("InvitationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("InvitationId");

                    b.Property<int>("InvitationStatusId")
                        .HasColumnType("int")
                        .HasColumnName("InvitationStatusId");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("CompanyInvitation");
                });

            modelBuilder.Entity("Mahamma.Domain.Meeting.Entity.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("CreatorUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<int>("DurationInMinutes")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int?>("TaskId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WorkSpaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Meeting");
                });

            modelBuilder.Entity("Mahamma.Domain.Meeting.Entity.MeetingAgendaTopics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<int>("DurationInMinutes")
                        .HasColumnType("int");

                    b.Property<int>("MeetingId")
                        .HasColumnType("int");

                    b.Property<string>("Topic")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.ToTable("MeetingAgendaTopics");
                });

            modelBuilder.Entity("Mahamma.Domain.Meeting.Entity.MeetingMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Attended")
                        .HasColumnType("bit");

                    b.Property<bool?>("CanMakeMinuteOfMeeting")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<bool?>("InvitationAccepted")
                        .HasColumnType("bit");

                    b.Property<int>("MeetingId")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.ToTable("MeetingMember");
                });

            modelBuilder.Entity("Mahamma.Domain.Meeting.Entity.MinuteOfMeeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("CreatorUserId")
                        .HasColumnType("bigint");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDraft")
                        .HasColumnType("bit");

                    b.Property<int>("MeetingId")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int?>("TaskId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.ToTable("MinuteOfMeeting");
                });

            modelBuilder.Entity("Mahamma.Domain.Project.Entity.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<long>("CreatorUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatorUserId");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Description");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime")
                        .HasColumnName("DueDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<int>("WorkSpaceId")
                        .HasColumnType("int")
                        .HasColumnName("WorkSpaceId");

                    b.HasKey("Id");

                    b.HasIndex("WorkSpaceId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Mahamma.Domain.Project.Entity.ProjectMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("ProjectId");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectMember");
                });

            modelBuilder.Entity("Mahamma.Domain.ProjectActivity.Entity.ProjectActivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Action");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("ProjectId");

                    b.Property<int>("ProjectMemberId")
                        .HasColumnType("int")
                        .HasColumnName("ProjectMemberId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectActivity");
                });

            modelBuilder.Entity("Mahamma.Domain.ProjectAttachment.Entity.ProjectAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("FileUrl");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("ProjectId");

                    b.Property<int?>("TaskId")
                        .HasColumnType("int")
                        .HasColumnName("TaskId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TaskId");

                    b.ToTable("ProjectAttachment");
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Entity.LikeComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<int?>("TaskCommentId")
                        .HasColumnType("int");

                    b.Property<int?>("TaskMemberId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TaskCommentId");

                    b.HasIndex("TaskMemberId");

                    b.ToTable("LikeComment");
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Entity.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<long>("CreatorUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatorUserId");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Description");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime")
                        .HasColumnName("DueDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<int?>("ParentTaskId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("ProjectId");

                    b.Property<double?>("Rating")
                        .HasColumnType("float");

                    b.Property<bool>("ReviewRequest")
                        .HasColumnType("bit")
                        .HasColumnName("ReviewRequest");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime")
                        .HasColumnName("StartDate");

                    b.Property<int>("TaskPriorityId")
                        .HasColumnType("int")
                        .HasColumnName("TaskPriorityId");

                    b.Property<int>("TaskStatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentTaskId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TaskPriorityId");

                    b.HasIndex("TaskStatusId");

                    b.ToTable("Task");
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Entity.TaskComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Comment");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentCommentId")
                        .HasColumnType("int");

                    b.Property<int?>("TaskId")
                        .HasColumnType("int")
                        .HasColumnName("TaskId");

                    b.Property<int>("TaskMemberId")
                        .HasColumnType("int")
                        .HasColumnName("TaskMemberId");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("TaskId");

                    b.HasIndex("TaskMemberId");

                    b.ToTable("TaskComment");
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Entity.TaskMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<double?>("Rating")
                        .HasColumnType("float")
                        .HasColumnName("Rating");

                    b.Property<int>("TaskId")
                        .HasColumnType("int")
                        .HasColumnName("TaskId");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskMember");
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Enum.TaskPriority", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TaskPriority");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Normal"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Major"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Urgent"
                        });
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Enum.TaskStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TaskStatus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "New"
                        },
                        new
                        {
                            Id = 2,
                            Name = "In Progress"
                        },
                        new
                        {
                            Id = 3,
                            Name = "In Progress With Delay"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Completed On Time"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Completed Early"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Completed Late"
                        });
                });

            modelBuilder.Entity("Mahamma.Domain.TaskActivity.Entity.TaskActivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Action");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<int>("TaskId")
                        .HasColumnType("int")
                        .HasColumnName("TaskId");

                    b.Property<int>("TaskMemberId")
                        .HasColumnType("int")
                        .HasColumnName("TaskMemberId");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskActivity");
                });

            modelBuilder.Entity("Mahamma.Domain.Workspace.Entity.Workspace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Color");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int")
                        .HasColumnName("CompanyId");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<long>("CreatorUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatorUserId");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ImageUrl");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Workspace");
                });

            modelBuilder.Entity("Mahamma.Domain.Workspace.Entity.WorkspaceMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("WorkSpaceMember");
                });

            modelBuilder.Entity("Mahamma.Domain.Company.Entity.CompanyInvitation", b =>
                {
                    b.HasOne("Mahamma.Domain.Company.Entity.Company", null)
                        .WithMany("CompanyInvitations")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mahamma.Domain.Meeting.Entity.MeetingAgendaTopics", b =>
                {
                    b.HasOne("Mahamma.Domain.Meeting.Entity.Meeting", null)
                        .WithMany("AgendaTopics")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mahamma.Domain.Meeting.Entity.MeetingMember", b =>
                {
                    b.HasOne("Mahamma.Domain.Meeting.Entity.Meeting", null)
                        .WithMany("Members")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mahamma.Domain.Meeting.Entity.MinuteOfMeeting", b =>
                {
                    b.HasOne("Mahamma.Domain.Meeting.Entity.Meeting", null)
                        .WithMany("MinuteOfMeeting")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mahamma.Domain.Project.Entity.Project", b =>
                {
                    b.HasOne("Mahamma.Domain.Workspace.Entity.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkSpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("Mahamma.Domain.Project.Entity.ProjectMember", b =>
                {
                    b.HasOne("Mahamma.Domain.Project.Entity.Project", null)
                        .WithMany("ProjectMembers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mahamma.Domain.ProjectActivity.Entity.ProjectActivity", b =>
                {
                    b.HasOne("Mahamma.Domain.Project.Entity.Project", null)
                        .WithMany("ProjectActivities")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mahamma.Domain.ProjectAttachment.Entity.ProjectAttachment", b =>
                {
                    b.HasOne("Mahamma.Domain.Project.Entity.Project", null)
                        .WithMany("ProjectAttachments")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mahamma.Domain.Task.Entity.Task", null)
                        .WithMany("Attachments")
                        .HasForeignKey("TaskId");
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Entity.LikeComment", b =>
                {
                    b.HasOne("Mahamma.Domain.Task.Entity.TaskComment", "TaskComment")
                        .WithMany("Likes")
                        .HasForeignKey("TaskCommentId");

                    b.HasOne("Mahamma.Domain.Task.Entity.TaskMember", "TaskMember")
                        .WithMany("LikedComments")
                        .HasForeignKey("TaskMemberId");

                    b.Navigation("TaskComment");

                    b.Navigation("TaskMember");
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Entity.Task", b =>
                {
                    b.HasOne("Mahamma.Domain.Task.Entity.Task", null)
                        .WithMany("SubTask")
                        .HasForeignKey("ParentTaskId");

                    b.HasOne("Mahamma.Domain.Project.Entity.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mahamma.Domain.Task.Enum.TaskPriority", null)
                        .WithMany()
                        .HasForeignKey("TaskPriorityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mahamma.Domain.Task.Enum.TaskStatus", null)
                        .WithMany()
                        .HasForeignKey("TaskStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Entity.TaskComment", b =>
                {
                    b.HasOne("Mahamma.Domain.Task.Entity.TaskComment", null)
                        .WithMany("Replies")
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("Mahamma.Domain.Task.Entity.Task", null)
                        .WithMany("TaskComments")
                        .HasForeignKey("TaskId");

                    b.HasOne("Mahamma.Domain.Task.Entity.TaskMember", null)
                        .WithMany("TaskComments")
                        .HasForeignKey("TaskMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Entity.TaskMember", b =>
                {
                    b.HasOne("Mahamma.Domain.Task.Entity.Task", "Task")
                        .WithMany("TaskMembers")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Mahamma.Domain.TaskActivity.Entity.TaskActivity", b =>
                {
                    b.HasOne("Mahamma.Domain.Task.Entity.Task", null)
                        .WithMany("TaskActivities")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mahamma.Domain.Workspace.Entity.Workspace", b =>
                {
                    b.HasOne("Mahamma.Domain.Company.Entity.Company", null)
                        .WithMany("Workspaces")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mahamma.Domain.Workspace.Entity.WorkspaceMember", b =>
                {
                    b.HasOne("Mahamma.Domain.Workspace.Entity.Workspace", null)
                        .WithMany("WorkspaceMembers")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mahamma.Domain.Company.Entity.Company", b =>
                {
                    b.Navigation("CompanyInvitations");

                    b.Navigation("Workspaces");
                });

            modelBuilder.Entity("Mahamma.Domain.Meeting.Entity.Meeting", b =>
                {
                    b.Navigation("AgendaTopics");

                    b.Navigation("Members");

                    b.Navigation("MinuteOfMeeting");
                });

            modelBuilder.Entity("Mahamma.Domain.Project.Entity.Project", b =>
                {
                    b.Navigation("ProjectActivities");

                    b.Navigation("ProjectAttachments");

                    b.Navigation("ProjectMembers");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Entity.Task", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("SubTask");

                    b.Navigation("TaskActivities");

                    b.Navigation("TaskComments");

                    b.Navigation("TaskMembers");
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Entity.TaskComment", b =>
                {
                    b.Navigation("Likes");

                    b.Navigation("Replies");
                });

            modelBuilder.Entity("Mahamma.Domain.Task.Entity.TaskMember", b =>
                {
                    b.Navigation("LikedComments");

                    b.Navigation("TaskComments");
                });

            modelBuilder.Entity("Mahamma.Domain.Workspace.Entity.Workspace", b =>
                {
                    b.Navigation("WorkspaceMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
