
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.API.Models.Tag
{
    public class TagCheckedModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;
        public bool Checked { get; set; } = false;
    }
}
