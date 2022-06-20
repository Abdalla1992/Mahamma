IF OBJECT_ID('[dbo].[sp_GetMinutesOfMeeting]') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_GetMinutesOfMeeting]
GO
CREATE PROCEDURE [dbo].[sp_GetMinutesOfMeeting] @MeetingId INT, @UserId BIGINT
AS BEGIN
	SELECT mom.Id,
	mom.MeetingId,
	(CASE
		WHEN mom.TaskId IS NOT NULL THEN task.[Name]
		WHEN mom.ProjectId IS NOT NULL THEN proj.[Name]
		ELSE mom.[Description]
	END) AS ActionTitle,
	mom.MinuteOfMeetingLevel AS ActionLevel,
	(CASE
		WHEN mom.TaskId IS NOT NULL THEN 
			(SELECT STRING_AGG(taskmember.UserId, ', ') FROM [dbo].[TaskMember] taskmember WHERE taskmember.TaskId = mom.TaskId)
		WHEN mom.ProjectId IS NOT NULL THEN 
			(SELECT STRING_AGG(projmember.UserId, ', ') FROM [dbo].[ProjectMember] projmember WHERE projmember.ProjectId =  mom.ProjectId)
		ELSE CAST(mom.CreatorUserId AS NVARCHAR(MAX))
	END) AS Assignee,
	(CASE
		WHEN mom.TaskId IS NOT NULL THEN task.ProgressPercentage
		WHEN mom.ProjectId IS NOT NULL THEN proj.ProgressPercentage
		ELSE 0
	END) AS ProgressPercentage,
	mom.IsDraft,
	proj.WorkSpaceId AS WorkspaceId,
	(CASE
		WHEN mom.ProjectId IS NULL THEN task.ProjectId
		ELSE mom.ProjectId
	END) AS ProjectId,
	mom.TaskId,
	task.ParentTaskId,
	mom.[Description]
	FROM [dbo].[MinutesOfMeetings] mom 
	LEFT OUTER JOIN [dbo].[Project] proj on mom.ProjectId = proj.Id 
	LEFT OUTER JOIN [dbo].[Task] task on mom.TaskId = task.Id 
	WHERE mom.MeetingId = @MeetingId AND (mom.IsDraft <> 1 OR mom.CreatorUserId = @UserId)
	AND mom.DeletedStatus = 1
END
