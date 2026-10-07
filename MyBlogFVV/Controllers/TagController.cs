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
    public class TagController(ITagService tagService, ILogger<TagController> logger) : Controller
    {
        readonly ITagService _tagService = tagService;
        private readonly ILogger<TagController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }
        //---------------Блок создания тега-----------------
        [Route("Add")]
        [HttpGet]
        public IActionResult Add()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку создания тега. Add", User.Identity?.Name);
            TagViewModel tagViewModel = new();
            return View(tagViewModel);
        }
        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add(TagViewModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается добавить тег. Add", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                TagRequest tagRequest = MyMappingTag.GetTagRequestFromTagViewModel(model);
                _logger.LogInformation("Запрос в БД. Посылаем запрос на создания тега в БД. Add");

                OperationDetails result;
                try
                {
                    result = await _tagService.Create(tagRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Add класс TagController.");
                    return View("Error");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Получилось. Add");
                    return RedirectToAction("TagList");
                }
                else
                {
                    _logger.LogWarning("Не удалось. Сообщение - result.Message. Add");
                    ModelState.AddModelError(result.Property, result.Message);
                    return View("Add", model);
                }
            }
            else
            {
                _logger.LogInformation("Некорректные данные. Add");
                ModelState.AddModelError("", "Некорректные данные");
                return View("Add", model);
            }            
        }
        //------------------------------------------------
        //--------Редактирование тега-------------------
        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int tagId)
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел в тег с id={id}. Edit", User.Identity?.Name, tagId.ToString());
            TagEditViewModel tagEditViewModel;
            TagRequest? tagRequest;
            if (tagId > 0)
            {
                _logger.LogInformation("Запрос в БД. Посылаем запрос на получение тега по его id={id}. Edit", tagId.ToString());

                try
                {
                    tagRequest = await _tagService.GetTagById(tagId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Edit класс TagController. tagId={id}", tagId.ToString());
                    return View("Error");
                }                

                if (tagRequest != null)
                {
                    _logger.LogInformation("Подгрузили. Edit");
                    tagEditViewModel = MyMappingTag.GetTagEditViewModelFromTagRequest(tagRequest);
                    return View("Edit", tagEditViewModel);
                }
                else
                {
                    _logger.LogInformation("Не подгрузили. Edit");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю. Edit");
            }
            return RedirectToAction("CommentList");
        }

        [Route("UpdateEdit")]
        [HttpPost]
        public async Task<IActionResult> UpdateEdit(TagEditViewModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается изменить тег. UpdateEdit", User.Identity?.Name);
            TagRequest? tagRequest;
            if (ModelState.IsValid)
            {
                tagRequest = MyMappingTag.GetTagRequestFromTagEditViewModel(model);
                _logger.LogInformation("Запрос в БД. Посылаем запрос на изменение тега. UpdateEdit");

                OperationDetails result;
                try
                {
                    result = await _tagService.Update(tagRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе UpdateEdit класс TagController.");
                    return View("Error");
                }
                
                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Получилось. UpdateEdit");
                    return RedirectToAction("TagList");
                }
                else
                {
                    _logger.LogWarning("Не удалось. Сообщение - {Message}. UpdateEdit", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return View("Edit", model);
                }
            }
            else
            {
                _logger.LogInformation("Введены некорректные данные. UpdateEdit");
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
            _logger.LogInformation("Пользователь {UserLogin} пытается удалить тег. Delete", User.Identity?.Name);
            if (tagId > 0)
            {
                _logger.LogInformation("Запрос в БД. Посылаем запрос на удаление тега. Delete");

                OperationDetails result;
                try
                {
                    result = await _tagService.Delete(tagId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Delete класс TagController. tagId={id}", tagId.ToString());
                    return View("Error");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Получилось. Delete");
                    return RedirectToAction("TagList");
                }
                else
                {
                    _logger.LogWarning("Удалить не удалось. Сообщение - {Message}. Delete", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return RedirectToAction("TagList");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю. Delete");
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
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку просмотра тегов. TagList", User.Identity?.Name);
            SearchTagsViewModel searchTagsViewModel = new();
            _logger.LogInformation("Запрос в БД. Посылаем запрос на подгрузку всех тегов. TagList");

            List<TagRequest>? tagList;
            try
            {
                tagList = await _tagService.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка в методе TagList класс TagController.");
                return View("Error");
            }

            if (tagList != null)
            {
                _logger.LogInformation("Подгрузили. TagList");
                searchTagsViewModel = MyMappingTag.GetSearchTagsViewModelFromListTagRequest(tagList);
            }
            else
            {
                _logger.LogInformation("Не удалось. TagList");
            }
            return View("TagList", searchTagsViewModel);
        }
        
        [Route("TagList")]
        [HttpPost]
        public async Task<IActionResult> TagList(int Id)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается найти тег по его id={id}. TagList", User.Identity?.Name, Id.ToString());
            SearchTagsViewModel searchTagsViewModel;
            if (Id > 0)
            {
                TagRequest? tagRequest;
                _logger.LogInformation("Запрос в БД. Посылаем запрос на подгрузку тега по его id={id}. TagList", Id.ToString());

                try
                {
                    tagRequest = await _tagService.GetTagById(Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе TagList класс TagController. id={id}", Id.ToString());
                    return View("Error");
                }

                List<TagRequest> tagList = [];
                if (tagRequest != null)
                {
                    _logger.LogInformation("Подгрузили. TagList");
                    tagList.Add(tagRequest);
                    searchTagsViewModel = MyMappingTag.GetSearchTagsViewModelFromListTagRequest(tagList);
                    return View("TagList", searchTagsViewModel);
                }
                else
                {
                    _logger.LogWarning("Подгрузить не удалось. TagList");
                    return RedirectToAction("TagList");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю. TagList");
                return RedirectToAction("TagList");
            }
        }
        //------------------------------------------------
        //----------Показать страничку с тегом------------
        [Route("ShowTag")]
        [HttpPost]
        public async Task<IActionResult> ShowTag(int tagId)
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку просмотра тега. ShowTag", User.Identity?.Name);
            TagViewModel tagViewModel;
            if (tagId > 0)
            {
                TagRequest? tagRequest;
                _logger.LogInformation("Запрос в БД. Посылаем запрос на подгрузку тега по его id={id}. ShowTag", tagId.ToString());

                try
                {
                    tagRequest = await _tagService.GetTagById(tagId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе ShowTag класс TagController. tagId={id}", tagId.ToString());
                    return View("Error");
                }
                                
                if (tagRequest != null)
                {
                    _logger.LogInformation("Подгрузили. ShowTag");
                    tagViewModel = MyMappingTag.GetTagViewModelFromTagRequest(tagRequest);
                    return View("ShowTag", tagViewModel);
                }
                else
                {
                    _logger.LogWarning("Не удалось. ShowTag");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю. ShowTag");
            }
            return RedirectToAction("TagList", "Tag");
        }
        //----------------------------------------------
    }
}
