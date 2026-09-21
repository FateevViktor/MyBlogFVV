
namespace MyBlogFVV.BLL.Models.User
{
    public class RegisterRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;

    }
}
