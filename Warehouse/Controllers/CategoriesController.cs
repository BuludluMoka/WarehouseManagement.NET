using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Warehouse.Core.Services;
using Warehouse.Data.Dto;
using Warehouse.Data.Dto.Category;
using Warehouse.Data.Models;
using Warehouse.Core;

namespace Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly WarehouseDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(WarehouseDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<string> GetCategories()
        {
            if (_context.Categories == null)
            {
                return "Gozlenilmeyen bir xeta bas verdi";
            }
            List<Category> categories = await _context.Categories.ToListAsync();
            List<Category> category = CategoryTree.GetCategoryTree(categories);
            foreach (Category item in category)
            {
                if (item.categoryChildren == null)
                {
                    item.categoryChildren = new List<Category>();
                }
            }
            var Tree = category.Select(x => new
            {
                x.Id,
                x.Name,
                SubCat = x.categoryChildren.Select(d => new { d.Name, d.Id })
            });

            return JsonConvert.SerializeObject(Tree,Formatting.Indented,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategorybyId(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var category = await (from a in _context.Categories
                                  where a.Id == id
                                  select new
                                  {
                                      a.Id,
                                      a.Name,
                                      a.ParentId
                                  }).ToListAsync();
            if (category.Count == 0)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryUpdateDto model)
        {

            Category category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            category.Name = model.Name;
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory([FromForm] CategoryCreateDto model)
        {

            Category category = _mapper.Map<Category>(model);

            await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();

            return CreatedAtAction("PostCategory", new { id = category.Id }, model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
