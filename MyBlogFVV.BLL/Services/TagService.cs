
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Tag;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.BLL.Services
{
    public class TagService(IUnitOfWork uow) : ITagService
    {
        IUnitOfWork Database { get; set; } = uow;

        public async Task<OperationDetails> Create(TagRequest tagRequest)
        {
            if (tagRequest == null)
            {
                return new OperationDetails(false, "Передаете RegisterRequest равный null", "");
            }
            else
            {
                //Проверим, есть ли данный тег в базе
                Tag? tagСheck = await Database.Tags.GetTagByText(tagRequest.Text);
                if (tagСheck != null)
                {
                    return new OperationDetails(false, "Такой тег уже существует", "Text");
                }

                Tag tag = MyMappingTag.GetTagFromTagRequest(tagRequest);
                await Database.Tags.Create(tag);
                await Database.Save();
                return new OperationDetails(true, "Тег успешно создан", "");
            }
        }

        public async Task<OperationDetails> Delete(int id)
        {
            //Проверим, есть ли данный Id в базе
            Tag? tagСheckId = await Database.Tags.GetTagById(id);
            if (tagСheckId != null)
            {
                Database.Tags.Delete(tagСheckId);
                await Database.Save();
                return new OperationDetails(true, "Удалили тег", "MessageOperationDetails");
            }
            else
            {
                return new OperationDetails(false, "Тега с таким Id не найдено", "MessageOperationDetails");
            }
        }

        public void Dispose()
        {
            Database.Dispose();
            GC.SuppressFinalize(this); // Блокируем вызов финализатора
        }

        public async Task<List<TagRequest>> GetAll()
        {
            List<TagRequest> tagRequest = [];
            List<Tag> tags = await Database.Tags.GetAll();
            if (tags != null)
            {
                tagRequest = MyMappingTag.GetListTagRequestFromListTag(tags);
            }
            return tagRequest;
        }

        public async Task<TagRequest?> GetTagById(int id)
        {
            TagRequest? tagRequest = null;
            //Проверим, есть ли данный Id в базе
            Tag? tagСheck = await Database.Tags.GetTagById(id);
            if (tagСheck != null)
            {
                tagRequest = MyMappingTag.GetTagRequestFromTag(tagСheck);
            }
            return tagRequest;
        }

        public async Task<OperationDetails> Update(TagRequest tagRequest)
        {
            //Проверим, есть ли данный тег в базе
            Tag? tagСheck = await Database.Tags.GetTagById(tagRequest.Id);
            if (tagСheck == null)
            {
                return new OperationDetails(false, "Тега с таким Id не существует", "Id");
            }
            //Теперь пробежимся по тем полям, которые изменились...
            if (tagRequest.Text != tagСheck.Text) tagСheck.Text = tagRequest.Text;

            Database.Tags.Update(tagСheck);
            await Database.Save();
            return new OperationDetails(true, "Редактирование тега завершено успешно", "MessageOperationDetails");
        }
    }
}
