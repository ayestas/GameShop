using GameShop.Api.DataTransferObjects;
using GameShop.Core.Entities;
using GameShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameShop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepository<Category> categoryRepository;

        public CategoriesController(IRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet(Name = "GetCategories")]
        public ActionResult<IEnumerable<CategoryListDto>> GetCategories([FromQuery] string? genre) 
        {
            if(string.IsNullOrEmpty(genre))
            {
                return Ok(categoryRepository.Get()
                    .Select(x => new CategoryListDto
                    {
                        Id = x.Id,
                        Genre = x.Genre
                    }));
            }

            var categories = categoryRepository.Filter(x => x.Genre.StartsWith(genre));
            return Ok(categories.Select(x => new CategoryListDto
            {
                Id = x.Id,
                Genre = x.Genre
            }));
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryListDto> GetCategoryById(int id)
        {
            var category = categoryRepository.GetById(id);
            if(category is null)
            {
                return BadRequest("No existe la categoría.");
            }

            return Ok(new CategoryDetailDto
            {
                Id = category.Id,
                Genre = category.Genre,
                Videogames = category.Videogames.Select(x => new VideogameDetailDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CategoryId = x.CategoryId,
                }).ToList()
            });
        }

        [HttpPost]
        public ActionResult<CategoryDetailDto> CreateCategory([FromBody]CategoryCreateDto categoryDto)
        {
            var category = new Category()
            {
                Genre = categoryDto.Genre
            };
            var newCategory = categoryRepository.Add(category);
            return Ok(new CategoryDetailDto
            {
                Id = newCategory.Id,
                Genre = newCategory.Genre,
            });
        }
    }
}
