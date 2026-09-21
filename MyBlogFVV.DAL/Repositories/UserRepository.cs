using Microsoft.EntityFrameworkCore;
using MyBlogFVV.DAL.EF;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext db;
        public UserRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public async Task<List<User>> GetAll()
        {
            return await db.Users.ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await db.Users.FindAsync(id);
        }
        public async Task<User?> GetUserByEmail(string item)
        {
            User? user = await db.Users.Include(role => role.UserRoles).ThenInclude(role => role.Role).FirstOrDefaultAsync(p => p.Email == item); //работает
            return user;
        }
        public async Task<User?> GetUserByLogin(string item)
        {
            User? user = await db.Users.Include(role => role.UserRoles).ThenInclude(role => role.Role).FirstOrDefaultAsync(p => p.Login == item); //работает
            return user;
        }
        public async Task<User?> GetUserByLoginAndPassword(string login, string password)
        {
            User? user = await db.Users.Include(role => role.UserRoles).ThenInclude(role => role.Role).FirstOrDefaultAsync(p => p.Login == login && p.Password == password); //работает
            return user;
        }
        public async Task Create(User user)
        {
            var foundRole = db.Roles.SingleOrDefault(role => role.Text == "User"); //вернет один элемент или null
            if (foundRole == null)
            {
                throw new Exception(" Роль User не найдена ");
            }
            user.UserRoles = new List<UserRole>
            {
                new UserRole
                {
                    User = user,
                    Role = foundRole
                }
            };
            await db.Users.AddAsync(user);                
        }

        public void Update(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public void Delete(User user)
        {
            /*
            User? user = db.Users.Find(id);
            if (user != null)
                db.Users.Remove(user);*/
            db.Users.Remove(user);
        }
    }
}
