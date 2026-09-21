
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.BLL.Services
{
    public class PostService : IPostService
    {
        IUnitOfWork Database { get; set; }
        MyMappingPost myMappingPost = new MyMappingPost();
        public PostService(IUnitOfWork uow)
        {
            Database = uow;
        }
        //Создаем статью
        public async Task<OperationDetails> Create(PostRequest postRequest)
        {
            if (postRequest == null)
            {
                return new OperationDetails(false, "Передаете PostRequest равный null", "MessageOperationDetails");
            }
            else
            {
                //Ищем пользователя по Id
                User? user = await Database.Users.GetUserById(postRequest.Author.Id);
                if (user is not null)
                {
                    Post post = myMappingPost.GetPostFromPostRequest(postRequest);
                    post.User = user;
                    await Database.Posts.Create(post);
                    await Database.Save();
                    return new OperationDetails(true, "Пост создан", "MessageOperationDetails");
                }
                else
                {
                    return new OperationDetails(false, "Автор статьи не найден", "MessageOperationDetails");
                }
            }
        }
        //получить все статьи
        public async Task<List<PostRequest>> GetAll()
        {
            List<PostRequest>? postRequest = new List<PostRequest>();
            List<Post>? posts = await Database.Posts.GetAll();
            if (posts != null)
            {
                postRequest = myMappingPost.GetListPostRequestFromListPost(posts);
            }
            return postRequest;
        }
        //получить все статьи определенного автора
        public async Task<List<PostRequest>> GetAll(int id)
        {
            List<PostRequest>? postRequest = new List<PostRequest>();
            List<Post>? posts = await Database.Posts.GetAll(id);
            if (posts != null)
            {
                postRequest = myMappingPost.GetListPostRequestFromListPost(posts);
            }
            return postRequest;
        }
        public async Task<PostRequest> GetPostById(int id) //Получить статью по Id
        {
            PostRequest? postRequest = null;
            //Проверим, есть ли данный Id в базе
            Post? postСheck = await Database.Posts.Get(id);
            if (postСheck != null)
            {
                postRequest = myMappingPost.GetPostRequestFromPost(postСheck);
            }
            return postRequest;
        }
        public async Task<OperationDetails> Update(PostRequest postRequest) //Редактировать статьи
        {
            //Проверим, есть ли данная статья в базе базе
            Post? postСheck = await Database.Posts.Get(postRequest.Id);
            if (postСheck == null)
            {
                return new OperationDetails(false, "Статьи с таким Id не существует", "Id");
            }
            //Теперь пробежимся по тем полям, которые изменились...
            if (postRequest.Text != postСheck.Text) postСheck.Text = postRequest.Text;
            if (postRequest.Title != postСheck.Title) postСheck.Title = postRequest.Title;            
            if (postRequest.PostDate.ToString() != postСheck.Date) postСheck.Date = postRequest.PostDate.ToString();

            Database.Posts.Update(postСheck);
            await Database.Save();
            return new OperationDetails(true, "Редактирование статьи завершено успешно", "MessageOperationDetails");
        }
        public async Task<OperationDetails> Delete(int id) //Удаление статьи по его Id
        {
            //Проверим, есть ли данный Id в базе
            Post? postСheckId = await Database.Posts.Get(id);
            if (postСheckId != null)
            {
                Database.Posts.Delete(postСheckId.PostId);
                await Database.Save();
                return new OperationDetails(true, "Удалили вашу статью", "MessageOperationDetails");
            }
            else
            {
                return new OperationDetails(false, "Статьи с таким Id не найдено", "MessageOperationDetails");
            }
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
