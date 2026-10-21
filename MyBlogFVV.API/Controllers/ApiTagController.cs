using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.API.Models.Tag;
using MyBlogFVV.API.Services;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Tag;

namespace MyBlogFVV.API.Controllers
{
    /// <summary>
    /// api по упревлению тегами
    /// </summary>
    /// <returns></returns>
    [ApiController]
    [Route("api/[controller]")]
    public class ApiTagController(ITagService tagService, ILogger<ApiTagController> logger) : Controller
    {
        readonly ITagService _tagService = tagService;
        private readonly ILogger<ApiTagController> _logger = logger;

        /// <summary>
        /// Добавление нового тега
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Сообщение об успешности или нет</returns>
        /// <response code="201">Тег успешно добавлен</response>
        /// <response code="400">Возникла ошибка</response>
        [Route("Add")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] TagModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается добавить тег. Add", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                TagRequest tagRequest = MyMappingTag.GetTagRequestFromTagModel(model);
                _logger.LogInformation("Запрос в БД. Посылаем запрос на создания тега в БД. Add");

                OperationDetails result;
                try
                {
                    result = await _tagService.Create(tagRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Add класс TagController.");
                    return StatusCode(400, $"Ошибка: Тег не удалось занести в БД");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Получилось. Add");
                    return StatusCode(201, $"Тег успешно добавлен!");
                }
                else
                {
                    _logger.LogWarning("Не удалось. Сообщение - result.Message. Add");
                    ModelState.AddModelError(result.Property, result.Message);
                    return StatusCode(400, $"Ошибка: Добавить тег в БД не удалось. Событие - {result.Message}");
                }
            }
            else
            {
                _logger.LogInformation("Некорректные данные. Add");
                ModelState.AddModelError("", "Некорректные данные");
                return StatusCode(400, $"Ошибка: Введены некорректные данные");
            }
        }

        /// <summary>
        /// Редактирование тега
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Тег успешно изменен</response>
        /// <response code="400">Возникла ошибка</response>
        [Route("Edit")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Edit([FromBody] TagEditModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается изменить тег. Edit", User.Identity?.Name);
            TagRequest? tagRequest;
            if (ModelState.IsValid)
            {
                tagRequest = MyMappingTag.GetTagRequestFromTagEditModel(model);
                _logger.LogInformation("Запрос в БД. Посылаем запрос на изменение тега. Edit");

                OperationDetails result;
                try
                {
                    result = await _tagService.Update(tagRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Edit класс TagController.");
                    return StatusCode(400, $"Ошибка: Тег не удалось изменить в БД");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Получилось. Edit");
                    return StatusCode(200, $"Тег успешно изменили!");
                }
                else
                {
                    _logger.LogWarning("Не удалось. Сообщение - {Message}. Edit", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return StatusCode(400, $"Ошибка: Тег не удалось изменить - {result.Message}");
                }
            }
            else
            {
                _logger.LogInformation("Введены некорректные данные. Edit");
                ModelState.AddModelError("", "Некорректные данные");
                return StatusCode(400, $"Ошибка: Введены некорректные данные");
            }
        }

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        /// <response code="200">Тег успешно удалили</response>
        /// <response code="400">Возникла ошибка</response>
        [HttpDelete]
        [Route("Delete/{tagId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int tagId)
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
                    return StatusCode(400, $"Ошибка: Тег не удалось удалить из БД");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Получилось. Delete");
                    return StatusCode(200, $"Тег успешно удалили!");
                }
                else
                {
                    _logger.LogWarning("Удалить не удалось. Сообщение - {Message}. Delete", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return StatusCode(400, $"Ошибка: Удалить не удалось. Сообщение - {result.Message}");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю. Delete");
                ModelState.AddModelError("Id", "Id должен быть более нуля");
                return StatusCode(400, $"Ошибка: Id должен быть более нуля");
            }
        }

        /// <summary>
        /// Посмотреть все теги
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Успешно загрузили теги</response>
        /// <response code="400">Возникла ошибка</response>
        [Route("TagList")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TagList()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку просмотра тегов. TagList", User.Identity?.Name);
            SearchTagsModel searchTagsViewModel;
            _logger.LogInformation("Запрос в БД. Посылаем запрос на подгрузку всех тегов. TagList");

            List<TagRequest>? tagList;
            try
            {
                tagList = await _tagService.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка в методе TagList класс TagController.");
                return StatusCode(400, $"Ошибка: Тег не удалось загрузить из БД");
            }

            if (tagList != null)
            {
                _logger.LogInformation("Подгрузили. TagList");
                searchTagsViewModel = MyMappingTag.GetSearchTagsModelFromListTagRequest(tagList);
                return StatusCode(200, searchTagsViewModel);
            }
            else
            {
                _logger.LogInformation("Не удалось. TagList");
                return StatusCode(400, $"Ошибка: Загрузить не удалось.");
            }
        }
    }
}
