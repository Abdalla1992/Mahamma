using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Identity.Api.Migrations
{
    public partial class CreateIdentityDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRtl = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Page",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkingDays = table.Column<int>(type: "int", nullable: false),
                    WorkingHours = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    UserProfileStatusId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageLocalization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DispalyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageLocalization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageLocalization_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PagePermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagePermission_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PagePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionLocalization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionLocalization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionLocalization_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionLocalization_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePagePermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    PagePermissionId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePagePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePagePermission_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePagePermission_PagePermission_PagePermissionId",
                        column: x => x.PagePermissionId,
                        principalTable: "PagePermission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "Id", "Alias", "CreationDate", "DeletedStatus", "IsRtl", "Name" },
                values: new object[,]
                {
                    { 1, "En", new DateTime(2021, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, "English" },
                    { 2, "Ar", new DateTime(2021, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, "Arabic" }
                });

            migrationBuilder.InsertData(
                table: "Page",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "WorkspaceProfile" },
                    { 2, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "ProjectProfile" },
                    { 3, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "TaskProfile" },
                    { 4, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "SubtaskProfile" },
                    { 5, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "ManageRoles" }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 16, "ViewProject" },
                    { 17, "UpdateProject" },
                    { 18, "DeleteProject" },
                    { 19, "ArchiveProject" },
                    { 20, "ViewRole" },
                    { 24, "UploadDocument" },
                    { 22, "UpdateRole" },
                    { 23, "DeleteRole" },
                    { 15, "AddProject" },
                    { 25, "DownloadDocument" },
                    { 26, "DeleteDocument" },
                    { 21, "AddRole" },
                    { 14, "AddComment" },
                    { 11, "ViewFile" },
                    { 12, "DeleteFile" },
                    { 27, "SubmitTask" },
                    { 10, "AddFile" },
                    { 9, "ArchiveTask" },
                    { 8, "DeleteTask" },
                    { 7, "UpdateTask" },
                    { 6, "AddTask" },
                    { 5, "ViewTask" },
                    { 4, "DeleteWorkspace" },
                    { 3, "UpdateWorkspace" },
                    { 2, "ViewWorkspace" },
                    { 1, "AddWorkspace" },
                    { 13, "AssignMember" },
                    { 28, "AddSubTask" }
                });

            migrationBuilder.InsertData(
                table: "PageLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DispalyName", "LanguageId" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Workspace Profile", 1 },
                    { 3, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Project Profile", 1 },
                    { 5, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Task Profile", 1 },
                    { 7, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Subtask Profile", 1 },
                    { 9, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manage Roles", 1 },
                    { 2, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "الصفحة الخاصة بمساحة العمل", 2 },
                    { 4, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "الصفحة الخاصة بالمشروع", 2 },
                    { 6, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "الصفحة الخاصة بالمهمة", 2 },
                    { 8, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "الصفحة الخاصة بالمهمة الفرعية", 2 },
                    { 10, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "تنظيم الادوار", 2 }
                });

            migrationBuilder.InsertData(
                table: "PagePermission",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "PageId", "PermissionId" },
                values: new object[,]
                {
                    { 32, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 13 },
                    { 22, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 13 },
                    { 18, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 13 },
                    { 30, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 12 },
                    { 20, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 12 },
                    { 16, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 12 },
                    { 15, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 10 },
                    { 21, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 11 },
                    { 29, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 10 },
                    { 19, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 10 },
                    { 38, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 13 },
                    { 44, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 9 },
                    { 31, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 11 },
                    { 23, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 14 },
                    { 45, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 16 },
                    { 5, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 15 },
                    { 39, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 15 },
                    { 6, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 16 },
                    { 28, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 9 },
                    { 7, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 17 },
                    { 8, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 18 },
                    { 41, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 18 },
                    { 9, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 19 },
                    { 42, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 19 },
                    { 35, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 5, 20 },
                    { 34, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 5, 21 },
                    { 36, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 5, 22 },
                    { 37, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 5, 23 },
                    { 33, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 14 },
                    { 14, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 9 },
                    { 17, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 11 },
                    { 11, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 5 }
                });

            migrationBuilder.InsertData(
                table: "PagePermission",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "PageId", "PermissionId" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1 },
                    { 2, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 2 },
                    { 3, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 3 },
                    { 4, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 4 },
                    { 25, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 5 },
                    { 46, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 5 },
                    { 10, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 6 },
                    { 24, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 6 },
                    { 40, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 6 },
                    { 12, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 7 },
                    { 47, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 28 },
                    { 26, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 7 },
                    { 43, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 8 },
                    { 13, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 8 },
                    { 27, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 8 }
                });

            migrationBuilder.InsertData(
                table: "PermissionLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PermissionId" },
                values: new object[,]
                {
                    { 33, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Update Project", 1, 17 },
                    { 34, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "تعديل مشروع", 2, 17 },
                    { 5, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Update Workspace", 1, 3 },
                    { 17, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Archive Task", 1, 9 },
                    { 35, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Delete Project", 1, 18 },
                    { 36, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مسح مشروع", 2, 18 },
                    { 4, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مشاهدة  مساحة العمل", 2, 2 },
                    { 3, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "View Workspace", 1, 2 },
                    { 37, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Archive Project", 1, 19 },
                    { 38, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "ارشيف المشروع", 2, 19 },
                    { 39, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "View Role", 1, 20 },
                    { 40, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مشاهدة دور", 2, 20 },
                    { 2, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة مساحة عمل", 2, 1 },
                    { 41, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add Role", 1, 21 },
                    { 42, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة دور", 2, 21 },
                    { 1, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add WorkSpace", 1, 1 },
                    { 43, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Update Role", 1, 22 },
                    { 44, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "تعديل دور", 2, 22 },
                    { 45, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Delete Role", 1, 23 },
                    { 6, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "تعديل مساحة العمل", 2, 3 },
                    { 46, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مسح دور", 2, 23 },
                    { 31, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "View Project", 1, 16 },
                    { 18, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "ارشيف المهمة", 2, 9 },
                    { 21, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "View File", 1, 11 },
                    { 22, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مشاهدة ملف", 2, 11 },
                    { 12, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة مهمة", 2, 6 },
                    { 11, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add Task", 1, 6 }
                });

            migrationBuilder.InsertData(
                table: "PermissionLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PermissionId" },
                values: new object[,]
                {
                    { 20, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة ملفات", 2, 10 },
                    { 23, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Delete File", 1, 12 },
                    { 24, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مسح ملف", 2, 12 },
                    { 19, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add File", 1, 10 },
                    { 13, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Update Task", 1, 7 },
                    { 10, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مشاهدة المهمة", 2, 5 },
                    { 9, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "View Task", 1, 5 },
                    { 25, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Assign Member", 1, 13 },
                    { 26, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة عضو", 2, 13 },
                    { 14, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "تعديل المهمة", 2, 7 },
                    { 27, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add Comment", 1, 14 },
                    { 28, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة كومنت", 2, 14 },
                    { 16, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مسح المهمة", 2, 8 },
                    { 8, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مسح مساحة العمل", 2, 4 },
                    { 29, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add Project", 1, 15 },
                    { 30, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة مشروع", 2, 15 },
                    { 7, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Delete Workspace", 1, 4 },
                    { 32, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مشاهدة مشروع", 2, 16 },
                    { 15, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Delete Task", 1, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LanguageId",
                table: "AspNetUsers",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PageLocalization_LanguageId",
                table: "PageLocalization",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermission_PageId",
                table: "PagePermission",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermission_PermissionId",
                table: "PagePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionLocalization_LanguageId",
                table: "PermissionLocalization",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionLocalization_PermissionId",
                table: "PermissionLocalization",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePagePermission_PagePermissionId",
                table: "RolePagePermission",
                column: "PagePermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePagePermission_RoleId",
                table: "RolePagePermission",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "PageLocalization");

            migrationBuilder.DropTable(
                name: "PermissionLocalization");

            migrationBuilder.DropTable(
                name: "RolePagePermission");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "PagePermission");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Page");

            migrationBuilder.DropTable(
                name: "Permission");
        }
    }
}
