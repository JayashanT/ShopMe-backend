using System.Collections.Generic;
using webapi.Dtos;

namespace webapi.Services
{
    public interface ICategoryService
    {
        bool DeleteCategory(int id);
        IEnumerable<CategoryDto> GetAllCategories();
        CategoryDto GetCategoryById(int id);
        bool UpdateCategory(CategoryDto categoryDto);
    }
}