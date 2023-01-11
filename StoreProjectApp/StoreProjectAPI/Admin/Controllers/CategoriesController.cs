using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreProjectAPI.Admin.Dtos.CategoryDtos;
using Store.Core.Entities;
using Store.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Store.Core.Repositories;
using StoreProjectAPI.Admin.Dtos;

namespace StoreProjectAPI.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(CategoryPostDto postDto)
        {
            if (await _categoryRepository.IsExistAsync(x => x.Name == postDto.Name))
            {
                ModelState.AddModelError("Name", "Category already created!");
                return BadRequest(ModelState);
            }

            Category category = _mapper.Map<Category>(postDto);

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.CommitAsync();

            return StatusCode(201, category);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Category category = await _categoryRepository.GetAsync(x => x.Id == id);

            if (category == null) return NotFound();

            CategoryGetDto categoryDto = _mapper.Map<CategoryGetDto>(category);

            return Ok(categoryDto);
        }

        [HttpGet("")]
        public IActionResult GetAll(int page = 1)
        {
            var query = _categoryRepository.GetAll(x => true);

            var categoryDtos = _mapper.Map<List<CategoryListItemDto>>(query.Skip((page - 1) * 4).Take(4).ToList());

            PaginationListDto<CategoryListItemDto> model =
            new PaginationListDto<CategoryListItemDto>(categoryDtos, page, 4, query.Count());

            return Ok(model);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            List<CategoryListItemDto> items = _mapper.Map<List<CategoryListItemDto>>(_categoryRepository.GetAll(x => true).ToList());
            return Ok(items);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _categoryRepository.GetAsync(x => x.Id == id);

            if (category == null) return NotFound();

            _categoryRepository.Remove(category);
            _categoryRepository.Commit();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, CategoryPostDto postDto)
        {
            Category category = await _categoryRepository.GetAsync(x => x.Id == id);

            if (category == null) return NotFound();

            if (await _categoryRepository.IsExistAsync(x => x.Id != id && x.Name == postDto.Name)) return BadRequest();

            category.Name = postDto.Name;
            _categoryRepository.Commit();

            return NoContent();
        }
    }
}
