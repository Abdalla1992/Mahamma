using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.MyWork.Dto;
using Mahamma.Domain.MyWork.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.Domain.MyWork.Repository
{
    public interface INoteRepository : IRepository<Note>
    {
        Task<List<NoteDto>> GetAllNotes(long ownerId);
        void AddNote(NoteDto note);
        void DeleteNote(int noteId);
        void UpdateNoteColor(int id, string colorCode);
        UserTasksInfoDto GetUserTasksInfo(long userId);
    }
}
