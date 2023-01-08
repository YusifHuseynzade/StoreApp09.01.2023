using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities;
using Store.Data;
using StoreProjectAPI.Dtos.CategoryDtos;

namespace StoreProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public CategoriesController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound();

            CategoryDetailDto detailDto = new CategoryDetailDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(detailDto);
        }

        [HttpGet("")]
        public IActionResult GetAll(int page = 1)
        {
            var categories = _context.Categories.Skip((page - 1) * 4).Take(4).ToList();
            var categoryListDto = categories.Select(x => new CategoryListItemDto { Id = x.Id, Name = x.Name });

            return Ok(categoryListDto);
        }


        [HttpPost("")]
        public IActionResult Create(CategoryPostDto categoryDto)
        {
            Category category = new Category
            {
                Name = categoryDto.Name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Created("", category);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryPostDto categoryPostDto)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound();

            category.Name = categoryPostDto.Name;
            _context.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
