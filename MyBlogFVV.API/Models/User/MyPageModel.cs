using MyBlogFVV.API.Models.Post;

namespace MyBlogFVV.API.Models.User
{
    public class MyPageModel
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime? BirthdayDate { get; set; }
        public List<PostModel> Posts { get; set; } = [];
        public List<string> Roles { get; set; } = [];
    }
}
