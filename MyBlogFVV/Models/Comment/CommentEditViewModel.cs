
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.Comment
{
    public class CommentEditViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Дата комментария")]
        [DataType(DataType.Date)] //Указываем, что нам нужна только дата
        public DateTime CommentDate { get; set; }

        [Required]
        [Display(Name = "Текст комментария")]
        public string Text { get; set; } = string.Empty;
    }
}
