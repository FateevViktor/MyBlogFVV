namespace MyBlogFVV.DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
