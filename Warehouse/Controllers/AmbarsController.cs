using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Dto;
using Warehouse.Data.Models;

namespace Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmbarsController : ControllerBase
    {
        private readonly WarehouseDbContext _context;

        public AmbarsController(WarehouseDbContext context)
        {
            _context = context;
        }

        // GET: api/Ambars
        [HttpGet]
        public IActionResult GetAmbars()
        {
            if (_context.Ambars == null)
            {
                return NotFound();
            }

            var ambar = (from a in _context.Ambars
                         select new { a.Name, a.Place, a.Type, a.CreatedDate, a.UpdatedDate }).ToList();

            return Ok(ambar);
        }

        // GET: api/Ambars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ambar>> GetAmbar(int id)
        {
            if (_context.Ambars == null)
            {
                return NotFound();
            }
            var ambar = await _context.Ambars.FindAsync(id);

            if (ambar == null)
            {
                return NotFound();
            }


            return ambar;
        }

        // PUT: api/Ambars/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAmbar(int id, AmbarUpdateDto model)
        {
            Ambar ambar = await _context.Ambars.FindAsync(id);
            ambar.Name = model.Name;
            ambar.Type = model.Type;
            ambar.Place = model.Place;
            _context.Entry(ambar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmbarExists(id))
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

        // POST: api/Ambars
        [HttpPost]
        public async Task<ActionResult<Ambar>> PostAmbar(AmbarCreateDto ambar)
        {

            Ambar newAmbar = new();
            newAmbar.Name = ambar.Name;
            newAmbar.Place = ambar.Place;
            newAmbar.Type = ambar.Type;

            _context.Ambars.Add(newAmbar);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAmbar", new { id = newAmbar.Id }, ambar);
        }

        // DELETE: api/Ambars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmbar(int id)
        {

            var ambar = await _context.Ambars.FindAsync(id);
            if (ambar == null)
            {
                return NotFound();
            }

            _context.Ambars.Remove(ambar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AmbarExists(int id)
        {
            return (_context.Ambars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
