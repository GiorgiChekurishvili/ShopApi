using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [Authorize]
        [RoleFilter("StandartUser,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
        {
            var product = await _productService.GetProducts();
            if (product.Count() ==0)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [Authorize]
        [RoleFilter("Admin")]
        [HttpGet("{id}")]
        public async  Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        
        [HttpPost]
        public IActionResult Post([FromBody] ProductReturnDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userid = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var x = _productService.CreateProduct(productDto, userid);
            return CreatedAtAction(nameof(Get), new { id = x.Id }, productDto);
        }
    }
}
