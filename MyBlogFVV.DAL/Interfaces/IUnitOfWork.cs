using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IPostRepository Posts { get; }
        ICommentRepository Comments { get; }
        ITagRepository Tags { get; }
        //IRepository<PostTag> PostTags { get; }
        IRepository<UserRole> UserRoles { get; }
        IRepository<Role> Roles { get; }
        Task Save();
    }
}
