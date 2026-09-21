
namespace MyBlogFVV.BLL.Models.User
{
    public class UserRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } 
        public string Login { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public ICollection<string> Roles { get; set; } = new List<string>();
    }
}
