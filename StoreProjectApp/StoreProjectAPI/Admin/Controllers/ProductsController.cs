using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreProjectAPI.Admin.Dtos.ProductDtos;
using Store.Core.Entities;
using Store.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using StoreProjectAPI.Helpers;

namespace StoreProjectAPI.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ProductsController(StoreDbContext context,IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        [HttpPost("")]
        public IActionResult Create([FromForm] ProductPostDto postDto)
        {
            if (!_context.Categories.Any(x => x.Id == postDto.CategoryId))
                return BadRequest(new { error = new { field = "CategoryId", message = "Catgory not found!" } });

            if (_context.Products.Any(x => x.Name == postDto.Name)) 
                return BadRequest(new { error = new { field = "Name", message = "Product already exist!" } });
        

            Product product = _mapper.Map<Product>(postDto);
            product.Image = FileManager.Save(postDto.ImageFile, _env.WebRootPath, "uploads/products");

            _context.Products.Add(product);
            _context.SaveChanges();

            return StatusCode(201, product);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Product product = _context.Products.Include(x=>x.Category).FirstOrDefault(x => x.Id == id);

            if (product == null) return NotFound();

            ProductGetDto productDto = _mapper.Map<ProductGetDto>(product);

            return Ok(productDto);
        }

        [HttpGet("")]
        public IActionResult GetAll(int page = 1)
        {
            var products = _context.Products.Skip((page - 1) * 4).Take(4).ToList();

            var productDtos = products.Select(x => new ProductListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId,
                CostPrice = x.CostPrice,
                SalePrice = x.SalePrice,
                DiscountPercent = x.DiscountPercent
            }).ToList();

            return Ok(productDtos);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromForm] ProductPostDto postDto)
        {
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);

            if (product == null) return NotFound();

            if(product.CategoryId!=postDto.CategoryId && !_context.Categories.Any(x=>x.Id == postDto.CategoryId))
                return BadRequest(new { error = new { field = "CategoryId", message = "Catgory not found!" } });


            if (product.Name!=postDto.Name && _context.Products.Any(x =>x.Id!=id && x.Name == postDto.Name))
                return BadRequest(new { error = new { field = "Name", message = "Product already exist!" } });


            product.Name = postDto.Name;
            product.CategoryId = postDto.CategoryId;
            product.CostPrice = postDto.CostPrice;
            product.SalePrice = postDto.SalePrice;
            product.DiscountPercent = postDto.DiscountPercent;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);

            if (product == null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent();
        }


    }
}
