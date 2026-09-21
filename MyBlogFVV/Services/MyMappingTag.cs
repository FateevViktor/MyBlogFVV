using MyBlogFVV.BLL.Models.Tag;
using MyBlogFVV.WEB.Models.Tag;

namespace MyBlogFVV.WEB.Services
{
    internal class MyMappingTag
    {
        public TagRequest GetTagRequestFromTagViewModel(TagViewModel tagViewModel)
        {
            TagRequest tagRequest = new TagRequest();
            tagRequest.Id = tagViewModel.Id;
            tagRequest.Text = tagViewModel.Text;
            return tagRequest;
        }
        public TagViewModel GetTagViewModelFromTagRequest(TagRequest tagRequest)
        {
            TagViewModel tagViewModel = new TagViewModel();
            tagViewModel.Id = tagRequest.Id;
            tagViewModel.Text = tagRequest.Text;
            return tagViewModel;
        }
        public SearchTagsViewModel GetSearchTagsViewModelFromListTagRequest(List<TagRequest> tagRequest)
        {
            SearchTagsViewModel searchTagsViewModel = new SearchTagsViewModel();
            foreach (var item in tagRequest)
            {
                TagViewModel tagViewModel = new TagViewModel();

                tagViewModel.Id = item.Id;
                tagViewModel.Text = item.Text;

                searchTagsViewModel.TagList.Add(tagViewModel);
            }
            return searchTagsViewModel;
        }
        public TagEditViewModel GetTagEditViewModelFromTagRequest(TagRequest tagRequest)
        {
            TagEditViewModel tagEditViewModel = new TagEditViewModel();
            tagEditViewModel.Id = tagRequest.Id;
            tagEditViewModel.Text = tagRequest.Text;
            return tagEditViewModel;
        }
        public TagRequest GetTagRequestFromTagEditViewModel(TagEditViewModel tagEditViewModel)
        {
            TagRequest tagRequest = new TagRequest();
            tagRequest.Id = tagEditViewModel.Id;
            tagRequest.Text = tagEditViewModel.Text;
            return tagRequest;
        }
    }
}
