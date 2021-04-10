using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;
        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            return await _context.Products
                .Select(x => productsDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);   
            if(product ==null)
            {
                return NotFound();
            }

            return productsDTO(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO productDTO)
        {
            var product = new Product
            {
                IsAvailable = productDTO.IsAvailable,
                Name = productDTO.Name,
                Price = productDTO.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(
                nameof(GetProduct), 
                new {id = product.Id}, 
                productsDTO(product));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDTO)
        {
            if(id!=productDTO.Id)
            {
                return BadRequest();
            }

            var product = await _context.Products.FindAsync(id);
            if(product ==null)
            {
                return NotFound();
            }

            product.Name = productDTO.Name;
            product.Price = productDTO.Price;
            product.IsAvailable = productDTO.IsAvailable;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;  
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id) =>
            _context.Products.Any(e => e.Id == id);
        
        private static ProductDTO productsDTO(Product product) =>
            new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                IsAvailable = product.IsAvailable
            };
    }
}