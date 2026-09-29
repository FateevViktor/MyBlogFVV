using MyBlogFVV.BLL.Models.Tag;
using MyBlogFVV.WEB.Models.Tag;

namespace MyBlogFVV.WEB.Services
{
    internal class MyMappingTag
    {
        public static TagRequest GetTagRequestFromTagViewModel(TagViewModel tagViewModel)
        {
            TagRequest tagRequest = new()
            {
                Id = tagViewModel.Id,
                Text = tagViewModel.Text
            };
            return tagRequest;
        }
        public static TagViewModel GetTagViewModelFromTagRequest(TagRequest tagRequest)
        {
            TagViewModel tagViewModel = new()
            {
                Id = tagRequest.Id,
                Text = tagRequest.Text
            };
            return tagViewModel;
        }
        public static SearchTagsViewModel GetSearchTagsViewModelFromListTagRequest(List<TagRequest> tagRequest)
        {
            SearchTagsViewModel searchTagsViewModel = new();
            foreach (var item in tagRequest)
            {
                TagViewModel tagViewModel = new()
                {
                    Id = item.Id,
                    Text = item.Text
                };

                searchTagsViewModel.TagList.Add(tagViewModel);
            }
            return searchTagsViewModel;
        }
        public static TagEditViewModel GetTagEditViewModelFromTagRequest(TagRequest tagRequest)
        {
            TagEditViewModel tagEditViewModel = new()
            {
                Id = tagRequest.Id,
                Text = tagRequest.Text
            };
            return tagEditViewModel;
        }
        public static TagRequest GetTagRequestFromTagEditViewModel(TagEditViewModel tagEditViewModel)
        {
            TagRequest tagRequest = new()
            {
                Id = tagEditViewModel.Id,
                Text = tagEditViewModel.Text
            };
            return tagRequest;
        }
        public static TagCheckedViewModel GetTagCheckedViewModelFromTagRequest(TagRequest tagRequest)
        {
            TagCheckedViewModel tagCheckedViewModel = new()
            {
                Id = tagRequest.Id,
                Text = tagRequest.Text,
                Checked = false
            };
            return tagCheckedViewModel;
        }
        public static List<TagCheckedViewModel> GetListTagCheckedViewModelFromListTagRequest(List<TagRequest> tagRequest)
        {
            List<TagCheckedViewModel> listTagCheckedViewModel = [];
            foreach (var item in tagRequest)
            {
                TagCheckedViewModel tagCheckedViewModel = new()
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
