using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Helpers;
using Warehouse.Data.Dto;
using Warehouse.Data.Dto.Ambar;
using Warehouse.Data.Models;

namespace Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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
                return NotFound(new Response<object> { });
            }
            var anbarlar = await (from x in _context.Anbars
                           select new
                           {
                               x.Id,
                               x.Name,
                               x.Place
                              
                           }).ToListAsync();
            return Ok(new Response<object>(anbarlar));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Anbar>> PostWarehouse( AnbarCreateDto anbar)
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
                            x.Id,
                            x.Name,
                            x.Place,
                        }).ToListAsync();

            if (anbar.Count == 0)
            {
                return NotFound();
            }


            return Ok(new Response<object>(anbar));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
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

            return Ok(new Response<object>() { Message = "Anbar Yeniləndi"});
        }

      

        [HttpDelete("{id}")]
          [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {

            var ambar = await _context.Anbars.FindAsync(id);
            if (ambar == null)
            {
                return NotFound();
            }

            try
            {
                _context.Anbars.Remove(ambar);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest(new Response<object>() { Message = "Anbari silerken bir xeta yarandi" });

            }

            return Ok(new Response<object>() { Message = "Anbar Silindi" });

        }

        private bool AmbarExists(int id)
        {
            return (_context.Anbars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
