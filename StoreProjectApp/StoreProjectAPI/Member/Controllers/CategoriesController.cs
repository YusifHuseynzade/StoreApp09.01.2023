using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entities;
using Store.Data.DAL;
using StoreProjectAPI.Member.Dtos.CategoryDtos;

namespace StoreProjectAPI.Member.Controllers
{
    [Route("Member/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null) return NotFound();

            CategoryGetDto categoryDto = _mapper.Map<CategoryGetDto>(category);

            return Ok(categoryDto);
        }

        [HttpGet("")]
        public IActionResult GetAll(int page = 1)
        {
            var categories = _context.Categories.Skip((page - 1) * 4).Take(4).ToList();

            var categoryDtos = _mapper.Map<List<CategoryListItemDto>>(categories);

            return Ok(categoryDtos);
        }
    }
}
