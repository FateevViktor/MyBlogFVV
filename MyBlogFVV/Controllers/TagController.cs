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
    public class TagController(ITagService tagService) : Controller
    {
        readonly ITagService _tagService = tagService;

        public IActionResult Index()
        {
            return View();
        }
        //---------------Блок создания тега-----------------
        [Route("Add")]
        [HttpGet]
        public IActionResult Add()
        {
            TagViewModel tagViewModel = new();
            return View(tagViewModel);
        }
        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add(TagViewModel model)
        {
            if (ModelState.IsValid)
            {
                TagRequest tagRequest = MyMappingTag.GetTagRequestFromTagViewModel(model);

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
            TagEditViewModel tagEditViewModel;
            TagRequest? tagRequest;
            if (tagId > 0)
            {
                tagRequest = await _tagService.GetTagById(tagId);

                if (tagRequest != null)
                {
                    tagEditViewModel = MyMappingTag.GetTagEditViewModelFromTagRequest(tagRequest);
                    return View("Edit", tagEditViewModel);
                }
            }
            return RedirectToAction("CommentList");
        }

        [Route("UpdateEdit")]
        [HttpPost]
        public async Task<IActionResult> UpdateEdit(TagEditViewModel model)
        {
            TagRequest? tagRequest;
            if (ModelState.IsValid)
            {
                tagRequest = MyMappingTag.GetTagRequestFromTagEditViewModel(model);
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
            SearchTagsViewModel searchTagsViewModel = new();
            List<TagRequest>? tagList = await _tagService.GetAll();
            if (tagList != null)
            {
                searchTagsViewModel = MyMappingTag.GetSearchTagsViewModelFromListTagRequest(tagList);
            }
            return View("TagList", searchTagsViewModel);
        }
        
        [Route("TagList")]
        [HttpPost]
        public async Task<IActionResult> TagList(int Id)
        {
            SearchTagsViewModel searchTagsViewModel;
            if (Id > 0)
            {
                TagRequest? tagRequest;
                tagRequest = await _tagService.GetTagById(Id);
                List<TagRequest> tagList = [];
                if (tagRequest != null)
                {
                    tagList.Add(tagRequest);
                    searchTagsViewModel = MyMappingTag.GetSearchTagsViewModelFromListTagRequest(tagList);
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
        //----------Показать страничку с тегом------------
        [Route("ShowTag")]
        [HttpPost]
        public async Task<IActionResult> ShowTag(int tagId)
        {
            TagViewModel tagViewModel;
            if (tagId > 0)
            {
                TagRequest? tagRequest;
                tagRequest = await _tagService.GetTagById(tagId);
                if (tagRequest != null)
                {
                    tagViewModel = MyMappingTag.GetTagViewModelFromTagRequest(tagRequest);
                    return View("ShowTag", tagViewModel);
                }
            }
            return RedirectToAction("TagList", "Tag");
        }
        //----------------------------------------------
    }
}
