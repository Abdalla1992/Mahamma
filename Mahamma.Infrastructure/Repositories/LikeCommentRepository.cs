using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Entity;
using Mahamma.Domain.Task.Repository;
using Mahamma.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class LikeCommentRepository : Base.EntityRepository<Domain.Task.Entity.LikeComment>, ILikeCommentRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public LikeCommentRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }
        public async System.Threading.Tasks.Task LikeComment(int memberId, int commentId)
        {
            LikeComment like = await FirstOrDefaultAsync(c => c.TaskCommentId == commentId && c.TaskMemberId == memberId);
            if (like == null)
            {
                like = new();
                like.LikeAComment(commentId, memberId);
                CreateAsyn(like);
            }
            else
            {
                Delete(like);
            }
        }
    }
}
