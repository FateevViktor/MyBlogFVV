using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.Tag
{
    public class TagCheckedViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Тег")]
        public string Text { get; set; } = string.Empty;

        public bool Checked { get; set; } = false;
    }
}
