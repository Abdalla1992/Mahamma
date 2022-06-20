using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Identity.Api.Migrations
{
    public partial class InsertNewPermisionInMeetinAndMOF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Page",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "Name" },
                values: new object[,]
                {
                    { 7, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "MeetingProfile" },
                    { 8, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "ManageMeetings" },
                    { 9, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "ManageMinutesOfMeetings" }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 30, "AddMeetingGeneral" },
                    { 31, "AddMeetingWorkspace" },
                    { 32, "AddMeetingProject" },
                    { 33, "AddMeetingTask" },
                    { 34, "UpdateMeeting" },
                    { 35, "DeleteMeeting" },
                    { 36, "ViewMeeting" },
                    { 37, "UpdateMinuteOfMeeting" },
                    { 38, "DeleteMinuteOfMeeting" },
                    { 39, "AddMinuteOfMeeting" },
                    { 40, "ViewMinuteOfMeeting" }
                });

            migrationBuilder.InsertData(
                table: "PageLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PageId" },
                values: new object[,]
                {
                    { 13, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Meeting Profile", 1, 7 },
                    { 14, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "الصفحة الخاصة بالاجتماعات", 2, 7 },
                    { 15, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manage Meetings", 1, 8 },
                    { 16, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "تنظيم الاجتماعات", 2, 8 },
                    { 17, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manage Minutes Of Meetings", 1, 9 },
                    { 18, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "تنظيم النقاط الخاصة بالاجتماع", 2, 9 }
                });

            migrationBuilder.InsertData(
                table: "PagePermission",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "PageId", "PermissionId" },
                values: new object[,]
                {
                    { 58, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7, 38 },
                    { 62, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 9, 38 },
                    { 52, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8, 33 },
                    { 61, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 9, 37 },
                    { 56, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7, 36 },
                    { 51, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8, 32 },
                    { 50, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8, 31 },
                    { 57, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7, 37 },
                    { 53, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8, 34 },
                    { 54, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8, 36 },
                    { 59, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 9, 40 },
                    { 49, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8, 30 },
                    { 60, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 9, 39 },
                    { 55, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8, 35 }
                });

            migrationBuilder.InsertData(
                table: "PermissionLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PermissionId" },
                values: new object[,]
                {
                    { 66, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مسح النقاط الخاصة بالاجتماع", 2, 38 },
                    { 64, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "تعديل النقاط الخاصة بالاجتماع", 2, 37 },
                    { 65, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Delete Minute Of Meeting", 1, 38 },
                    { 67, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add Minute Of Meeting", 1, 39 },
                    { 68, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة نقاط الخاصة بالاجتماع", 2, 39 },
                    { 63, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Update Minute Of Meeting", 1, 37 },
                    { 62, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مشاهدة الاجتماع", 2, 36 },
                    { 58, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "تعديل الاجتماع", 2, 34 },
                    { 60, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مسح الاجتماع", 2, 35 },
                    { 59, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Delete Meeting", 1, 35 },
                    { 69, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "View Minute Of Meeting", 1, 40 },
                    { 57, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Update Meeting", 1, 34 },
                    { 56, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة اجتماع على مستوى المهمة", 2, 33 },
                    { 55, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add Meeting Task", 1, 33 },
                    { 54, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة اجتماع على مستوى المشروع", 2, 32 },
                    { 53, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add Meeting Project", 1, 32 },
                    { 52, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة اجتماع على مستوى مساحة العمل", 2, 31 },
                    { 51, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add Meeting Workspace", 1, 31 },
                    { 50, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة اجتماع على العموم", 2, 30 },
                    { 49, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add Meeting General", 1, 30 },
                    { 61, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "View Meeting", 1, 36 },
                    { 70, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مشاهدة النقاط الخاصة بالاجتماع", 2, 40 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Page",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Page",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Page",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 40);
        }
    }
}
