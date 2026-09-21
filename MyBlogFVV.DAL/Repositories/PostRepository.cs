using Microsoft.EntityFrameworkCore;
using MyBlogFVV.DAL.EF;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.DAL.Repositories
{
    public class PostRepository : IPostRepository
    {
        private ApplicationDbContext db;

        public PostRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public async Task<List<Post>> GetAll()
        {
            List<Post> x = await db.Posts.Include(a => a.User).ToListAsync();
            return x;
        }
        public async Task<List<Post>> GetAll(int id)
        {
            List<Post> x = await db.Posts.Include(a => a.User).Where(a => a.User.UserId == id).ToListAsync();
            return x;
        }

        public async Task<Post> Get(int id) //Ищем статью по её Id
        {
            Post? x = await db.Posts.Include(u => u.User).Include(u => u.Comments).ThenInclude(u => u.User).FirstOrDefaultAsync(u => u.PostId == id);
            return x;
        }

        public async Task Create(Post post)        
        {
            await db.Posts.AddAsync(post);
        }

        public void Update(Post post)
        {
            db.Entry(post).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Post? post = db.Posts.Find(id);
            if (post != null)
                db.Posts.Remove(post);
        }
    }
}
