using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.Tag
{
    public class TagEditViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Тег")]
        public string Text { get; set; } = string.Empty;
    }
}
