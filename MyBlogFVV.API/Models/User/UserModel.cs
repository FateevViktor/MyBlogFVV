
namespace MyBlogFVV.API.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
