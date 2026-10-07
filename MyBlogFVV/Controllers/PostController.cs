using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.Tag;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.WEB.Models.Post;
using MyBlogFVV.WEB.Models.Tag;
using MyBlogFVV.WEB.Models.User;
using MyBlogFVV.WEB.Services;
using System.Diagnostics;

namespace MyBlogFVV.WEB.Controllers
{
    [Route("Post")]
    //[Authorize(Policy = "OnlyForRoleAdmin")]
    [Authorize]
    public class PostController(IPostService postService, IUserService userService, IHttpContextAccessor httpContextAccessor, ITagService tagService, ILogger<PostController> logger) : Controller
    {
        readonly IPostService _postService = postService;
        readonly IUserService _userService = userService;
        readonly ITagService _tagService = tagService;
        readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<PostController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }
        //---------------Блок создания статьи-----------------
        [Route("Add")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку добавления статьи. Add.", User.Identity?.Name);
            PostAddViewModel model = new();
            string? username = User.Identity?.Name;
            UserRequest? userRequest;
            UserViewModel userViewModel = new();
            if (username != null)
            {
                _logger.LogInformation("Запрос в БД. Запрашиваем личную информацию из БД по логину {UserLogin}.", username);

                try
                {
                    userRequest = await _userService.GetUserByLogin(username);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Add класс PostController, при загрузке пользователя по логину. username={UserLogin}", username);
                    return View("Error");
                }
                
                if (userRequest != null)
                {
                    _logger.LogInformation("Пользователь найден в БД");
                    userViewModel.Id = userRequest.Id;
                    userViewModel.FirstName = userRequest.FirstName;
                    userViewModel.LastName = userRequest.LastName;
                    userViewModel.MiddleName = userRequest.MiddleName;
                    userViewModel.Email = userRequest.Email;
                    userViewModel.BirthDate = userRequest.BirthDate;

                    model.Author = userViewModel;
                    model.PostDate = DateTime.Now;
                    model.Text = "Напишите вашу статью";

                    //Подгрузим все возможные теги
                    _logger.LogInformation("Запрос в БД. Подгрузим из БД все возможные теги");

                    List<TagRequest>? tagList;
                    try
                    {
                        tagList = await _tagService.GetAll();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Произошла ошибка в методе Add класс PostController, при загрузке тегов.");
                        return View("Error");
                    }

                    List<TagCheckedViewModel> tagChecked = [];
                    if (tagList != null)
                    {
                        _logger.LogInformation("Теги загрузились успешно");
                        tagChecked = MyMappingTag.GetListTagCheckedViewModelFromListTagRequest(tagList);
                    }
                    else
                    {
                        _logger.LogInformation("Теги не подгрузились");
                    }
                    model.Tag = tagChecked;

                    ViewBag.PostAddSuccessful = false;

                    return View(model);
                }
                else
                {
                    _logger.LogWarning("Нет такого пользователя");
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.LogWarning("User.Identity?.Name почемуто равно null. Add.");
                return RedirectToAction("Index", "Home");
            }            
        }
        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add(PostAddViewModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается добавить статью. Add.", User.Identity?.Name);
            ViewBag.PostAddSuccessful = false;
            string? username = User.Identity?.Name;
            UserRequest? userRequest;
            UserViewModel userViewModel = new();
            
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
                        return View("Error");
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

                        PostRequest postRequest = MyMappingPost.GetPostRequestFromPostAddViewModel(model);

                        _logger.LogInformation("Запрос в БД. Пытаемся создать пост в БД. Add.");

                        OperationDetails result;
                        try
                        {
                            result = await _postService.Create(postRequest);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Произошла ошибка в методе Add класс PostController.");
                            return View("Error");
                        }

                        if (result.Succedeed == true)
                        {
                            _logger.LogInformation("Пост создан. Add.");
                            ViewBag.PostAddSuccessful = true;
                            return View(model);
                        }
                        else
                        {
                            _logger.LogWarning("Записать статью в БД не удалось. Сообщение - result.Message. Add.");
                            ModelState.AddModelError(result.Property, result.Message);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Подгрузить пользователя из БД не удалось. Add.");
                    }
                }
                else
                {
                    _logger.LogWarning("User.Identity?.Name равен null. Add.");
                }
            }
            else
            {
                _logger.LogInformation("Введены некорректные данные. Add.");
            }
            return View("Add", model);
        }
        //------------------------------------------------
        //--------Редактирование статьи-------------------
        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку редактирования статьи. Edit.", User.Identity?.Name);
            PostEditViewModel postEditViewModel = new();
            if (id > 0)
            {
                PostRequest? postRequest;
                _logger.LogInformation("Запрос в БД. Пытаемся запросить пост в БД по его id={id}. Edit.", id.ToString());

                try
                {
                    postRequest = await _postService.GetPostById(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Edit класс PostController. id={id}", id.ToString());
                    return View("Error");
                }
                
                if (postRequest != null)
                {
                    _logger.LogInformation("Статья подгрузилась. Edit.");
                    //Подгрузим все возможные теги
                    _logger.LogInformation("Запрос в БД. Подгрузим из БД все возможные теги");

                    List<TagRequest> tagList;
                    try
                    {
                        tagList = await _tagService.GetAll();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Произошла ошибка в методе Edit класс PostController, при загрузке тегов.");
                        return View("Error");
                    }

                    if (tagList != null)
                    {
                        _logger.LogInformation("Теги подгрузились");
                        postEditViewModel = MyMappingPost.GetPostEditViewModelFromPostRequest(postRequest, tagList);
                    }
                    else
                    {
                        _logger.LogWarning("Теги не подгрузились");
                    }
                    return View("EditUpdate", postEditViewModel);
                }
                else
                {
                    _logger.LogWarning("Статья не подгрузилась. Edit.");
                }
            }
            else
            {
                _logger.LogInformation("id не более нуля. Edit.");
            }
            return RedirectToAction("PostList", "Post");
        }
        [Route("EditUpdate")]
        [HttpPost]
        public async Task<IActionResult> EditUpdate(PostEditViewModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается изменить статью. EditUpdate.", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                PostRequest? postRequest;
                if (model != null)
                {
                    postRequest = MyMappingPost.GetPostRequestFromPostEditViewModel(model);
                    _logger.LogInformation("Запрос в БД. Пытаемся изменить статью в БД. EditUpdate.");

                    OperationDetails result;
                    try
                    {
                        result = await _postService.Update(postRequest);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Произошла ошибка в методе EditUpdate класс PostController.");
                        return View("Error");
                    }

                    if (result.Succedeed == true)
                    {
                        _logger.LogInformation("Изменить статью в БД удалось. EditUpdate.");
                        ViewBag.EditSuccessful = true;
                        return RedirectToAction("PostList", "Post");
                    }
                    else
                    {
                        _logger.LogWarning("Изменить статью не удалось. Сообщение - result.Message. EditUpdate.");
                        ModelState.AddModelError(result.Property, result.Message);
                    }
                }
                else
                {
                    _logger.LogWarning("Передаваемая модель равна null. EditUpdate.");
                }
                return View("Edit", model);
            }
            else
            {
                _logger.LogInformation("Пользователь ввел некорректные данные. EditUpdate.");
                ModelState.AddModelError("", "Некорректные данные");
                return View("EditUpdate", model);
            }
        }
        //------------------------------------------------
        //--------------Удаление статьи-------------
        [Route("Delete")]
        [HttpPost]
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
                    return View("Error");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Статью удалили успешно. Delete.");
                    return RedirectToAction("PostList", "Post");
                }
                else
                {
                    _logger.LogWarning("Статью удалить не удалось. Delete.");
                    ModelState.AddModelError(result.Property, result.Message);
                }
            }
            else
            {
                _logger.LogInformation("id не более нуля.");
                ModelState.AddModelError("Id", "Id должен быть более нуля");
            }
            return RedirectToAction("PostList", "Post");
        }
        //------------------------------------------------
        //---------Список всех статей-------------------
        [Route("PostList")]
        [HttpGet]
        public async Task<IActionResult> PostList()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку просмотра всех статей. PostList.", User.Identity?.Name);
            SearchPostsViewModel searchPostsViewModel = new();
            _logger.LogInformation("Запрос в БД. Пытаемся подгрузить все статьи в БД. PostList.");

            List<PostRequest>? postList;
            try
            {
                postList = await _postService.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка в методе PostList класс PostController.");
                return View("Error");
            }

            if (postList != null)
            {
                _logger.LogInformation("Статьи подгрузились успешно. PostList.");
                searchPostsViewModel = MyMappingPost.GetSearchPostsViewModelFromListPostRequest(postList);
            }
            else
            {
                _logger.LogInformation("Статьи подгрузить не удалось. PostList.");
            }
            return View("PostList", searchPostsViewModel);
        }

        [Route("PostList")]
        [HttpPost]
        public async Task<IActionResult> PostList(int Id)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается найти статью по ее id={id}. PostList.", User.Identity?.Name, Id.ToString());
            SearchPostsViewModel searchPostsViewModel;
            if (Id > 0)
            {
                PostRequest? postRequest;
                _logger.LogInformation("Запрос в БД. Пытаемся подгрузить статью с id={id}. PostList.", Id.ToString());

                try
                {
                    postRequest = await _postService.GetPostById(Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе PostList класс PostController. id={id}", Id.ToString());
                    return View("Error");
                }
                                
                List<PostRequest> postList = [];
                if (postRequest != null)
                {
                    _logger.LogWarning("Статья подгрузилась. PostList.");
                    postList.Add(postRequest);
                    searchPostsViewModel = MyMappingPost.GetSearchPostsViewModelFromListPostRequest(postList);
                    return View("PostList", searchPostsViewModel);
                }
                else
                {
                    _logger.LogWarning("Подгрузить статью не удалось. PostList.");
                    return RedirectToAction("PostList");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю. PostList.");
                return RedirectToAction("PostList");
            }
        }
        //------------------------------------------------
        //----------Сприсок постов автора-----------------
        [Route("AuthorPostList")]
        [HttpGet]
        public async Task<IActionResult> AuthorPostList(int Id)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается посмотреть список статей автора с id={id}. AuthorPostList.", User.Identity?.Name, Id.ToString());
            SearchPostsViewModel searchPostsViewModel;
            if (Id > 0)
            {
                _logger.LogInformation("Запрос в БД. Пытаемся подгрузить статьи автора с id={id}. AuthorPostList.", Id.ToString());

                List<PostRequest> postList;
                try
                {
                    postList = await _postService.GetAll(Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе AuthorPostList класс PostController. id={id}", Id.ToString());
                    return View("Error");
                }
                
                if (postList != null)
                {
                    _logger.LogInformation("Статьи подгрузизись. AuthorPostList.");
                    searchPostsViewModel = MyMappingPost.GetSearchPostsViewModelFromListPostRequest(postList);
                    return View("AuthorPostList", searchPostsViewModel);
                }
                else
                {
                    _logger.LogWarning("Подгрузить статьи не удалось. AuthorPostList.");
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю");
                return RedirectToAction("Index", "Home");
            }
        }
        //------------------------------------------------
        //---------Список моих статей-------------------
        [Route("MyPostList")]
        [HttpGet]
        public async Task<IActionResult> MyPostList()
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается посмотреть список своих статей. MyPostList.", User.Identity?.Name);
            SearchPostsViewModel searchPostsViewModel;
            string? username = User.Identity?.Name;
            UserRequest? userRequest;
            if (username != null)
            {
                _logger.LogInformation("Запрос в БД. Пользователь пытается подгрузить свои данные. MyPostList.");

                try
                {
                    userRequest = await _userService.GetUserByLogin(username);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе MyPostList класс PostController. username={UserLogin}", username);
                    return View("Error");
                }
                
                if (userRequest != null)
                {
                    _logger.LogInformation("Подгрузили. MyPostList.");
                    _logger.LogInformation("Запрос в БД. Пользователь пытается подгрузить свои статьи по id={id}. MyPostList.", userRequest.Id);

                    List<PostRequest>? postList;
                    try
                    {
                        postList = await _postService.GetAll(userRequest.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Произошла ошибка в методе MyPostList класс PostController. userRequest.Id={id}", userRequest.Id.ToString());
                        return View("Error");
                    }

                    if (postList != null)
                    {
                        _logger.LogInformation("Подгрузили. MyPostList.");
                        searchPostsViewModel = MyMappingPost.GetSearchPostsViewModelFromListPostRequest(postList);
                        return View("PostList", searchPostsViewModel);
                    }
                    else
                    {
                        _logger.LogInformation("Подгрузить не удалось. MyPostList.");
                    }
                }
                else
                {
                    _logger.LogWarning("Подгрузить не удалось. MyPostList.");
                }
            }
            else
            {
                _logger.LogWarning("Почемуто User.Identity?.Name равно null. MyPostList.");
            }
            return RedirectToAction("Index", "Home");
        }
        //----------------------------------------------
        //---------Показать статью----------------------
        [Route("ShowPost")]
        [HttpGet]
        public async Task<IActionResult> ShowPost(int id)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается посмотреть статью с id={id}. ShowPost.", User.Identity?.Name, id.ToString());
            PostViewModel postViewModel;
            if (id > 0)
            {
                PostRequest? postRequest;
                _logger.LogInformation("Запрос в БД. Пользователь {UserLogin} пытается загрузить статью с id={id} из БД. ShowPost.", User.Identity?.Name, id.ToString());

                try
                {
                    postRequest = await _postService.GetPostById(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе ShowPost класс PostController. id={id}", id.ToString());
                    return View("Error");
                }
                
                if (postRequest != null)
                {
                    _logger.LogInformation("Подгрузили. ShowPost.");
                    postViewModel = MyMappingPost.GetPostViewModelFromPostRequest(postRequest);
                    return View("ShowPost", postViewModel);
                }
                else
                {
                    _logger.LogWarning("Загрузить не удалось. ShowPost.");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю. ShowPost.");
            }
                return RedirectToAction("PostList", "Post");
        }
        //----------------------------------------------
    }
}
