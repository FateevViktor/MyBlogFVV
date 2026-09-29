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

namespace MyBlogFVV.WEB.Controllers
{
    [Route("Post")]
    //[Authorize(Policy = "OnlyForRoleAdmin")]
    [Authorize]
    public class PostController(IPostService postService, IUserService userService, IHttpContextAccessor httpContextAccessor, ITagService tagService) : Controller
    {
        readonly IPostService _postService = postService;
        readonly IUserService _userService = userService;
        readonly ITagService _tagService = tagService;
        readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public IActionResult Index()
        {
            return View();
        }
        //---------------Блок создания статьи-----------------
        [Route("Add")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            PostAddViewModel model = new();
            string? username = User.Identity?.Name;
            UserRequest? userRequest;
            UserViewModel userViewModel = new();
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

                    //Подгрузим все возможные теги
                    List<TagRequest>? tagList = await _tagService.GetAll();
                    List<TagCheckedViewModel> tagChecked = [];
                    if (tagList != null)
                    {
                        tagChecked = MyMappingTag.GetListTagCheckedViewModelFromListTagRequest(tagList);
                    }
                    model.Tag = tagChecked;

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
        public async Task<IActionResult> Add(PostAddViewModel model)
        {
            ViewBag.PostAddSuccessful = false;
            string? username = User.Identity?.Name;
            UserRequest? userRequest;
            UserViewModel userViewModel = new();
            
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

                        PostRequest postRequest = MyMappingPost.GetPostRequestFromPostAddViewModel(model);

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
            PostEditViewModel postEditViewModel = new();
            if (id > 0)
            {
                PostRequest? postRequest;
                postRequest = await _postService.GetPostById(id);
                if (postRequest != null)
                {
                    //Подгрузим все возможные теги
                    List<TagRequest> tagList = await _tagService.GetAll();
                    if (tagList != null)
                    {
                        postEditViewModel = MyMappingPost.GetPostEditViewModelFromPostRequest(postRequest, tagList);
                    }                    
                    return View("EditUpdate", postEditViewModel);
                }
            }
            return RedirectToAction("PostList", "Post");
        }
        [Route("EditUpdate")]
        [HttpPost]
        public async Task<IActionResult> EditUpdate(PostEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                PostRequest? postRequest;
                if (model != null)
                {
                    postRequest = MyMappingPost.GetPostRequestFromPostEditViewModel(model);
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
            SearchPostsViewModel searchPostsViewModel = new();
            List<PostRequest>? postList = await _postService.GetAll();
            if (postList != null)
            {
                searchPostsViewModel = MyMappingPost.GetSearchPostsViewModelFromListPostRequest(postList);
            }
            return View("PostList", searchPostsViewModel);
        }

        [Route("PostList")]
        [HttpPost]
        public async Task<IActionResult> PostList(int Id)
        {
            SearchPostsViewModel searchPostsViewModel;
            if (Id > 0)
            {
                PostRequest? postRequest;
                postRequest = await _postService.GetPostById(Id);
                List<PostRequest> postList = [];
                if (postRequest != null)
                {
                    postList.Add(postRequest);
                    searchPostsViewModel = MyMappingPost.GetSearchPostsViewModelFromListPostRequest(postList);
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
        //----------Сприсок постов автора-----------------
        [Route("AuthorPostList")]
        [HttpGet]
        public async Task<IActionResult> AuthorPostList(int Id)
        {
            SearchPostsViewModel searchPostsViewModel;
            if (Id > 0)
            {
                List<PostRequest> postList = await _postService.GetAll(Id);
                
                if (postList != null)
                {
                    searchPostsViewModel = MyMappingPost.GetSearchPostsViewModelFromListPostRequest(postList);
                    return View("AuthorPostList", searchPostsViewModel);
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
        //---------Список моих статей-------------------
        [Route("MyPostList")]
        [HttpGet]
        public async Task<IActionResult> MyPostList()
        {
            SearchPostsViewModel searchPostsViewModel;
            string? username = User.Identity?.Name;
            UserRequest? userRequest;
            if (username != null)
            {
                userRequest = await _userService.GetUserByLogin(username);
                if (userRequest != null)
                {
                    List<PostRequest>? postList = await _postService.GetAll(userRequest.Id);
                    if (postList != null)
                    {
                        searchPostsViewModel = MyMappingPost.GetSearchPostsViewModelFromListPostRequest(postList);
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
            PostViewModel postViewModel;
            if (id > 0)
            {
                PostRequest? postRequest;
                postRequest = await _postService.GetPostById(id);
                if (postRequest != null)
                {
                    postViewModel = MyMappingPost.GetPostViewModelFromPostRequest(postRequest);
                    return View("ShowPost", postViewModel);
                }
            }
            return RedirectToAction("PostList", "Post");
        }
        //----------------------------------------------
    }
}
