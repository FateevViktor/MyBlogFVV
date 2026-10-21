using MyBlogFVV.BLL.Models.Tag;
using MyBlogFVV.API.Models.Tag;

namespace MyBlogFVV.API.Services
{
    internal class MyMappingTag
    {
        public static TagRequest GetTagRequestFromTagModel(TagModel tagViewModel)
        {
            TagRequest tagRequest = new()
            {
                Id = tagViewModel.Id,
                Text = tagViewModel.Text
            };
            return tagRequest;
        }
        public static TagModel GetTagModelFromTagRequest(TagRequest tagRequest)
        {
            TagModel tagViewModel = new()
            {
                Id = tagRequest.Id,
                Text = tagRequest.Text
            };
            return tagViewModel;
        }
        public static SearchTagsModel GetSearchTagsModelFromListTagRequest(List<TagRequest> tagRequest)
        {
            SearchTagsModel searchTagsViewModel = new();
            foreach (var item in tagRequest)
            {
                TagModel tagViewModel = new()
                {
                    Id = item.Id,
                    Text = item.Text
                };

                searchTagsViewModel.TagList.Add(tagViewModel);
            }
            return searchTagsViewModel;
        }
        public static TagEditModel GetTagEditModelFromTagRequest(TagRequest tagRequest)
        {
            TagEditModel tagEditViewModel = new()
            {
                Id = tagRequest.Id,
                Text = tagRequest.Text
            };
            return tagEditViewModel;
        }
        public static TagRequest GetTagRequestFromTagEditModel(TagEditModel tagEditViewModel)
        {
            TagRequest tagRequest = new()
            {
                Id = tagEditViewModel.Id,
                Text = tagEditViewModel.Text
            };
            return tagRequest;
        }
        public static TagCheckedModel GetTagCheckedModelFromTagRequest(TagRequest tagRequest)
        {
            TagCheckedModel tagCheckedViewModel = new()
            {
                Id = tagRequest.Id,
                Text = tagRequest.Text,
                Checked = false
            };
            return tagCheckedViewModel;
        }
        public static List<TagCheckedModel> GetListTagCheckedModelFromListTagRequest(List<TagRequest> tagRequest)
        {
            List<TagCheckedModel> listTagCheckedViewModel = [];
            foreach (var item in tagRequest)
            {
                TagCheckedModel tagCheckedViewModel = new()
                {
                    Id = item.Id,
                    Text = item.Text,
                    Checked = false
                };

                listTagCheckedViewModel.Add(tagCheckedViewModel);
            }
            return listTagCheckedViewModel;
        }

    }
}
