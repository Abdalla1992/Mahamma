using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.ProjectActivity.Entity;
using Mahamma.Domain.ProjectActivity.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.AtivityLogger.Project
{
    public class ProjectActivityLogger : IProjectActivityLogger
    {
        private readonly IProjectActivityRepository _projectActivityRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        public ProjectActivityLogger(IProjectActivityRepository projectActivityRepository , IProjectMemberRepository projectMemberRepository)
        {
            _projectActivityRepository = projectActivityRepository;
            _projectMemberRepository = projectMemberRepository;
        }

        public void LogProjectActivity(string action, int ProjectId, int projectMemberId, CancellationToken cancellationToken)
        {
            ProjectActivity activity = new();
            activity.LogProjectActivity(action,ProjectId, projectMemberId);
            _projectActivityRepository.LogActivity(activity);
        }
       
        public async System.Threading.Tasks.Task LogProjectActivity(string action, int projectId, long userId, CancellationToken cancellationToken)
        {
            int member = await _projectMemberRepository.GetMemberById(projectId, userId);
            ProjectActivity activity = new();
            activity.LogProjectActivity(action, projectId, member);
            _projectActivityRepository.LogActivity(activity);
        }
    }
}
