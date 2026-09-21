using MyBlogFVV.DAL.EF;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;


namespace MyBlogFVV.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        private IUserRepository? userRepository;
        private IPostRepository? postRepository;
        private ICommentRepository? commentRepository;
        private ITagRepository? tagRepository;
        //private IRepository<PostTag>? postTagRepository;
        private IRepository<UserRole>? userRoleRepository;
        private IRepository<Role>? roleRepository;

        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }
        public ITagRepository Tags
        {
            get
            {
                if (tagRepository == null)
                    tagRepository = new TagRepository(db);
                return tagRepository;
            }
        }
        /*
        public IRepository<PostTag> PostTags
        {
            get
            {
                if (postTagRepository == null)
                    postTagRepository = new PostTagRepository(db);
                return postTagRepository;
            }
        }
        */
        public IPostRepository Posts
        {
            get
            {
                if (postRepository == null)
                    postRepository = new PostRepository(db);
                return postRepository;
            }
        }

        public ICommentRepository Comments
        {
            get
            {
                if (commentRepository == null)
                    commentRepository = new CommentRepository(db);
                return commentRepository;
            }
        }
        public IRepository<UserRole> UserRoles
        {
            get
            {
                if (userRoleRepository == null)
                    userRoleRepository = new UserRoleRepository(db);
                return userRoleRepository;
            }
        }
        public IRepository<Role> Roles
        {
            get
            {
                if (roleRepository == null)
                    roleRepository = new RoleRepository(db);
                return roleRepository;
            }
        }

        public async Task Save()
        {
            await db.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
