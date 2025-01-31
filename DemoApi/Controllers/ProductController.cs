using DemoApi.Dtos;
using DemoApi.Interface;
using DemoApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        public ProductController(IRepository<Product> repository)
        {
                _repository = repository;
        }

        [HttpGet("Get-AllProduct")]
        public async Task<ActionResult<IEnumerable<ProductDtos>>> GetProduct()
        {
            var product = await _repository.GetAllAsync();
            if (product == null || !product.Any())
            {
                return NotFound(new { message = "No products found." });
            }
            var productdtos = product.Select(p => new ProductDtos { Name = p.Name, Price = p.Price, DateOnly = p.DateOnly });
            return Ok(productdtos);
        }

        [HttpGet("Get-ProductById/{Id}")]
        public async Task<ActionResult<ProductDtos>> GetProductById(int Id)
        {
            var product = await _repository.GetByIdAsync(Id);
            if (product == null)
            {
                return NotFound( new { message = $"No products found for the Id : {Id}." });
            }
            var productdtos =  new ProductDtos { Name = product.Name, Price = product.Price, DateOnly = product.DateOnly };
            return Ok(productdtos);
        }

        [HttpPost("Create-Product")]
        public async Task<ActionResult> CreateProduct([FromBody] ProductDtos productDtos)
        {
            if (productDtos == null || string.IsNullOrEmpty(productDtos.Name) || productDtos.Price <= 0)
            {
                return BadRequest(new { message = "Invalid product data." });
            }
            var product = new Product 
            { 
                Name = productDtos.Name,
                Price = productDtos.Price,
                DateOnly = productDtos.DateOnly 
            };
            await _repository.AddAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("Update-ProductById/{Id}")]
        public async Task<ActionResult> UpdateProduct(int Id, [FromBody] ProductDtos productDto)
        {
            if (productDto == null || string.IsNullOrEmpty(productDto.Name) || productDto.Price <= 0)
            {         
                return BadRequest(new { message = "Invalid product data." });
            }

            var product = await _repository.GetByIdAsync(Id);
       
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {Id} not found." });
            }

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            await _repository.UpdateAsync(product);
    
            return NoContent(); 
        }

        [HttpDelete("Delete-ProductById/{Id}")]
        public async Task<ActionResult> DeleteProduct(int Id)
        {
            var product = await _repository.GetByIdAsync(Id);
      
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {Id} not found." });
            }

            await _repository.DeleteByIdAsync(Id);

            
            return NoContent();
        }
    }
}
