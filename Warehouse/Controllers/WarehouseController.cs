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
using Warehouse.Core.Helpers;
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

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetWarehouseById(int id)
        //{
            //List<AppUser> WarehouseUsers = _userManager.Users.Include(x=>x.Anbar).Where(x=>x.AnbarId == 2).ToList();
            //if (_context.Users == null)
            //{
            //    //return NotFound();
            //}
            //var a = _context.Transactions.
            //    Select(x => new Transaction
            //    {
            //        Id = x.Id,
            //        Product = x.Product,
            //        Receiver = x.Receiver,
            //        User = x.User
                   
                    
            //    }).FirstOrDefault().;

            //.Select(n => new Questionnaire
            //{
            //    Id = n.Id,
            //    Name = n.Name,
            //    Questions = n.Questions.Select(q => new Question
            //    {
            //        Id = q.Id,
            //        Text = q.Text,
            //        Answers = q.Where(a => a.UserId == userId).ToList()
            //    }).ToList()
            //})

            //var data = from x in a
            //           select new
            //           {
            //               x.Id,
            //               ProductName =  new List<Product> { x.Product }
            //           };

            //return Ok(a);
            //var inProduct = (from x in _context.Transactions

            //                 select x).ToList();

            //object returnedInfo = new
            //{
            //    Users = WarehouseUsers.Select(x => x.Email).ToList(),
            //    Warehouse = _context.Anbars.FirstOrDefault(x=>x.Id == 2)
            //};

            //int outProduct = (from t in _context.Transactions
            //                  where t.ProductId == model.ProductId && t.sender_id == model.sender_id && t.Status != false
            //                  select t).Sum(x => x.Count);

            //var reletions = from x in _context.Transactions
            //                 where x
            //                 where 


            //if (anbar.Count == 0)
            //{
            //    return NotFound();
            //}



            //return JsonConvert.SerializeObject(a, Formatting.Indented,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });
        //}

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
