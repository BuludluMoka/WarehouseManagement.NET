using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.RequestParameters;
using Warehouse.Data.Dto;
using Warehouse.Data.Dto.Products;
using Warehouse.Data.Models;

namespace Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly WarehouseDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(WarehouseDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProducts([FromQuery]Pagination pagination)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var totalProducts = _context.Products.Count();
            var products = (from a in _context.Products
                                  select new { a.Name, a.buyPrice, a.sellPrice, Category = a.Category.Name, a.CreatedDate }).Skip(pagination.Size * pagination.Page).Take(pagination.Size);
            return Ok(new { totalProducts, products });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var products = await (from a in _context.Products
                                  where a.Id == id
                                  select new { a.Id, a.Name, a.buyPrice, a.sellPrice, Category = a.Category.Name, a.CreatedDate }).ToListAsync();

            if (products.Count == 0)
            {
                return NotFound();
            }

            return Ok(products);
        }



        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductCreateDto model)
        {

            bool isCategoryExist = await _context.Categories.AnyAsync(x => x.Id == model.CategoryId);
            if (!isCategoryExist)
            {
                return NotFound(new { ErrorMessage = "Gosterdiyniz kateqoriya tapilmadi.." });
            }
            Product newProduct = _mapper.Map<Product>(model);

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostProduct", new { id = newProduct.Id }, model);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto model)
        {
            Product products = await _context.Products.FindAsync(id);
            if (products == null) return NotFound(new { ErrorMessage = $"{id} nomreli mehsul tapilmadi" });
            Product product = _mapper.Map<Product>(model);

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
