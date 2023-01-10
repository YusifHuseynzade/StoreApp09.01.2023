using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Data.DAL;
using StoreProjectAPI.Member.Dtos.ProductDtos;

namespace StoreProjectAPI.Member.Controllers
{
    [ApiExplorerSettings(GroupName = "user")]
    [Route("Member/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Product product = _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);

            if (product == null) return NotFound();

            ProductGetDto productDto = _mapper.Map<ProductGetDto>(product);

            return Ok(productDto);
        }

        [HttpGet("")]
        public IActionResult GetAll(int page = 1)
        {
            var products = _context.Products.Skip((page - 1) * 4).Take(4).ToList();

            var productDtos = _mapper.Map<List<ProductListItemDto>>(products);

            return Ok(productDtos);
        }
    }
}
