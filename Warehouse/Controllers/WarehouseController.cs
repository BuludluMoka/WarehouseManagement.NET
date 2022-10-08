using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Warehouse.Core.Helpers.Wrappers;
using Warehouse.Data.Dto;
using Warehouse.Data.Dto.Ambar;
using Warehouse.Data.Models;
using Warehouse.Data.Models.Common.Authentication;

namespace Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly WarehouseDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public WarehouseController(WarehouseDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet]
        [Authorize(Roles = "Admin,User")]
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
        public async Task<ActionResult<Anbar>> PostWarehouse(AnbarCreateDto anbar)
        {

            Anbar newAmbar = _mapper.Map<Anbar>(anbar);
            _context.Anbars.Add(newAmbar);

            await _context.SaveChangesAsync();

            object returnedWarehouse = new
            {
                Id = newAmbar.Id,
                Name = newAmbar.Name,
                Place = newAmbar.Place,
                Phone = newAmbar.Phone,
            };
            return Ok(new Response<object>(returnedWarehouse) { Message = "Warehouse elave edildi" });
            //return CreatedAtAction("PostWarehouse", new { id = newAmbar.Id }, anbar);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouseById(int id)
        {

            var notfound = _context.Anbars.Any(x => x.Id == id);
            if (!notfound) return NotFound(new Response<object> { Message = "Bele bir Anbar Tapılmadı" });
            
            List<AppUser> WarehouseUsers = await _userManager.Users.IgnoreQueryFilters().Include(x => x.Anbar).Where(x => x.AnbarId == id).ToListAsync();

            var ProductsOutWarehouse = (from t in _context.Transactions
                                        where t.sender_id == id
                                        group t by t.Product.Name into g
                                        select new
                                        {
                                            Id = g.Select(x => x.ProductId).FirstOrDefault(),
                                            Name = g.Key,
                                            Count = g.Sum(a => a.Count)
                                        }).ToList();

            var ProductsInWarehouse = (from t in _context.Transactions
                                       where t.receiver_id == id
                                       group t by t.Product.Name into g
                                       select new
                                       {
                                           Id = g.Select(x => x.ProductId).FirstOrDefault(),
                                           Name = g.Key,
                                           Count = g.Sum(a => a.Count) 
                                       }).ToList();


            if (ProductsInWarehouse == null) return NotFound(new Response<object>() { Message = "Tapılmadı" });

            object returnType = new
            {
                id = WarehouseUsers.Select(x => x.AnbarId).FirstOrDefault(),
                Warehouse = WarehouseUsers.Select(x => x.Anbar.Name).FirstOrDefault(),
                Place = WarehouseUsers.Select(x => x.Anbar.Place).FirstOrDefault(),
                Phone = WarehouseUsers.Select(x => x.PhoneNumber).FirstOrDefault(),
                CreatedDate = _context.Anbars.Find(id).CreatedDate.ToString("yyyy-MM-dd : HH-mm-ss"),
                Users = WarehouseUsers.Where(x => x.Id != "a18be9c0-aa65-4af8-bd17-00bd9344e575").Select(x => new { x.Id,x.UserName, x.PhoneNumber, x.Address, x.Email, x.Status, }),
                Products = ProductsInWarehouse.Select(x => new {
                    x.Id, x.Name, Miqdar = x.Id == ProductsOutWarehouse.Select(x=>x.Id).FirstOrDefault()? x.Count - ProductsOutWarehouse.Select(x=>x.Count).FirstOrDefault(): x.Count
                })

            };
            return Ok(new Response<object>(returnType));

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

            return Ok(new Response<object>() { Message = "Anbar Yeniləndi" });
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            if (_context.Anbars == null) return NotFound();

            bool hasUserWarehouse = _context.AppUsers.Any(x => x.AnbarId == id);
            bool hasTransaction = _context.Transactions.Any(x => x.sender_id == id || x.receiver_id == id);
            if (hasUserWarehouse || hasTransaction) return BadRequest(new Response<object>() { Message = "Anbarı silə bilməzsiniz" });
            var ambar = await _context.Anbars.FindAsync(id);

            _context.Anbars.Remove(ambar);
            await _context.SaveChangesAsync();

            return Ok(new Response<object>() { Succeeded = true, Message = "Anbar Silindi" });

        }

        private bool AmbarExists(int id)
        {
            return (_context.Anbars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
