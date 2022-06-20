namespace Mahamma.Domain.MyWork.Dto
{
    public class NoteDto
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public string ColorCode { get; set; }
        public bool IsTask { get; set; }
        public long OwnerId { get; set; }
        public string OwnerName { get; set; }
    }
}
