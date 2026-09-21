using MyBlogFVV.WEB.Models.Comment;
using MyBlogFVV.WEB.Models.Post;
using MyBlogFVV.WEB.Models.Tag;
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.User
{
    public class MyPageViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }

        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)] //Указываем, что нам нужна только дата
        public DateTime? BirthdayDate { get; set; }

        public List<PostViewModel> Posts { get; set; } = new List<PostViewModel>();
        //public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        //public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
        public List<string> Roles { get; set; } = new List<string>();
    }
}
