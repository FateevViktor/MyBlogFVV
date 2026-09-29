
using Microsoft.EntityFrameworkCore;
using MyBlogFVV.DAL.EF;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.DAL.Repositories
{
    public class PostTagsRepository(ApplicationDbContext context) : IRepository<PostTag>
    {
        private readonly ApplicationDbContext db = context;

        public async Task Create(PostTag item)
        {
            await db.PostTags.AddAsync(item);
        }

        public void Delete(int id)
        {
            PostTag? postTag = db.PostTags.Find(id);
            if (postTag != null)
                db.PostTags.Remove(postTag);
        }

        public async Task<PostTag?> Get(int id)
        {
            PostTag? postTag;
            postTag = await db.PostTags.FindAsync(id);
            return postTag;
        }

        public async Task<List<PostTag>> GetAll()
        {
            List<PostTag>? postTags;
            postTags = await db.PostTags.ToListAsync();

            return postTags;
        }

        public void Update(PostTag item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
