using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Dto;
using Warehouse.Data.Dto.Products;
using Warehouse.Data.Models;

namespace Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly WarehouseDbContext _context;

        public ProductsController(WarehouseDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task< IActionResult> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = (from a in _context.Products
                         select new { a.Name, a.buyPrice, a.sellPrice,Category = a.Category.Name, a.CreatedDate, a.UpdatedDate }).ToList();
            return Ok(product);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductUpdateDto model)
        {
            Product product = await _context.Products.FindAsync(id);
            product.Name = model.Name;
            product.buyPrice = model.buyPrice;
            product.sellPrice = model.sellPrice;
            product.Name = model.Name;

            _context.Entry(product).State = EntityState.Modified;

            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        //POST: api/Products
       [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductCreateDto model)
        {

            Product newProduct = new();
            newProduct.Name = model.Name;
            newProduct.buyPrice = model.buyPrice;
            newProduct.sellPrice = model.sellPrice;
            newProduct.category_Id = model.category_Id;

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = newProduct.Id }, model);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
