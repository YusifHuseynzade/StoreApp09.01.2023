using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Entities;
using StoreProjectAPI.Dtos.CategoryDtos;
using StoreProjectAPI.Dtos.ProductDtos;

namespace StoreProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public ProductsController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            Product product = _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            ProductDetailDto productDto = new ProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                SalePrice = product.SalePrice,
                DiscountPercent = product.DiscountPercent,
                Category = new CategoryInProductDetailDto
                {
                    Id = product.CategoryId,
                    Name = product.Category.Name
                }
            };

            return Ok(productDto);
        }

        [HttpGet("")]
        public IActionResult GetAll(int page = 1)
        {
            List<Product> products = _context.Products.Include(x => x.Category).Skip((page - 1) * 4).Take(4).ToList();

            return Ok(products);
        }

        [HttpPost("")]
        public IActionResult Create(ProductPostDto productDto)
        {
            Product product = new Product
            {
                CategoryId = productDto.CategoryId,
                Name = productDto.Name,
                SalePrice = productDto.SalePrice,
                CostPrice = productDto.CostPrice,
                DiscountPercent = productDto.DiscountPercent,
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return Created("", product);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductPostDto productPostDto)
        {
            Product product = _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            product.CategoryId = productPostDto.CategoryId;
            product.Name = productPostDto.Name;
            product.SalePrice = productPostDto.SalePrice;
            product.CostPrice = productPostDto.CostPrice;
            product.DiscountPercent = productPostDto.DiscountPercent;
            _context.SaveChanges();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
