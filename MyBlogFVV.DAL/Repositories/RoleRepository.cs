
using Microsoft.EntityFrameworkCore;
using MyBlogFVV.DAL.EF;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.DAL.Repositories
{
    internal class RoleRepository : IRepository<Role>
    {
        private ApplicationDbContext db;

        public RoleRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public List<Role>? GetAll()
        {
            List<Role>? x = db.Roles.ToList();
            return x;
        }

        public Role? Get(int id)
        {
            return db.Roles.Find(id);
        }

        public void Create(Role role)
        {
            db.Roles.Add(role);
        }

        public void Update(Role role)
        {
            db.Entry(role).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Role? role = db.Roles.Find(id);
            if (role != null)
                db.Roles.Remove(role);
        }
    }
}
