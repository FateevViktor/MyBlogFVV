
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.Comment
{
    public class CommentEditViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage ="Необходимо ввести дату и время комментария")]
        [Display(Name = "Дата комментария")]
        [DataType(DataType.DateTime)] //Указываем, что нам нужна дата и время
        public DateTime CommentDate { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить комментарий")]
        [Display(Name = "Текст комментария")]
        [DataType(DataType.MultilineText)] //многострочный текст
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Длина комментария должна быть от 1 до 200 символов")]
        public string Text { get; set; } = string.Empty;
    }
}
