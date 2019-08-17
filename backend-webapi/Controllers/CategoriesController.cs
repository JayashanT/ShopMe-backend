using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Dtos;
using webapi.Entities;
using webapi.Repositories;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private ICommonRepository<Category> _categoryRepository;

        public CategoriesController(ICommonRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allCategories = _categoryRepository.GetAll().ToList();
            var allCategoriesDto = allCategories.Select(x => Mapper.Map<CategoryDto>(x));
            return Ok(allCategoriesDto);
        }
    }
}

