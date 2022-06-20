IF OBJECT_ID('[dbo].[spGetUserTasks]') IS NOT NULL
	DROP PROCEDURE [dbo].[spGetUserTasks]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mohammed Mostafa
-- Create date: 2021-11-27
-- Description:	Get user tasks
-- =============================================
CREATE PROCEDURE [spGetUserTasks]
	@userId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select t.Id, t.[Name] as TaskName, t.TaskStatusId, t.DueDate as TaskDueDate, p.[Name] as ProjectName, ts.[Name] as TaskStatus,
	tm.Rating , ws.[Name] as WorkSpaceName from [Task] t
	inner join Project p on t.ProjectId = p.Id
	inner join TaskStatus ts on t.TaskStatusId = ts.Id
	inner join TaskMember tm on t.Id = tm.TaskId
    inner join [dbo].[Workspace] ws on p.WorkSpaceId = ws.Id
	where tm.UserId = @userId
	and tm.DeletedStatus = 1
	and t.DeletedStatus = 1
	and p.DeletedStatus = 1
	and ws.DeletedStatus = 1
END
GO
