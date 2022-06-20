IF OBJECT_ID('[dbo].[sp_GetPageAndPermissionBasedInRoles]') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_SearchInUserForAssigningToProject]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetPageAndPermissionBasedInRoles]    Script Date: 11/11/2021 9:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Eslam Ali
-- Create Date: 2021-11-10
-- Description: Get All Page And Permission Based In Roles
-- =============================================
create PROCEDURE sp_GetPageAndPermissionBasedInRoles
 @currentUserId bigint ,
 @currentRoleId bigint 
 as
 begin
 select pp.Id as PagePermissionId, pgl.DisplayName as PageName, prl.DisplayName as PermissionName, r.Id as RoleId,
 CASE
When r.Id is null then 0
else 1 end as IsAssigned
from PagePermission pp 
inner join [Page] pg on pg.Id = pp.PageId
inner join Permission pr on pr.Id = pp.PermissionId
inner join PageLocalization pgl on pg.Id = pgl.PageId
inner join PermissionLocalization prl on pr.Id = prl.PermissionId 
left outer join RolePagePermission rpp on pp.Id = rpp.PagePermissionId and rpp.roleid = @currentRoleId
left outer join AspNetRoles r on rpp.RoleId = r.Id 
where pgl.LanguageId in (select LanguageId from AspNetUsers where Id=@currentUserId)
and prl.LanguageId in (select LanguageId from AspNetUsers where Id=@currentUserId)
 end

