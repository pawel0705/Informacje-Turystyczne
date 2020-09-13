using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InformacjeTurystyczne.Models.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public readonly AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            return await _appDbContext.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category> GetCategoryByID(int? categoryID)
        {
            return await _appDbContext.Categories.AsNoTracking().FirstOrDefaultAsync(s => s.IdCategory == categoryID);
        }

        public async Task<Category> GetCategoryByIDWithoutInclude(int? categoryID)
        {
            return await _appDbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.IdCategory == categoryID);
        }

        public async Task<Category> GetCategoryByIDWithoutIncludeAndAsNoTracking(int? categoryID)
        {
            return await _appDbContext.Categories.FirstOrDefaultAsync(c => c.IdCategory == categoryID);
        }

        public async Task AddCategoryAsync(Category category)
        {
            _appDbContext.Categories.Add(category);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            _appDbContext.Categories.Remove(category);
            await _appDbContext.SaveChangesAsync();
        }

        public void EditCategory(Category category)
        {
            _appDbContext.Categories.Update(category);
            _appDbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public IEnumerable<Category> GetAllCategoryAsNoTracking()
        {
            return _appDbContext.Categories.AsNoTracking();
        }

        public IEnumerable<Category> GetAllCategoryToUser()
        {
            return _appDbContext.Categories.AsNoTracking().ToList();
        }
    }
}
