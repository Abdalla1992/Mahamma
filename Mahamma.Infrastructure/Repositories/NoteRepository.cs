using AutoMapper;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.MyWork.Dto;
using Mahamma.Domain.MyWork.Entity;
using Mahamma.Domain.MyWork.Repository;
using Mahamma.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mahamma.Domain.Meeting.Entity;

namespace Mahamma.Infrastructure.Repositories
{
    public class NoteRepository : Base.EntityRepository<Note>, INoteRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public NoteRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        {
        }

        public async Task<List<NoteDto>> GetAllNotes(long ownerId)
        {
            List<Note> notes = await GetWhereNoTrackingAsync(n => n.OwnerId == ownerId);
            return Mapper.Map<List<NoteDto>>(notes);
        }

        public void AddNote(NoteDto noteDto)
        {
            Note note = Mapper.Map<Note>(noteDto);
            note.CreateNote();

            CreateAsyn(note);
        }

        public void DeleteNote(int noteId)
        {
            Note note = AppDbContext.Set<Note>().SingleOrDefault(p => p.Id == noteId);
            Delete(note);
        }

        public void UpdateNoteColor(int id, string colorCode)
        {
            Note note = AppDbContext.Set<Note>().SingleOrDefault(p => p.Id == id);
            note.ColorCode = colorCode;
            Update(note);
        }

        public UserTasksInfoDto GetUserTasksInfo(long userId)
        {
            UserTasksInfoDto userTasksInfoDto = new UserTasksInfoDto();

            List<Domain.Task.Entity.Task> userTasks = AppDbContext.Task.Include(t => t.TaskMembers)
                                                       .Where(t => t.TaskMembers.Any(m => m.UserId == userId))
                                                       .AsNoTracking().ToList();

            userTasksInfoDto.TasksCount = userTasks.Count;

            userTasksInfoDto.DueDate = userTasks.Where(t => t.TaskStatusId != Domain.Task.Enum.TaskStatus.CompletedEarly.Id
                                                        && t.TaskStatusId != Domain.Task.Enum.TaskStatus.CompletedLate.Id
                                                        && t.TaskStatusId != Domain.Task.Enum.TaskStatus.CompletedOnTime.Id)
                                                .OrderBy(t => t.DueDate).FirstOrDefault().DueDate;

            userTasksInfoDto.ProgressPercentage = userTasks.Count(t => t.TaskStatusId != Domain.Task.Enum.TaskStatus.InProgress.Id
                                                                    && t.TaskStatusId != Domain.Task.Enum.TaskStatus.InProgressWithDelay.Id
                                                                    && t.TaskStatusId != Domain.Task.Enum.TaskStatus.New.Id)
                                                    * 100 / userTasks.Count();

            IQueryable<Meeting> userMeetings = AppDbContext.Set<Meeting>().Include(m => m.Members)
                                                     .Where(m => m.Members.Any(m => m.UserId == userId))
                                                     .AsNoTracking();

            userTasksInfoDto.UpcomingMeetingDate = userMeetings.OrderBy(m => m.Date).FirstOrDefault().Date;

            return userTasksInfoDto;
        }
    }
}
