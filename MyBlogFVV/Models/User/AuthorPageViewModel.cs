using MyBlogFVV.WEB.Models.Post;
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.User
{
    public class AuthorPageViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Логин")]
        public string Login { get; set; } = string.Empty;

        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }

        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)] //Указываем, что нам нужна только дата
        public DateTime? BirthdayDate { get; set; }

        public List<PostViewModel> Posts { get; set; } = [];
        //public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        //public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
        public List<string> Roles { get; set; } = [];
    }
}
