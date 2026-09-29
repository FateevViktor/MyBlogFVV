
using Microsoft.EntityFrameworkCore;
using MyBlogFVV.DAL.EF;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.DAL.Repositories
{
    internal class RoleRepository(ApplicationDbContext context) : IRoleRepository
    {
        private readonly ApplicationDbContext db = context;

        public async Task Create(Role item)
        {
            await db.Roles.AddAsync(item);
        }

        public void Delete(Role item)
        {
            db.Roles.Remove(item);
        }

        public async Task<List<Role>> GetAll()
        {
            List<Role>? x = await db.Roles.ToListAsync();
            return x;
        }

        public async Task<Role?> GetRoleById(int idRole)
        {
            Role? x = await db.Roles.FindAsync(idRole);
            return x;
        }
        public async Task<Role?> GetRoleByText(string text)
        {
            Role? x = await db.Roles.Where(a => a.Title == text).FirstOrDefaultAsync();
            return x;
        }

        public void Update(Role item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
