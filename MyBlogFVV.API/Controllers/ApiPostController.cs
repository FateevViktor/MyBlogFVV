using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.API.Models.Post;
using MyBlogFVV.API.Models.User;
using MyBlogFVV.API.Services;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.User;

namespace MyBlogFVV.API.Controllers
{
    /// <summary>
    /// api по упревлению статьями
    /// </summary>
    /// <returns></returns>
    [ApiController]
    [Route("api/[controller]")]
    public class ApiPostController(IPostService postService, IUserService userService, IHttpContextAccessor httpContextAccessor, ITagService tagService, ILogger<ApiPostController> logger) : Controller
    {
        readonly IPostService _postService = postService;
        readonly IUserService _userService = userService;
        readonly ITagService _tagService = tagService;
        readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<ApiPostController> _logger = logger;

        /// <summary>
        /// Добавление новой статьи
        /// </summary>
        /// <param name="modelApi"></param>
        /// <returns>Сообщение об успешности или нет</returns>
        /// <response code="201">Статья успешно добавлена</response>
        /// <response code="400">Возникла ошибка</response>
        [Route("Add")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] PostAddApiModel modelApi)
        {
            PostAddModel model = new PostAddModel();
            _logger.LogInformation("Пользователь {UserLogin} пытается добавить статью. Add.", User.Identity?.Name);

            string? username = modelApi.AuthorLogin;
            UserRequest? userRequest;
            UserModel userViewModel = new();

            if (ModelState.IsValid)
            {
                if (username != null)
                {
                    _logger.LogInformation("Запрос в БД. Подгрузим из БД пользователя по его логину. Add.");

                    try
                    {
                        userRequest = await _userService.GetUserByLogin(username);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Произошла ошибка в методе Add класс PostController. username={UserLogin}.", username);
                        return StatusCode(400, $"Ошибка: Нет такого пользователя по логину {username}");
                    }

                    if (userRequest != null)
                    {
                        _logger.LogInformation("Успешно подгрузили пользователя. Add.");
                        userViewModel.Id = userRequest.Id;
                        userViewModel.FirstName = userRequest.FirstName;
                        userViewModel.LastName = userRequest.LastName;
                        userViewModel.MiddleName = userRequest.MiddleName;
                        userViewModel.Email = userRequest.Email;
                        userViewModel.BirthDate = userRequest.BirthDate;

                        model.Author = userViewModel;
                        model.Title = modelApi.Title;
                        model.Text = modelApi.Text;
                        model.Summary = modelApi.Summary;
                        model.PostDate = DateTime.Now;
                        model.Tag = modelApi.Tag;

                        PostRequest postRequest = MyMappingPost.GetPostRequestFromPostAddModel(model);

                        _logger.LogInformation("Запрос в БД. Пытаемся создать пост в БД. Add.");

                        OperationDetails result;
                        try
                        {
                            result = await _postService.Create(postRequest);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Произошла ошибка в методе Add класс PostController.");
                            return StatusCode(400, $"Ошибка: Статью не удалось занести в БД");
                        }

                        if (result.Succedeed == true)
                        {
                            _logger.LogInformation("Пост создан. Add.");
                            ViewBag.PostAddSuccessful = true;
                            return StatusCode(201, $"Статья успешно добавлена!");
                        }
                        else
                        {
                            _logger.LogWarning("Записать статью в БД не удалось. Сообщение - result.Message. Add.");
                            ModelState.AddModelError(result.Property, result.Message);
                            return StatusCode(400, $"Ошибка: Добавить статью в БД не удалось. Сообщение - {result.Message}");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Подгрузить пользователя из БД не удалось. Add.");
                        return StatusCode(400, $"Ошибка: Подгрузить пользователя из БД не удалось");
                    }
                }
                else
                {
                    _logger.LogWarning("User.Identity?.Name равен null. Add.");
                    return StatusCode(400, $"Ошибка: User.Identity?.Name равен null");
                }
            }
            else
            {
                _logger.LogInformation("Введены некорректные данные. Add.");
                return StatusCode(400, $"Ошибка: Введены некорректные данные");
            }
        }

        /// <summary>
        /// Редактирование статьи
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Статья успешно изменена</response>
        /// <response code="400">Возникла ошибка</response>
        [Route("Edit")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditUpdate([FromBody] PostEditModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается изменить статью. Edit.", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                PostRequest? postRequest;
                if (model != null)
                {
                    postRequest = MyMappingPost.GetPostRequestFromPostEditModel(model);
                    _logger.LogInformation("Запрос в БД. Пытаемся изменить статью в БД. Edit.");

                    OperationDetails result;
                    try
                    {
                        result = await _postService.Update(postRequest);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Произошла ошибка в методе Edit класс PostController.");
                        return StatusCode(400, $"Ошибка: Статью не удалось изменить в БД");
                    }

                    if (result.Succedeed == true)
                    {
                        _logger.LogInformation("Изменить статью в БД удалось. Edit.");
                        ViewBag.EditSuccessful = true;
                        return StatusCode(201, $"Статья успешно изменена!");
                    }
                    else
                    {
                        _logger.LogWarning("Изменить статью не удалось. Сообщение - result.Message. Edit.");
                        ModelState.AddModelError(result.Property, result.Message);
                        return StatusCode(400, $"Ошибка: Статью не удалось изменить - {result.Message}");
                    }
                }
                else
                {
                    _logger.LogWarning("Передаваемая модель равна null. Edit.");
                    return StatusCode(400, $"Ошибка: Передаваемая модель равна null");
                }
            }
            else
            {
                _logger.LogInformation("Пользователь ввел некорректные данные. Edit.");
                ModelState.AddModelError("", "Некорректные данные");
                return StatusCode(400, $"Ошибка: Пользователь ввел некорректные данные");
            }
        }

        /// <summary>
        /// Удаление статьи
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Статью успешно удалили</response>
        /// <response code="400">Возникла ошибка</response>
        [HttpDelete]
        [Route("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается удалить статью. Delete.", User.Identity?.Name);
            if (id > 0)
            {
                _logger.LogInformation("Запрос в БД. Пытаемся удалить статью в БД. Delete.");

                OperationDetails result;
                try
                {
                    result = await _postService.Delete(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Delete класс PostController. id={id}", id.ToString());
                    return StatusCode(400, $"Ошибка: Статью не удалось удалить из БД");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Статью удалили успешно. Delete.");
                    return StatusCode(200, $"Статью успешно удалили!");
                }
                else
                {
                    _logger.LogWarning("Статью удалить не удалось. Delete.");
                    ModelState.AddModelError(result.Property, result.Message);
                    return StatusCode(400, $"Ошибка: Удалить не удалось. Сообщение - {result.Message}");
                }
            }
            else
            {
                _logger.LogInformation("id не более нуля.");
                ModelState.AddModelError("Id", "Id должен быть более нуля");
                return StatusCode(400, $"Ошибка: Id должен быть более нуля");
            }
        }

        /// <summary>
        /// Посмотреть все статьи
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Успешно загрузили статьи</response>
        /// <response code="400">Возникла ошибка</response>
        [Route("PostList")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostList()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку просмотра всех статей. PostList.", User.Identity?.Name);
            SearchPostsModel searchPostsViewModel = new();
            _logger.LogInformation("Запрос в БД. Пытаемся подгрузить все статьи в БД. PostList.");

            List<PostRequest>? postList;
            try
            {
                postList = await _postService.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка в методе PostList класс PostController.");
                return StatusCode(400, $"Ошибка: Статьи не удалось загрузить из БД");
            }

            if (postList != null)
            {
                _logger.LogInformation("Статьи подгрузились успешно. PostList.");
                searchPostsViewModel = MyMappingPost.GetSearchPostsModelFromListPostRequest(postList);
                return StatusCode(200, searchPostsViewModel);
            }
            else
            {
                _logger.LogInformation("Статьи подгрузить не удалось. PostList.");
                return StatusCode(400, $"Ошибка: Загрузить не удалось.");
            }
        }
    }
}
