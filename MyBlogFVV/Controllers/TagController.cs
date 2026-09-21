using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Tag;
using MyBlogFVV.WEB.Models.Tag;
using MyBlogFVV.WEB.Services;

namespace MyBlogFVV.WEB.Controllers
{
    [Route("Tag")]
    [Authorize]
    //[Authorize(Policy = "OnlyForRoleAdmin")]
    public class TagController : Controller
    {
        MyMappingTag myMappingTag = new MyMappingTag();
        ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //---------------Блок создания тега-----------------
        [Route("Add")]
        [HttpGet]
        public IActionResult Add()
        {
            TagViewModel tagViewModel = new TagViewModel();
            return View(tagViewModel);
        }
        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add(TagViewModel model)
        {
            if (ModelState.IsValid)
            {
                TagRequest tagRequest = myMappingTag.GetTagRequestFromTagViewModel(model);

                OperationDetails result = await _tagService.Create(tagRequest);
                if (result.Succedeed == true)
                {
                    return RedirectToAction("TagList");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                }
            }
            return RedirectToAction("TagList");
        }
        //------------------------------------------------
        //--------Редактирование тега-------------------
        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int tagId)
        {
            TagEditViewModel tagEditViewModel = new TagEditViewModel();
            TagRequest? tagRequest = new TagRequest();
            if (tagId > 0)
            {
                tagRequest = await _tagService.GetTagById(tagId);

                if (tagRequest != null)
                {
                    tagEditViewModel = myMappingTag.GetTagEditViewModelFromTagRequest(tagRequest);
                    return View("Edit", tagEditViewModel);
                }
            }
            return RedirectToAction("CommentList");
        }

        [Route("UpdateEdit")]
        [HttpPost]
        public async Task<IActionResult> UpdateEdit(TagEditViewModel model)
        {
            TagRequest? tagRequest = new TagRequest();
            if (ModelState.IsValid)
            {
                tagRequest = myMappingTag.GetTagRequestFromTagEditViewModel(model);
                OperationDetails result = await _tagService.Update(tagRequest);
                if (result.Succedeed == true)
                {
                    return RedirectToAction("TagList");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                    return RedirectToAction("TagList");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }
        //------------------------------------------------
        //--------------Удаление тега-------------
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int tagId)
        {
            if (tagId > 0)
            {
                OperationDetails result = await _tagService.Delete(tagId);

                if (result.Succedeed == true)
                {
                    return RedirectToAction("TagList");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                    return RedirectToAction("TagList");
                }
            }
            else
            {
                ModelState.AddModelError("Id", "Id должен быть более нуля");
            }
            return RedirectToAction("TagList");
        }
        //------------------------------------------------
        //---------Список всех тегов-------------------
        [Route("TagList")]
        [HttpGet]
        public async Task<IActionResult> TagList()
        {
            SearchTagsViewModel searchTagsViewModel = new SearchTagsViewModel();
            List<TagRequest>? tagList = await _tagService.GetAll();
            if (tagList != null)
            {
                searchTagsViewModel = myMappingTag.GetSearchTagsViewModelFromListTagRequest(tagList);
            }
            return View("TagList", searchTagsViewModel);
        }
        
        [Route("TagList")]
        [HttpPost]
        public async Task<IActionResult> TagList(int Id)
        {
            SearchTagsViewModel searchTagsViewModel = new SearchTagsViewModel();
            if (Id > 0)
            {
                TagRequest? tagRequest = new TagRequest();
                tagRequest = await _tagService.GetTagById(Id);
                List<TagRequest> tagList = new List<TagRequest>();
                if (tagRequest != null)
                {
                    tagList.Add(tagRequest);
                    searchTagsViewModel = myMappingTag.GetSearchTagsViewModelFromListTagRequest(tagList);
                    return View("TagList", searchTagsViewModel);
                }
                else
                {
                    return RedirectToAction("TagList");
                }
            }
            else
            {
                return RedirectToAction("TagList");
            }
        }
        //------------------------------------------------
    }
}
