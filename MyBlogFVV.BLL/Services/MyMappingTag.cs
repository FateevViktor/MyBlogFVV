using MyBlogFVV.BLL.Models.Tag;
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.BLL.Services
{
    internal class MyMappingTag
    {
        public Tag GetTagFromTagRequest(TagRequest tagRequest)
        {
            Tag tag = new Tag();
            tag.TagId = tagRequest.Id;
            tag.Text = tagRequest.Text;
            return tag;
        }
        public TagRequest GetTagRequestFromTag(Tag tag)
        {
            TagRequest tagRequest = new TagRequest();
            tagRequest.Id = tag.TagId;
            tagRequest.Text = tag.Text;
            return tagRequest;
        }
        public List<TagRequest> GetListTagRequestFromListTag(List<Tag> tag)
        {
            List<TagRequest> listTagRequest = new List<TagRequest>();

            foreach (var item in tag)
            {
                TagRequest tagRequest = new TagRequest();
                tagRequest.Id = item.TagId;
                tagRequest.Text = item.Text;
                listTagRequest.Add(tagRequest);
            }

            return listTagRequest;
        }
    }
}
