IF OBJECT_ID('[dbo].[sp_SearchInUserForAssigningToWorkspace]') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_SearchInUserForAssigningToWorkspace]
GO
/****** Object:  StoredProcedure [dbo].[sp_SearchInUserForAssigningToWorkspace]    Script Date: 10/24/2021 9:53:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Mohammed Mostafa
-- Create Date: 2021-10-24
-- Description: Search user for assigning to workspace
-- =============================================
CREATE PROCEDURE [dbo].[sp_SearchInUserForAssigningToWorkspace]
(
   @name nvarchar(max),
   @companyId int,
   @workspaceId int,
   @currentUserId int
)
AS
BEGIN
   Select distinct u.Id as UserId, u.FullName, u.ProfileImage
   From [Mahamma.Identity_Dev].dbo.[AspNetUsers] u 
   Where u.CompanyId = @companyId
   and u.Id not in (Select UserId from WorkSpaceMember Where WorkspaceId = @workspaceId and DeletedStatus<>2)
   and (u.Id <> @currentUserId or u.Id in (Select UserId from WorkSpaceMember Where WorkspaceId = @workspaceId and DeletedStatus = 2))
   and u.FullName like '%'+@name+'%'
END
