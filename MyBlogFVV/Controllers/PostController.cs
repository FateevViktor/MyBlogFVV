using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.WEB.Models.Post;
using MyBlogFVV.WEB.Models.User;
using MyBlogFVV.WEB.Services;

namespace MyBlogFVV.WEB.Controllers
{
    [Route("Post")]
    //[Authorize(Policy = "OnlyForRoleAdmin")]
    [Authorize]
    public class PostController : Controller
    {
        MyMappingPost myMappingPost = new MyMappingPost();
        MyMapping myMapping = new MyMapping();
        IPostService _postService;
        IUserService _userService;
        IHttpContextAccessor _httpContextAccessor;
        public PostController(IPostService postService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _postService = postService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        //---------------Блок создания статьи-----------------
        [Route("Add")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            PostViewModel model = new PostViewModel();
            string? username = _httpContextAccessor.HttpContext.User.Identity.Name;
            UserRequest? userRequest = null;
            UserViewModel userViewModel = new UserViewModel();
            if (username != null)
            {
                userRequest = await _userService.GetUserByLogin(username);
                if (userRequest != null)
                {
                    userViewModel.Id = userRequest.Id;
                    userViewModel.FirstName = userRequest.FirstName;
                    userViewModel.LastName = userRequest.LastName;
                    userViewModel.MiddleName = userRequest.MiddleName;
                    userViewModel.Email = userRequest.Email;
                    userViewModel.BirthDate = userRequest.BirthDate;

                    model.Author = userViewModel;
                    model.PostDate = DateTime.Now;
                    model.Text = "Напишите вашу статью";

                    ViewBag.PostAddSuccessful = false;

                    return View(model);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }            
        }
        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add(PostViewModel model)
        {
            ViewBag.PostAddSuccessful = false;
            string? username = _httpContextAccessor.HttpContext.User.Identity.Name;
            UserRequest? userRequest = null;
            UserViewModel userViewModel = new UserViewModel();
            
            if (ModelState.IsValid)
            {
                if (username != null)
                {
                    userRequest = await _userService.GetUserByLogin(username);
                    if (userRequest != null)
                    {
                        userViewModel.Id = userRequest.Id;
                        userViewModel.FirstName = userRequest.FirstName;
                        userViewModel.LastName = userRequest.LastName;
                        userViewModel.MiddleName = userRequest.MiddleName;
                        userViewModel.Email = userRequest.Email;
                        userViewModel.BirthDate = userRequest.BirthDate;

                        model.Author = userViewModel;

                        PostRequest postRequest = myMappingPost.GetPostRequestFromPostViewModel(model);

                        OperationDetails result = await _postService.Create(postRequest);
                        if (result.Succedeed == true)
                        {
                            ViewBag.PostAddSuccessful = true;
                            return View(model);
                        }
                        else
                        {
                            ModelState.AddModelError(result.Property, result.Message);
                        }
                    }
                }                
            }
            return View("Add", model);
        }
        //------------------------------------------------
        //--------Редактирование статьи-------------------
        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id)
        {
            PostViewModel postViewModel = new PostViewModel();
            if (id > 0)
            {
                PostRequest? postRequest = null;
                postRequest = await _postService.GetPostById(id);
                if (postRequest != null)
                {
                    postViewModel = myMappingPost.GetPostViewModelFromPostRequest(postRequest);
                    return View("EditUpdate", postViewModel);
                }
            }
            return RedirectToAction("PostList", "Post");
        }
        [Route("EditUpdate")]
        [HttpPost]
        public async Task<IActionResult> EditUpdate(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                PostRequest? postRequest = null;
                if (model != null)
                {
                    postRequest = myMappingPost.GetPostRequestFromPostViewModel(model);
                    OperationDetails result = await _postService.Update(postRequest);
                    if (result.Succedeed == true)
                    {
                        ViewBag.EditSuccessful = true;
                        return RedirectToAction("PostList", "Post");
                    }
                    else
                    {
                        ModelState.AddModelError(result.Property, result.Message);
                    }
                }
                return View("Edit", model);
            }
            else
            {
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
            if (id > 0)
            {
                OperationDetails result = await _postService.Delete(id);
                if (result.Succedeed == true)
                {
                    return RedirectToAction("PostList", "Post");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                }
            }
            else
            {
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
            SearchPostsViewModel searchPostsViewModel = new SearchPostsViewModel();
            List<PostRequest>? postList = await _postService.GetAll();
            if (postList != null)
            {
                searchPostsViewModel = myMappingPost.GetSearchPostsViewModelFromListPostRequest(postList);
            }
            return View("PostList", searchPostsViewModel);
        }

        [Route("PostList")]
        [HttpPost]
        public async Task<IActionResult> PostList(int Id)
        {
            SearchPostsViewModel searchPostsViewModel = new SearchPostsViewModel();
            if (Id > 0)
            {
                PostRequest? postRequest = new PostRequest();
                postRequest = await _postService.GetPostById(Id);
                List<PostRequest> postList = new List<PostRequest>();
                if (postRequest != null)
                {
                    postList.Add(postRequest);
                    searchPostsViewModel = myMappingPost.GetSearchPostsViewModelFromListPostRequest(postList);
                    return View("PostList", searchPostsViewModel);
                }
                else
                {
                    return RedirectToAction("PostList");
                }
            }
            else
            {
                return RedirectToAction("PostList");
            }
        }
        //------------------------------------------------
        //---------Список моих статей-------------------
        [Route("MyPostList")]
        [HttpGet]
        public async Task<IActionResult> MyPostList()
        {
            SearchPostsViewModel searchPostsViewModel = new SearchPostsViewModel();
            string? username = _httpContextAccessor.HttpContext.User.Identity.Name;
            UserRequest? userRequest = null;
            if (username != null)
            {
                userRequest = await _userService.GetUserByLogin(username);
                if (userRequest != null)
                {
                    List<PostRequest>? postList = await _postService.GetAll(userRequest.Id);
                    if (postList != null)
                    {
                        searchPostsViewModel = myMappingPost.GetSearchPostsViewModelFromListPostRequest(postList);
                        return View("PostList", searchPostsViewModel);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
        //----------------------------------------------
        //---------Показать статью----------------------
        [Route("ShowPost")]
        [HttpGet]
        public async Task<IActionResult> ShowPost(int id)
        {
            PostViewModel postViewModel = new PostViewModel();
            if (id > 0)
            {
                PostRequest? postRequest = null;
                postRequest = await _postService.GetPostById(id);
                if (postRequest != null)
                {
                    postViewModel = myMappingPost.GetPostViewModelFromPostRequest(postRequest);
                    return View("ShowPost", postViewModel);
                }
            }
            return RedirectToAction("PostList", "Post");
        }
        //----------------------------------------------
    }
}
