
using Microsoft.EntityFrameworkCore;
using MyBlogFVV.DAL.EF;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private ApplicationDbContext db;

        public CommentRepository(ApplicationDbContext context)
        {
            this.db = context;
        }
        public async Task Create(Comment item)
        {
            await db.Comments.AddAsync(item);
        }

        public void Delete(Comment comment)
        {
            db.Comments.Remove(comment);
        }

        public async Task<Comment> GetCommentById(int id)
        {
            Comment? x = await db.Comments.FindAsync(id);
            return x;
        }

        public async Task<List<Comment>> GetAll()
        {
            List<Comment> x = await db.Comments.Include(a => a.Post).Include(a => a.User).ToListAsync();
            return x;
        }

        public async Task<List<Comment>> GetAllPost(int idPost)
        {
            List<Comment> x = await db.Comments.Include(a => a.Post).Include(a => a.User).Where(a => a.Post.PostId == idPost).ToListAsync();
            return x;
        }

        public async Task<List<Comment>> GetAllUser(int idUser)
        {
            List<Comment> x = await db.Comments.Include(a => a.Post).Include(a => a.User).Where(a => a.User.UserId == idUser).ToListAsync();
            return x;
        }

        public void Update(Comment item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
