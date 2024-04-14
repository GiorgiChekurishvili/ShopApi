using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using System.Collections;
using WebApiWithoutEmpty.Dtos;
using WebApiWithoutEmpty.Entities;
using WebApiWithoutEmpty.Services;
using WebApiWithoutEmpty.Filter;
using System.Security.Claims;


namespace WebApiWithoutEmpty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categorservice;

        public CategoryController(ICategoryService categoryService)
        {
            _categorservice = categoryService;
        }
        [Authorize]
        [RoleFilter("Admin,StandartUser")]
        [HttpGet]
        
        public async  Task<ActionResult<IEnumerable<CategoryReturnDto>>> Get()
        {
            
            var categories = await _categorservice.GetCategories();
            
            if (!categories.Any())
            {
                return NotFound();
            }
            return Ok(categories);
        }
        [Authorize]
        [RoleFilter("StandartUser")]
       
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _categorservice.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
                
            }
            return Ok(category);
        }

    
        [HttpPost]
        public IActionResult Post([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _categorservice.CreateCategory(categoryDto, userid);
            return CreatedAtAction(nameof(Get), new { id = categoryDto.Id }, categoryDto);
        }
    }
}
