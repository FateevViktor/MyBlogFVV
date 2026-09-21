
using Microsoft.EntityFrameworkCore;
using MyBlogFVV.DAL.EF;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.DAL.Repositories
{
    public class TagRepository : ITagRepository
    {
        private ApplicationDbContext db;

        public TagRepository(ApplicationDbContext context)
        {
            this.db = context;
        }
        public async Task Create(Tag item)
        {
            await db.Tags.AddAsync(item);
        }

        public void Delete(Tag item)
        {
            db.Tags.Remove(item);
        }

        public async Task<List<Tag>> GetAll()
        {
            List<Tag>? x = await db.Tags.ToListAsync();
            return x;
        }

        public async Task<Tag?> GetTagById(int idTag)
        {
            Tag? x = await db.Tags.FindAsync(idTag);
            return x;
        }
        public async Task<Tag?> GetTagByText(string text)
        {
            Tag? x = await db.Tags.Where(a => a.Text == text).FirstOrDefaultAsync();
            return x;
        }

        public void Update(Tag item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
