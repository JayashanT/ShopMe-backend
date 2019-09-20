using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Dtos;
using webapi.Entities;
using webapi.Repositories;

namespace webapi.Services
{
    public class CategoryService : ICategoryService
    {
        private ICommonRepository<Category> _categoryRepository;

        public CategoryService(ICommonRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<CategoryDto> GetAllCategories()
        {
            var allCategories = _categoryRepository.GetAll().OrderBy(x => x.CategoryName).ToList();
            var allCategorysDetails = allCategories.Select(x => Mapper.Map<CategoryDto>(x));
            return allCategorysDetails;
        }

        public CategoryDto GetCategoryById(int id)
        {
            Category categoryFromRepo = _categoryRepository.Get(id);
            return Mapper.Map<CategoryDto>(categoryFromRepo);
        }

        public bool DeleteCategory(int id)
        {
            var category = _categoryRepository.Get(x => x.Id == id).FirstOrDefault();

            if (category != null)
            {
                _categoryRepository.Remove(category);
                _categoryRepository.Save();
                return true;
            }
            return false;
        }
        

        public bool UpdateCategory(CategoryDto categoryDto)
        {
            var categoryinDb = _categoryRepository.Get(x => x.Id ==categoryDto.Id).FirstOrDefault();

            if (categoryinDb != null)
            {
                Category category = Mapper.Map<Category>(categoryDto);
                _categoryRepository.Update(category);
                return _categoryRepository.Save();
            }
            return false;
        }

    }
}

