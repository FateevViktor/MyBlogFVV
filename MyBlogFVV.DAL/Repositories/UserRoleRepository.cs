
using Microsoft.EntityFrameworkCore;
using MyBlogFVV.DAL.EF;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.DAL.Repositories
{
    internal class UserRoleRepository(ApplicationDbContext context) : IRepository<UserRole>
    {
        private readonly ApplicationDbContext db = context;

        public async Task<List<UserRole>> GetAll()
        {
            List<UserRole>? userRoles;
            userRoles = await db.UserRoles.ToListAsync();

            return userRoles;
        }

        public async Task<UserRole?> Get(int id)
        {
            UserRole? userRole;
            userRole = await db.UserRoles.FindAsync(id);
            return userRole;
        }

        public async Task Create(UserRole userRole)
        {
            await db.UserRoles.AddAsync(userRole);
        }

        public void Update(UserRole userRole)
        {
            db.Entry(userRole).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            UserRole? userRole = db.UserRoles.Find(id);
            if (userRole != null)
                db.UserRoles.Remove(userRole);
        }
    }
}
