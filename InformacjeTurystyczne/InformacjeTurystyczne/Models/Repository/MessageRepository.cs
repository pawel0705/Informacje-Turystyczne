using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InformacjeTurystyczne.Models.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace InformacjeTurystyczne.Models.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _appDbContext;

        public MessageRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Message>> GetAllMessage()
        {
            return await _appDbContext.Messages.Include(c => c.Region).Include(d => d.Category).AsNoTracking().ToListAsync();
        }

        public async Task<Message> GetMessageByID(int? messageID)
        {
            return await _appDbContext.Messages.Include(c => c.Region).Include(d => d.Category).AsNoTracking().FirstOrDefaultAsync(s => s.IdMessage == messageID);
        }

        public async Task<Message> GetMessageByIDWithoutInclude(int? messageID)
        {
            return await _appDbContext.Messages.AsNoTracking().FirstOrDefaultAsync(c => c.IdMessage == messageID);
        }

        public async Task<Message> GetMessageByIDWithoutIncludeAndAsNoTracking(int? messageID)
        {
            return await _appDbContext.Messages.FirstOrDefaultAsync(c => c.IdMessage == messageID);
        }

        public async Task AddMessageAsync(Message message)
        {
            _appDbContext.Messages.Add(message);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(Message message)
        {
            _appDbContext.Messages.Remove(message);
            await _appDbContext.SaveChangesAsync();
        }

        public void EditMessage(Message message)
        {
            _appDbContext.Messages.Update(message);
            _appDbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public IEnumerable<Message> GetAllMessageAsNoTracking()
        {
            return _appDbContext.Messages.AsNoTracking();
        }

        public IEnumerable<Category> GetAllCategoryAsNoTracking()
        {
            return _appDbContext.Categories.AsNoTracking();
        }

        public IEnumerable<Region> GetAllRegionAsNoTracking()
        {
            return _appDbContext.Regions.AsNoTracking();
        }

        public IEnumerable<Message> GetAllMessageToUser()
        {
            return _appDbContext.Messages.Include(c => c.Region).Include(d => d.Category).AsNoTracking().ToList();
        }
    }
}
