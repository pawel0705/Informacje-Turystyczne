using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.Repository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategory();
        Task<Category> GetCategoryByID(int? categoryID);
        Task<Category> GetCategoryByIDWithoutInclude(int? categoryID);
        Task<Category> GetCategoryByIDWithoutIncludeAndAsNoTracking(int? categoryID);

        Task AddCategoryAsync(Category category);
        void EditCategory(Category category);
        Task DeleteCategoryAsync(Category category);

        Task SaveChangesAsync();
        IEnumerable<Category> GetAllCategoryAsNoTracking();
        IEnumerable<Category> GetAllCategoryToUser();
    }
}
