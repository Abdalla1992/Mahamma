using AutoMapper;
using Mahamma.Base.Dto.Enum;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Workspace.Entity;
using Mahamma.Domain.Workspace.Repository;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class WorkSpaceMemberRepository : Base.EntityRepository<WorkspaceMember>, IWorkspaceMemberRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public WorkSpaceMemberRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public async Task<List<WorkspaceMember>> GetWorkSpaceMemberById(int WorkSpaceId)
        {
            return await GetWhereAsync(m => m.WorkspaceId == WorkSpaceId && m.DeletedStatus != DeletedStatus.Deleted.Id);
        }

        public void AddWorkSpaceMember(WorkspaceMember workSpaceMember)
        {
            CreateAsyn(workSpaceMember);
        }

        public void UpdateWorkSpaceMemberList(List<WorkspaceMember> workSpaceMember)
        {
            UpdateList(workSpaceMember);
        }

        public async Task<WorkspaceMember> GetByWorkspaceIdAndUserId(int workspaceId, long userId)
        {
            return await FirstOrDefaultAsync(w => w.WorkspaceId == workspaceId && w.UserId == userId);
        }

        public void DeleteWorkSpaceMember(WorkspaceMember workSpaceMember)
        {
            Delete(workSpaceMember);
        }
    }
}
