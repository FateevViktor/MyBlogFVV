
using Microsoft.EntityFrameworkCore;
using MyBlogFVV.DAL.EF;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.DAL.Repositories
{
    internal class UserRoleRepository : IRepository<UserRole>
    {
        private ApplicationDbContext db;

        public UserRoleRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public List<UserRole>? GetAll()
        {
            //myMapping = 
            List<UserRole>? userRoles = null;
            userRoles = db.UserRoles.ToList();

            return userRoles;
        }

        public UserRole? Get(int id)
        {
            UserRole? userRole = null;
            userRole = db.UserRoles.Find(id);
            return userRole;
        }

        public void Create(UserRole userRole)
        {
            db.UserRoles.Add(userRole);
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
