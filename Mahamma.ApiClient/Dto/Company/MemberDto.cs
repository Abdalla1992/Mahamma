namespace Mahamma.ApiClient.Dto.Company
{
    public class MemberDto
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
        public int WorkspaceId { get; set; }
        public string WorkspaceName { get; set; }
    }
}
