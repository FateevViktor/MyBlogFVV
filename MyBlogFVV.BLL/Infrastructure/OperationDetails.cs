
namespace MyBlogFVV.BLL.Infrastructure
{
    /// <summary>
    /// Класс хранит информацию об успешности опирации
    /// </summary>
    public class OperationDetails(bool succedeed, string message, string prop)
    {
        public bool Succedeed { get; private set; } = succedeed;
        public string Message { get; private set; } = message;
        public string Property { get; private set; } = prop;
    }
}
