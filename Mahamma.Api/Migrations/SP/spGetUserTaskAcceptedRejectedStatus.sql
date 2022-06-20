IF OBJECT_ID('[dbo].[spGetUserTaskAcceptedRejectedStatus]') IS NOT NULL
	DROP PROCEDURE [dbo].spGetUserTaskAcceptedRejectedStatus
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Eslam Ali
-- Create date: 2022-5-11
-- Description:	Get user Task  Accepted Or Rejected Taskes Status
-- =============================================
create proc spGetUserTaskAcceptedRejectedStatus
@userId bigint
as
begin
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
select t.Id , t.Name as TaskName , p.Name as ProjectName ,t.TaskStatusId , t.DueDate as TaskDueDate , ws.Name as WorkspaceName ,ts.[Name] as TaskStatus 
from Task t 
inner join Project p on t.projectId = p.Id 
inner join Workspace ws on p.WorkspaceId = ws.Id
inner join TaskMember tm on t.Id = tm.TaskId
inner join TaskStatus ts on t.[TaskStatusId] = ts.Id
where tm.UserId = @userId 
and (tm.TaskAcceptedRejectedStatus = 4 or tm.TaskAcceptedRejectedStatus = 3)
and t.DeletedStatus = 1
and p.DeletedStatus = 1
and ws.DeletedStatus =1
and tm.DeletedStatus =1
end