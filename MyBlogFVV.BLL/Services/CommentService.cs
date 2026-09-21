
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Comment;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.BLL.Services
{
    public class CommentService : ICommentService
    {
        IUnitOfWork Database { get; set; }
        MyMappingComment myMappingComment = new MyMappingComment();
        public CommentService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(CommentRequest commentRequest)
        {
            if (commentRequest == null)
            {
                return new OperationDetails(false, "Передаете CommentRequest равный null", "MessageOperationDetails");
            }
            else
            {
                //Ищем пользователя по Id
                User? user = await Database.Users.GetUserById(commentRequest.Author.Id);
                if (user is not null)
                {
                    //Ищем пост по Id
                    Post? post = await Database.Posts.Get(commentRequest.Post.Id);
                    if (post is not null)
                    {
                        Comment comment = myMappingComment.GetCommentFromCommentRequest(commentRequest);
                        comment.User = user;
                        comment.Post = post;
                        await Database.Comments.Create(comment);
                        await Database.Save();
                        return new OperationDetails(true, "Комментарий создан", "MessageOperationDetails");
                    }
                    else
                    {
                        return new OperationDetails(false, "Пост комментария не найден", "MessageOperationDetails");
                    }
                }
                else
                {
                    return new OperationDetails(false, "Автор комментария не найден", "MessageOperationDetails");
                }
            }
        }

        public async Task<List<CommentRequest>> GetAll()
        {
            List<CommentRequest> commentRequest = new List<CommentRequest>();
            List<Comment> comments = await Database.Comments.GetAll();
            if (comments != null)
            {
                commentRequest = myMappingComment.GetListCommentRequestFromListComment(comments);
            }
            return commentRequest;
        }

        public async Task<CommentRequest?> GetCommentById(int id)
        {
            CommentRequest? commentRequest = null;
            //Проверим, есть ли данный Id в базе
            Comment? commentСheck = await Database.Comments.GetCommentById(id);
            if (commentСheck != null)
            {
                commentRequest = myMappingComment.GetCommentRequestFromComment(commentСheck);
            }
            return commentRequest;
        }

        public async Task<OperationDetails> Update(CommentRequest commentRequest)
        {
            //Проверим, есть ли данный комментарий в базе
            Comment? commentСheck = await Database.Comments.GetCommentById(commentRequest.Id);
            if (commentСheck == null)
            {
                return new OperationDetails(false, "Комментария с таким Id не существует", "Id");
            }
            //Теперь пробежимся по тем полям, которые изменились...
            if (commentRequest.Text != commentСheck.Text) commentСheck.Text = commentRequest.Text;
            if (commentRequest.CommentDate.ToString() != commentСheck.Date) commentСheck.Date = commentRequest.CommentDate.ToString();

            Database.Comments.Update(commentСheck);
            await Database.Save();
            return new OperationDetails(true, "Редактирование комментария завершено успешно", "MessageOperationDetails");
        }

        public async Task<OperationDetails> Delete(int id)
        {
            //Проверим, есть ли данный Id в базе
            Comment? commentСheckId = await Database.Comments.GetCommentById(id);
            if (commentСheckId != null)
            {
                Database.Comments.Delete(commentСheckId);
                await Database.Save();
                return new OperationDetails(true, "Удалили ваш комментарий", "MessageOperationDetails");
            }
            else
            {
                return new OperationDetails(false, "Комментария с таким Id не найдено", "MessageOperationDetails");
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
