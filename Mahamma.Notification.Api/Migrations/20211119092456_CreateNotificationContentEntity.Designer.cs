// <auto-generated />
using System;
using Mahamma.Notification.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mahamma.Notification.Api.Migrations
{
    [DbContext(typeof(NotificationContext))]
    [Migration("20211119092456_CreateNotificationContentEntity")]
    partial class CreateNotificationContentEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mahamma.Notification.Domain.Notification.Entity.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<bool>("IsRead")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("IsRead");

                    b.Property<int>("NotificationSendingStatusId")
                        .HasColumnType("int")
                        .HasColumnName("NotificationSendingStatusId");

                    b.Property<int>("NotificationSendingTypeId")
                        .HasColumnType("int")
                        .HasColumnName("NotificationSendingTypeId");

                    b.Property<int>("NotificationTypeId")
                        .HasColumnType("int")
                        .HasColumnName("NotificationTypeId");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("ProjectId");

                    b.Property<long>("ReceiverUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("ReceiverUserId");

                    b.Property<long>("SenderUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("SenderUserId");

                    b.Property<int?>("TaskId")
                        .HasColumnType("int")
                        .HasColumnName("TaskId");

                    b.Property<int?>("WorkSpaceId")
                        .HasColumnType("int")
                        .HasColumnName("WorkSpaceId");

                    b.HasKey("Id");

                    b.HasIndex("NotificationSendingStatusId");

                    b.HasIndex("NotificationSendingTypeId");

                    b.HasIndex("NotificationTypeId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Mahamma.Notification.Domain.Notification.Entity.NotificationContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Body");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("DeletedStatus")
                        .HasColumnType("int");

                    b.Property<int>("LanguageId")
                        .HasColumnType("int")
                        .HasColumnName("LanguageId");

                    b.Property<int>("NotificationId")
                        .HasColumnType("int")
                        .HasColumnName("NotificationId");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Title");

                    b.HasKey("Id");

                    b.HasIndex("NotificationId");

                    b.ToTable("NotificationContent");
                });

            modelBuilder.Entity("Mahamma.Notification.Domain.Notification.Enum.NotificationSendingStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NotificationSendingStatus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "New"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Send"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Faild"
                        });
                });

            modelBuilder.Entity("Mahamma.Notification.Domain.Notification.Enum.NotificationSendingType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NotificationSendingType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Email"
                        },
                        new
                        {
                            Id = 2,
                            Name = "PushNotification"
                        },
                        new
                        {
                            Id = 3,
                            Name = "All"
                        });
                });

            modelBuilder.Entity("Mahamma.Notification.Domain.Notification.Enum.NotificationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NotificationType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "AddProject"
                        },
                        new
                        {
                            Id = 2,
                            Name = "ArchiveProject"
                        },
                        new
                        {
                            Id = 3,
                            Name = "AssignMemberToProject"
                        },
                        new
                        {
                            Id = 4,
                            Name = "DeleteProject"
                        },
                        new
                        {
                            Id = 5,
                            Name = "UpdateProject"
                        },
                        new
                        {
                            Id = 6,
                            Name = "AddComment"
                        },
                        new
                        {
                            Id = 7,
                            Name = "AddTask"
                        },
                        new
                        {
                            Id = 8,
                            Name = "ArchiveTask"
                        },
                        new
                        {
                            Id = 9,
                            Name = "AssignMemberToTask"
                        },
                        new
                        {
                            Id = 10,
                            Name = "DeleteTask"
                        },
                        new
                        {
                            Id = 11,
                            Name = "LikeComment"
                        },
                        new
                        {
                            Id = 12,
                            Name = "SubmitTask"
                        },
                        new
                        {
                            Id = 13,
                            Name = "UpdateTask"
                        },
                        new
                        {
                            Id = 14,
                            Name = "AssignMemberToWorkspace"
                        });
                });

            modelBuilder.Entity("Mahamma.Notification.Domain.Notification.Entity.Notification", b =>
                {
                    b.HasOne("Mahamma.Notification.Domain.Notification.Enum.NotificationSendingStatus", null)
                        .WithMany()
                        .HasForeignKey("NotificationSendingStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mahamma.Notification.Domain.Notification.Enum.NotificationSendingType", null)
                        .WithMany()
                        .HasForeignKey("NotificationSendingTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mahamma.Notification.Domain.Notification.Enum.NotificationType", null)
                        .WithMany()
                        .HasForeignKey("NotificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mahamma.Notification.Domain.Notification.Entity.NotificationContent", b =>
                {
                    b.HasOne("Mahamma.Notification.Domain.Notification.Entity.Notification", null)
                        .WithMany("notificationContents")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mahamma.Notification.Domain.Notification.Entity.Notification", b =>
                {
                    b.Navigation("notificationContents");
                });
#pragma warning restore 612, 618
        }
    }
}
