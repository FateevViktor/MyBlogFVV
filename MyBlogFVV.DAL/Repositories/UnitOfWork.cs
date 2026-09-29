using MyBlogFVV.DAL.EF;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;


namespace MyBlogFVV.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;
        public UnitOfWork(ApplicationDbContext context)
        {
            db = context;
        }
        
        private IUserRepository? userRepository;
        private IPostRepository? postRepository;
        private ICommentRepository? commentRepository;
        private ITagRepository? tagRepository;
        private IRepository<PostTag>? postTagsRepository;
        private IRepository<UserRole>? userRoleRepository;
        private IRoleRepository? roleRepository;

        public IUserRepository Users
        {
            get
            {
                userRepository ??= new UserRepository(db);
                return userRepository;
            }
        }
        public ITagRepository Tags
        {
            get
            {
                tagRepository ??= new TagRepository(db);
                return tagRepository;
            }
        }
        
        public IRepository<PostTag> PostTags
        {
            get
            {
                postTagsRepository ??= new PostTagsRepository(db);
                return postTagsRepository;
            }
        }
        
        public IPostRepository Posts
        {
            get
            {
                postRepository ??= new PostRepository(db);
                return postRepository;
            }
        }

        public ICommentRepository Comments
        {
            get
            {
                commentRepository ??= new CommentRepository(db);
                return commentRepository;
            }
        }
        public IRepository<UserRole> UserRoles
        {
            get
            {
                userRoleRepository ??= new UserRoleRepository(db);
                return userRoleRepository;
            }
        }
        public IRoleRepository Roles
        {
            get
            {
                roleRepository ??= new RoleRepository(db);
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
