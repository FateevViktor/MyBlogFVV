
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.API.Models.Comment
{
    public class CommentEditModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести дату и время комментария")]
        [DataType(DataType.DateTime)] //Указываем, что нам нужна дата и время
        public DateTime CommentDate { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить комментарий")]
        [DataType(DataType.MultilineText)] //многострочный текст
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Длина комментария должна быть от 1 до 200 символов")]
        public string Text { get; set; } = string.Empty;
    }
}
