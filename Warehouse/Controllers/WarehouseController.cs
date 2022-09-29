using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Dto;
using Warehouse.Data.Dto.Ambar;
using Warehouse.Data.Models;

namespace Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly WarehouseDbContext _context;
        private readonly IMapper _mapper;

        public WarehouseController(WarehouseDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetWarehouses()
        {
            if (_context.Anbars == null)
            {
                return NotFound();
            }
            var anbarlar = await (from x in _context.Anbars
                           select new
                           {
                               x.Name,
                               x.Place
                              
                           }).ToListAsync();
            return Ok(anbarlar);
        }

        [HttpPost]
        public async Task<ActionResult<Anbar>> PostWarehouse([FromForm] AnbarCreateDto anbar)
        {

            Anbar newAmbar = _mapper.Map<Anbar>(anbar);
            _context.Anbars.Add(newAmbar);

            await _context.SaveChangesAsync();

            return CreatedAtAction("PostWarehouse", new { id = newAmbar.Id }, anbar);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetWarehouseById(int id)
        {
            if (_context.Anbars == null)
            {
                return NotFound();
            }
            var anbar = await (from x in _context.Anbars
                        where x.Id == id
                        select new
                        {
                            x.Name,
                            x.Place,
                        }).ToListAsync();

            if (anbar.Count == 0)
            {
                return NotFound();
            }


            return Ok(anbar);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWarehouse(int id, AnbarUpdateDto model)
        {

            Anbar ambar = await _context.Anbars.FindAsync(id);
            ambar.Name = model.Name;
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

      

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {

            var ambar = await _context.Anbars.FindAsync(id);
            if (ambar == null)
            {
                return NotFound();
            }

            _context.Anbars.Remove(ambar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AmbarExists(int id)
        {
            return (_context.Anbars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
