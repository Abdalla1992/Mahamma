IF OBJECT_ID('[dbo].[sp_SearchInUserForAssigningToProject]') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_SearchInUserForAssigningToProject]
GO
/****** Object:  StoredProcedure [dbo].[sp_SearchInUserForAssigningToProject]    Script Date: 10/24/2021 9:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Mohammed Mostafa
-- Create Date: 2021-10-24
-- Description: Search user for assigning to project
-- =============================================
CREATE PROCEDURE [dbo].[sp_SearchInUserForAssigningToProject]
(
   @name nvarchar(max),
   @companyId int,
   @projectId int,
   @currentUserId int
)
AS
BEGIN
   Select distinct u.Id as UserId, u.FullName, u.ProfileImage, ws.Id as WorkspaceId, ws.[Name] as WorkspaceName
   From [Mahamma.Identity_Dev].dbo.[AspNetUsers] u 
   Inner join WorkSpaceMember wsm on u.Id = wsm.UserId
   Inner join Workspace ws on wsm.WorkspaceId = ws.Id
   Where u.CompanyId = @companyId
   and u.Id not in (Select UserId from ProjectMember Where ProjectId = @projectId and DeletedStatus<>2)
   and (u.Id <> @currentUserId or u.Id in (Select UserId from ProjectMember Where ProjectId = @projectId and DeletedStatus = 2))
   and ws.DeletedStatus = 1
   and u.FullName Like '%'+@name+'%'
END
