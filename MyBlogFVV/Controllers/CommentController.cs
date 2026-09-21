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
    public class CommentController : Controller
    {
        MyMappingComment myMappingComment = new MyMappingComment();
        ICommentService _commentService;
        IUserService _userService;
        public CommentController(ICommentService commentService, IUserService userService)
        {
            _commentService = commentService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //---------------Блок создания комментария-----------------        
        
        [Route("Add")]
        [HttpGet]
        public IActionResult Add()
        {
            CommentViewModel commentViewModel = new CommentViewModel();
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
            CommentViewModel commentViewModel = new CommentViewModel();
            if (login.Count()>0 && postId > 0 && text.Count()>0)
            {                
                //Найдем автора
                UserRequest? userRequest = new UserRequest();
                userRequest = await _userService.GetUserByLogin(login);
                if (userRequest != null)
                {
                    commentViewModel.Text = text;
                    commentViewModel.CommentDate = DateTime.Now;
                    commentViewModel.Author.Id = userRequest.Id;
                    commentViewModel.Post.Id = postId;

                    CommentRequest commentRequest = myMappingComment.GetCommentRequestFromCommentViewModel(commentViewModel);

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
            CommentEditViewModel commentEditViewModel = new CommentEditViewModel();
            CommentRequest? commentRequest = new CommentRequest();
            if (commentId > 0)
            {
                commentRequest = await _commentService.GetCommentById(commentId);

                if (commentRequest != null)
                {
                    commentEditViewModel = myMappingComment.GetCommentEditViewModelFromCommentRequest(commentRequest);
                    return View("Edit", commentEditViewModel);
                }
            }
            return RedirectToAction("CommentList");
        }
        [Route("EditUpdate")]
        [HttpPost]
        public async Task<IActionResult> EditUpdate(CommentEditViewModel model)
        {
            CommentRequest? commentRequest = new CommentRequest();
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    commentRequest = myMappingComment.GetCommentRequestFromCommentEditViewModel(model);
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
            SearchCommentsViewModel searchCommentsViewModel = new SearchCommentsViewModel();
            List<CommentRequest> commentList = await _commentService.GetAll();
            if (commentList != null)
            {
                searchCommentsViewModel = myMappingComment.GetSearchCommentsViewModelFromListCommentRequest(commentList);
            }
            return View("CommentList", searchCommentsViewModel);
        }

        [Route("CommentList")]
        [HttpPost]
        public async Task<IActionResult> CommentList(int Id)
        {
            SearchCommentsViewModel searchCommentsViewModel = new SearchCommentsViewModel();
            if (Id > 0)
            {
                CommentRequest? commentRequest = new CommentRequest();
                commentRequest = await _commentService.GetCommentById(Id);
                List<CommentRequest> commentList = new List<CommentRequest>();
                if (commentRequest != null)
                {
                    commentList.Add(commentRequest);
                    searchCommentsViewModel = myMappingComment.GetSearchCommentsViewModelFromListCommentRequest(commentList);
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
    }
}
