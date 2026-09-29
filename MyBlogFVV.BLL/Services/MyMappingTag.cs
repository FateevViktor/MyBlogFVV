using MyBlogFVV.BLL.Models.Tag;
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.BLL.Services
{
    internal class MyMappingTag
    {
        public static Tag GetTagFromTagRequest(TagRequest tagRequest)
        {
            Tag tag = new()
            {
                TagId = tagRequest.Id,
                Text = tagRequest.Text
            };
            return tag;
        }
        public static TagRequest GetTagRequestFromTag(Tag tag)
        {
            TagRequest tagRequest = new()
            {
                Id = tag.TagId,
                Text = tag.Text ?? string.Empty
            };
            return tagRequest;
        }
        public static List<TagRequest> GetListTagRequestFromListTag(List<Tag> tag)
        {
            List<TagRequest> listTagRequest = [];

            foreach (var item in tag)
            {
                TagRequest tagRequest = new()
                {
                    Id = item.TagId,
                    Text = item.Text ?? string.Empty
                };
                listTagRequest.Add(tagRequest);
            }

            return listTagRequest;
        }
        public static List<Tag> GetListTagFromListTagRequest(List<TagRequest> tagRequest)
        {
            List<Tag> listTag = [];

            foreach (var item in tagRequest)
            {
                Tag tag = new()
                {
                    TagId = item.Id,
                    Text = item.Text
                };
                listTag.Add(tag);
            }

            return listTag;
        }
    }
}
