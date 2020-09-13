using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.InterfaceRepository
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetAllMessage();
        Task<Message> GetMessageByID(int? messageID);
        Task<Message> GetMessageByIDWithoutInclude(int? messageID);
        Task<Message> GetMessageByIDWithoutIncludeAndAsNoTracking(int? messageID);

        Task AddMessageAsync(Message message);
        Task DeleteMessageAsync(Message message);
        void EditMessage(Message message);
        Task SaveChangesAsync();

        IEnumerable<Message> GetAllMessageAsNoTracking();
        IEnumerable<Category> GetAllCategoryAsNoTracking();
        IEnumerable<Region> GetAllRegionAsNoTracking();
        IEnumerable<Message> GetAllMessageToUser();
    }
}
