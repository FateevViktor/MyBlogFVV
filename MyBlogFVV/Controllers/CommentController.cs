using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Comment;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.WEB.Models.Comment;
using MyBlogFVV.WEB.Services;


namespace MyBlogFVV.WEB.Controllers
{
    [Route("Comment")]
    [Authorize]
    //[Authorize(Policy = "OnlyForRoleAdmin")]
    public class CommentController(ICommentService commentService, IUserService userService, ILogger<CommentController> logger) : Controller
    {
        readonly ICommentService _commentService = commentService;
        readonly IUserService _userService = userService;
        private readonly ILogger<CommentController> _logger = logger;
        public IActionResult Index()
        {
            return View();
        }
        //---------------Блок создания комментария-----------------        
        
        [Route("Add")]
        [HttpGet]
        public IActionResult Add()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку добавления комментария Add", User.Identity?.Name);
            CommentViewModel commentViewModel = new();
            return View(commentViewModel);
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add(string text, string login, int postId)
        {
            CommentViewModel commentViewModel = new();
            _logger.LogInformation("Пользователь {UserLogin} пытается добавить комментарий", User.Identity?.Name);
            if (login.Length>0 && postId > 0 && String.IsNullOrEmpty(text)==false)
            {                
                //Найдем автора
                UserRequest? userRequest;
                _logger.LogInformation("Запрашиваем личную информацию из БД по пользователю с логином {UserLogin}.", login);
                
                try
                {
                    userRequest = await _userService.GetUserByLogin(login);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Add класс CommentController. login={UserLogin}", login);
                    return View("Error");
                }

                if (userRequest != null)
                {
                    commentViewModel.Text = text;
                    commentViewModel.CommentDate = DateTime.Now;
                    commentViewModel.Author.Id = userRequest.Id;
                    commentViewModel.Post.Id = postId;

                    CommentRequest commentRequest = MyMappingComment.GetCommentRequestFromCommentViewModel(commentViewModel);
                    _logger.LogInformation("Запрос в БД. Пользователь {UserLogin} пытается добавить комментарий", User.Identity?.Name);

                    OperationDetails result;
                    try
                    {
                        result = await _commentService.Create(commentRequest);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Произошла ошибка в методе Add класс CommentController.");
                        return View("Error");
                    }

                    if (result.Succedeed == true)
                    {
                        _logger.LogInformation("Комментарий добавлен в БД.");
                        return RedirectToAction("ShowPost", "Post", new { id = postId });
                    }
                    else
                    {
                        _logger.LogInformation("Не удалось добавить комментарий в БД. Сообщение - {Message}", result.Message);
                        ModelState.AddModelError(result.Property, result.Message);
                    }
                }
                else 
                {
                    _logger.LogWarning("Автор комментария неизвестен нам.");
                    ModelState.AddModelError("", "Автор комментария неизвестен нам");
                }
            }
            else
            {
                _logger.LogInformation("Пользователь {UserLogin} не написал текст комментария", User.Identity?.Name);
                ModelState.AddModelError("", "Текст должен быть заполнен");
            }
            return RedirectToAction("ShowPost", "Post", new { id = postId });
        }
        //------------------------------------------------
        //--------Редактирование комментария-------------------
        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int commentId)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается отредактировать комментарий с id={id} Edit", User.Identity?.Name, commentId.ToString());
            CommentEditViewModel commentEditViewModel;
            CommentRequest? commentRequest;
            if (commentId > 0)
            {
                _logger.LogInformation("Запрос в БД. Запрашиваем комментарий по id={id}.", commentId);

                try
                {
                    commentRequest = await _commentService.GetCommentById(commentId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Edit класс CommentController. commentId={id}", commentId.ToString());
                    return View("Error");
                }

                if (commentRequest != null)
                {
                    _logger.LogInformation("Комментарий нашли");
                    commentEditViewModel = MyMappingComment.GetCommentEditViewModelFromCommentRequest(commentRequest);
                    return View("Edit", commentEditViewModel);
                }
                else
                {
                    _logger.LogWarning("Запрашиваемый комментарий с id={id} не найден в БД.", commentId);
                }
            }
            else
            {
                _logger.LogInformation("id не больше 0 Edit");
            }
            return RedirectToAction("CommentList");
        }
        [Route("EditUpdate")]
        [HttpPost]
        public async Task<IActionResult> EditUpdate(CommentEditViewModel model)
        {
            CommentRequest? commentRequest;
            _logger.LogInformation("Пользователь {UserLogin} пытается отредактировать комментарий передавая модель. EditUpdate", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    commentRequest = MyMappingComment.GetCommentRequestFromCommentEditViewModel(model);
                    _logger.LogInformation("Запрос в БД. Пытаемся изменить комментарий.");
                    OperationDetails result;
                    try
                    {
                        result = await _commentService.Update(commentRequest);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Произошла ошибка в методе EditUpdate класс CommentController.");
                        return View("Error");
                    }

                    if (result.Succedeed == true)
                    {
                        _logger.LogInformation("Комментарий успешно изменили.");
                        return RedirectToAction("CommentList");
                    }
                    else
                    {
                        _logger.LogInformation("Комментарий изменить не удалось. Сообщение - {Message}.", result.Message);
                        ModelState.AddModelError(result.Property, result.Message);
                        return RedirectToAction("CommentList");
                    }
                }
            }
            else
            {
                _logger.LogInformation("Пользователь передал некорректные данные. EditUpdate");
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
            return RedirectToAction("CommentList");
        }
        //------------------------------------------------
        //--------------Удаление комментария-------------
        [Route("Delete")]
        [HttpPost]
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
                    return View("Error");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Комментарий удалили успешно.");
                    return RedirectToAction("CommentList");
                }
                else
                {
                    _logger.LogInformation("Удалить комментарий не удалось. Сообщение - {Message}", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return RedirectToAction("CommentList");
                }
            }
            else
            {
                _logger.LogInformation("id не больше 0. Delete");
                ModelState.AddModelError("Id", "Id должен быть более нуля");
            }
            return RedirectToAction("CommentList");
        }
        //----------------------------------------------------
        //---------Список всех комментариев-------------------
        [Route("CommentList")]
        [HttpGet]
        public async Task<IActionResult> CommentList()
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается посмотреть все комментарии. CommentList", User.Identity?.Name);
            SearchCommentsViewModel searchCommentsViewModel = new();
            List<CommentRequest> commentList;
            
            try
            {
                commentList = await _commentService.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка в методе CommentList класс CommentController.");
                return View("Error");
            }

            if (commentList != null)
            {
                _logger.LogInformation("Комментарии в БД найдены. CommentList");
                searchCommentsViewModel = MyMappingComment.GetSearchCommentsViewModelFromListCommentRequest(commentList);
            }
            else
            {
                _logger.LogWarning("Комментарии в БД не найдены. CommentList");
            }
            return View("CommentList", searchCommentsViewModel);
        }

        [Route("CommentList")]
        [HttpPost]
        public async Task<IActionResult> CommentList(int Id)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается найти комментарий с id={id}. CommentList", User.Identity?.Name, Id.ToString());
            SearchCommentsViewModel searchCommentsViewModel;
            if (Id > 0)
            {
                CommentRequest? commentRequest;
                _logger.LogInformation("Запрос в БД. Хотим получить комментарий по id={id}. CommentList", Id.ToString());

                try
                {
                    commentRequest = await _commentService.GetCommentById(Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе CommentList класс CommentController. Id={id}", Id.ToString());
                    return View("Error");
                }
                
                List<CommentRequest> commentList = [];
                if (commentRequest != null)
                {
                    _logger.LogInformation("Комментарий найден. CommentList");
                    commentList.Add(commentRequest);
                    searchCommentsViewModel = MyMappingComment.GetSearchCommentsViewModelFromListCommentRequest(commentList);
                    return View("CommentList", searchCommentsViewModel);
                }
                else
                {
                    _logger.LogInformation("Комментарий не найден. CommentList");
                    return RedirectToAction("CommentList");
                }
            }
            else
            {
                _logger.LogInformation("id не более 0. CommentList");
                ModelState.AddModelError("", "Id должен быть больше 0");
                return RedirectToAction("CommentList");
            }
        }
        //------------------------------------------------
        //----------Сприсок комментариев автора-----------------
        [Route("AuthorCommentList")]
        [HttpGet]
        public async Task<IActionResult> AuthorCommentList(int Id)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается посмотреть комментарии автора с id={id}. AuthorCommentList", User.Identity?.Name, Id.ToString());
            SearchCommentsViewModel searchCommentsViewModel;
            if (Id > 0)
            {
                List<CommentRequest> commentList;
                try
                {
                    commentList = await _commentService.GetAllUser(Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе AuthorCommentList класс CommentController. Id={id}", Id.ToString());
                    return View("Error");
                }

                if (commentList != null)
                {
                    _logger.LogInformation("Комментарии в БД автора найдены. AuthorCommentList");
                    searchCommentsViewModel = MyMappingComment.GetSearchCommentsViewModelFromListCommentRequest(commentList);
                    return View("AuthorCommentList", searchCommentsViewModel);
                }
                else
                {
                    _logger.LogInformation("Комментарии автора не найдены. AuthorCommentList");
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                _logger.LogInformation("id не более 0. AuthorCommentList");
                return RedirectToAction("Index", "Home");
            }
        }
        //------------------------------------------------
    }
}
