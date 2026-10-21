using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.API.Models.Comment;
using MyBlogFVV.API.Services;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Comment;
using MyBlogFVV.BLL.Models.User;

namespace MyBlogFVV.API.Controllers
{
    /// <summary>
    /// api по упревлению комментариями
    /// </summary>
    /// <returns></returns>
    /// [ApiController]
    [Route("api/[controller]")]
    public class ApiCommentController(ICommentService commentService, IUserService userService, ILogger<ApiCommentController> logger) : Controller
    {
        readonly ICommentService _commentService = commentService;
        readonly IUserService _userService = userService;
        private readonly ILogger<ApiCommentController> _logger = logger;

        /// <summary>
        /// Добавление нового комментария
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Сообщение об успешности или нет</returns>
        /// <response code="201">Комментарий успешно добавлен</response>
        /// <response code="400">Возникла ошибка</response>
        [Route("Add")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(CommentAddModel model)
        {
            CommentModel commentViewModel = new();
            _logger.LogInformation("Пользователь {UserLogin} пытается добавить комментарий", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                //Найдем автора
                UserRequest? userRequest;
                _logger.LogInformation("Запрашиваем личную информацию из БД по пользователю с логином {UserLogin}.", model.AuthorLogin);

                try
                {
                    userRequest = await _userService.GetUserByLogin(model.AuthorLogin);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Add класс CommentController. login={UserLogin}", model.AuthorLogin);
                    return StatusCode(400, $"Ошибка: Автора статьи не удалось найти по его логину");
                }

                if (userRequest != null)
                {
                    commentViewModel.Text = model.CommentText;
                    commentViewModel.CommentDate = DateTime.Now;
                    commentViewModel.Author.Id = userRequest.Id;
                    commentViewModel.Post.Id = model.PostId;

                    CommentRequest commentRequest = MyMappingComment.GetCommentRequestFromCommentModel(commentViewModel);
                    _logger.LogInformation("Запрос в БД. Пользователь {UserLogin} пытается добавить комментарий", User.Identity?.Name);

                    OperationDetails result;
                    try
                    {
                        result = await _commentService.Create(commentRequest);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Произошла ошибка в методе Add класс CommentController.");
                        return StatusCode(400, $"Ошибка: Комментарий не удалось занести в БД");
                    }

                    if (result.Succedeed == true)
                    {
                        _logger.LogInformation("Комментарий добавлен в БД.");
                        return StatusCode(201, $"Комментарий успешно добавлен!");
                    }
                    else
                    {
                        _logger.LogInformation("Не удалось добавить комментарий в БД. Сообщение - {Message}", result.Message);
                        ModelState.AddModelError(result.Property, result.Message);
                        return StatusCode(400, $"Ошибка: Добавить комментарий в БД не удалось. Событие - {result.Message}");
                    }
                }
                else
                {
                    _logger.LogWarning("Автор комментария неизвестен нам.");
                    ModelState.AddModelError("", "Автор комментария неизвестен нам");
                    return StatusCode(400, $"Ошибка: Автор комментария неизвестен нам");
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
        /// Редактирование комментария
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Комментарий успешно изменен</response>
        /// <response code="400">Возникла ошибка</response>
        [Route("Edit")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditUpdate([FromBody] CommentEditModel model)
        {
            CommentRequest? commentRequest;
            _logger.LogInformation("Пользователь {UserLogin} пытается отредактировать комментарий передавая модель. Edit", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    commentRequest = MyMappingComment.GetCommentRequestFromCommentEditModel(model);
                    _logger.LogInformation("Запрос в БД. Пытаемся изменить комментарий.");
                    OperationDetails result;
                    try
                    {
                        result = await _commentService.Update(commentRequest);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Произошла ошибка в методе Edit класс CommentController.");
                        return StatusCode(400, $"Ошибка: Комментарий не удалось изменить в БД");
                    }

                    if (result.Succedeed == true)
                    {
                        _logger.LogInformation("Комментарий успешно изменили.");
                        return StatusCode(200, $"Комментарий успешно изменили!");
                    }
                    else
                    {
                        _logger.LogInformation("Комментарий изменить не удалось. Сообщение - {Message}.", result.Message);
                        ModelState.AddModelError(result.Property, result.Message);
                        return StatusCode(400, $"Ошибка: Комментарий не удалось изменить - {result.Message}");
                    }
                }
                else
                {
                    return StatusCode(400, $"Ошибка: Передали модель равной null");
                }

            }
            else
            {
                _logger.LogInformation("Пользователь передал некорректные данные. EditUpdate");
                ModelState.AddModelError("", "Некорректные данные");
                return StatusCode(400, $"Ошибка: Введены некорректные данные");
            }
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        /// <response code="200">Комментарий успешно удалили</response>
        /// <response code="400">Возникла ошибка</response>
        [HttpDelete]
        [Route("Delete/{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int commentId)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается удалить комментарий с id={id}. Delete", User.Identity?.Name, commentId.ToString());
            if (commentId > 0)
            {
                _logger.LogInformation("Запрос в БД. Пытаемся удалить комментарий.");
                OperationDetails result;
                try
                {
                    result = await _commentService.Delete(commentId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Delete класс CommentController.");
                    return StatusCode(400, $"Ошибка: Комментарий не удалось удалить из БД");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Комментарий удалили успешно.");
                    return StatusCode(200, $"Комментарий успешно удалили!");
                }
                else
                {
                    _logger.LogInformation("Удалить комментарий не удалось. Сообщение - {Message}", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return StatusCode(400, $"Ошибка: Удалить не удалось. Сообщение - {result.Message}");
                }
            }
            else
            {
                _logger.LogInformation("id не больше 0. Delete");
                ModelState.AddModelError("Id", "Id должен быть более нуля");
                return StatusCode(400, $"Ошибка: Id должен быть более нуля");
            }
        }

        /// <summary>
        /// Посмотреть все комментарии
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Успешно загрузили комментарии</response>
        /// <response code="400">Возникла ошибка</response>
        [Route("CommentList")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CommentList()
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается посмотреть все комментарии. CommentList", User.Identity?.Name);
            SearchCommentsModel searchCommentsViewModel = new();
            List<CommentRequest> commentList;

            try
            {
                commentList = await _commentService.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка в методе CommentList класс CommentController.");
                return StatusCode(400, $"Ошибка: Комментарии не удалось загрузить из БД");
            }

            if (commentList != null)
            {
                _logger.LogInformation("Комментарии в БД найдены. CommentList");
                searchCommentsViewModel = MyMappingComment.GetSearchCommentsModelFromListCommentRequest(commentList);
                return StatusCode(200, searchCommentsViewModel);
            }
            else
            {
                _logger.LogWarning("Комментарии в БД не найдены. CommentList");
            }
            return StatusCode(400, $"Ошибка: Загрузить не удалось.");
        }
    }
}
