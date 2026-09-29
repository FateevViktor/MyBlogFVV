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
    public class CommentController(ICommentService commentService, IUserService userService) : Controller
    {
        readonly ICommentService _commentService = commentService;
        readonly IUserService _userService = userService;

        public IActionResult Index()
        {
            return View();
        }
        //---------------Блок создания комментария-----------------        
        
        [Route("Add")]
        [HttpGet]
        public IActionResult Add()
        {
            CommentViewModel commentViewModel = new();
            return View(commentViewModel);
        }
        /*
        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add(CommentViewModel commentViewModel)
        {
            //CommentViewModel commentViewModel = new CommentViewModel();
            if (ModelState.IsValid)
            {
                    commentViewModel.CommentDate = DateTime.Now;

                    CommentRequest commentRequest = myMappingComment.GetCommentRequestFromCommentViewModel(commentViewModel);

                    OperationDetails result = _commentService.Create(commentRequest);
                    if (result.Succedeed == true)
                    {
                        return RedirectToAction("ShowPost", "Post", new { id = commentViewModel.Post.Id });
                    }
                    else
                    {
                        ModelState.AddModelError(result.Property, result.Message);
                    }
            }
            return RedirectToAction("ShowPost", "Post", new { id = commentViewModel.Post.Id });
        }*/
        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add(string text, string login, int postId)
        {
            CommentViewModel commentViewModel = new();
            if (login.Length>0 && postId > 0 && text.Length>0)
            {                
                //Найдем автора
                UserRequest? userRequest;
                userRequest = await _userService.GetUserByLogin(login);
                if (userRequest != null)
                {
                    commentViewModel.Text = text;
                    commentViewModel.CommentDate = DateTime.Now;
                    commentViewModel.Author.Id = userRequest.Id;
                    commentViewModel.Post.Id = postId;

                    CommentRequest commentRequest = MyMappingComment.GetCommentRequestFromCommentViewModel(commentViewModel);

                    OperationDetails result = await _commentService.Create(commentRequest);
                    if (result.Succedeed == true)
                    {
                        return RedirectToAction("ShowPost", "Post", new { id = postId });
                    }
                    else
                    {
                        ModelState.AddModelError(result.Property, result.Message);
                    }
                }
                else 
                {
                    ModelState.AddModelError("", "Автор комментария неизвестен нам");
                }
            }
            return RedirectToAction("ShowPost", "Post", new { id = postId });
        }
        //------------------------------------------------
        //--------Редактирование комментария-------------------
        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int commentId)
        {
            CommentEditViewModel commentEditViewModel;
            CommentRequest? commentRequest;
            if (commentId > 0)
            {
                commentRequest = await _commentService.GetCommentById(commentId);

                if (commentRequest != null)
                {
                    commentEditViewModel = MyMappingComment.GetCommentEditViewModelFromCommentRequest(commentRequest);
                    return View("Edit", commentEditViewModel);
                }
            }
            return RedirectToAction("CommentList");
        }
        [Route("EditUpdate")]
        [HttpPost]
        public async Task<IActionResult> EditUpdate(CommentEditViewModel model)
        {
            CommentRequest? commentRequest;
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    commentRequest = MyMappingComment.GetCommentRequestFromCommentEditViewModel(model);
                    OperationDetails result = await _commentService.Update(commentRequest);
                    if (result.Succedeed == true)
                    {
                        return RedirectToAction("CommentList");
                    }
                    else
                    {
                        ModelState.AddModelError(result.Property, result.Message);
                        return RedirectToAction("CommentList");
                    }
                }
            }
            return RedirectToAction("CommentList");
        }
        //------------------------------------------------
        //--------------Удаление комментария-------------
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int commentId)
        {
            if (commentId > 0)
            {
                OperationDetails result = await _commentService.Delete(commentId);

                if (result.Succedeed == true)
                {
                    return RedirectToAction("CommentList");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                    return RedirectToAction("CommentList");
                }
            }
            else
            {
                ModelState.AddModelError("Id", "Id должен быть более нуля");
            }
            return RedirectToAction("CommentList");
        }
        //------------------------------------------------
        //---------Список всех комментариев-------------------
        [Route("CommentList")]
        [HttpGet]
        public async Task<IActionResult> CommentList()
        {
            SearchCommentsViewModel searchCommentsViewModel = new();
            List<CommentRequest> commentList = await _commentService.GetAll();
            if (commentList != null)
            {
                searchCommentsViewModel = MyMappingComment.GetSearchCommentsViewModelFromListCommentRequest(commentList);
            }
            return View("CommentList", searchCommentsViewModel);
        }

        [Route("CommentList")]
        [HttpPost]
        public async Task<IActionResult> CommentList(int Id)
        {
            SearchCommentsViewModel searchCommentsViewModel;
            if (Id > 0)
            {
                CommentRequest? commentRequest;
                commentRequest = await _commentService.GetCommentById(Id);
                List<CommentRequest> commentList = [];
                if (commentRequest != null)
                {
                    commentList.Add(commentRequest);
                    searchCommentsViewModel = MyMappingComment.GetSearchCommentsViewModelFromListCommentRequest(commentList);
                    return View("CommentList", searchCommentsViewModel);
                }
                else
                {
                    return RedirectToAction("CommentList");
                }
            }
            else
            {
                return RedirectToAction("CommentList");
            }
        }
        //------------------------------------------------
        //----------Сприсок комментариев автора-----------------
        [Route("AuthorCommentList")]
        [HttpGet]
        public async Task<IActionResult> AuthorCommentList(int Id)
        {
            SearchCommentsViewModel searchCommentsViewModel;
            if (Id > 0)
            {
                List<CommentRequest> commentList = await _commentService.GetAllUser(Id);

                if (commentList != null)
                {
                    searchCommentsViewModel = MyMappingComment.GetSearchCommentsViewModelFromListCommentRequest(commentList);
                    return View("AuthorCommentList", searchCommentsViewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //------------------------------------------------
    }
}
