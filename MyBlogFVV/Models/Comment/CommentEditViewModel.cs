
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
        [DataType(DataType.DateTime)] //Указываем, что нам нужна дата и время
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CommentDate { get; set; }

        [Required]
        [Display(Name = "Текст комментария")]
        [DataType(DataType.MultilineText)] //многострочный текст
        public string Text { get; set; } = string.Empty;
    }
}
