using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Helpers.Wrappers;
using Warehouse.Data.Dto;
using Warehouse.Data.Dto.Transactions;
using Warehouse.Data.Models;
using Warehouse.Data.Models.Common.Authentication;

namespace Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class TransactionsController : ControllerBase
    {
        private readonly WarehouseDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public TransactionsController(WarehouseDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public  ActionResult GetTransactions([FromQuery] PaginationFilter pFilter, string searchString, string sort)
        {
            var totalTransaction = _context.Transactions.Count();
            var transactions = (from x in _context.Transactions
                                select new
                                {
                                    x.Id,
                                    Sender = x.Sender.Name,
                                    Receiver = x.Receiver.Name,
                                    Mehsul = x.Product.Name,
                                    Miqdar = x.Count,
                                    CreatedDate = x.CreatedDate
                                });

            if (!string.IsNullOrEmpty(searchString))
            {
                transactions = transactions.Where(x => x.Sender.Contains(searchString) || x.Mehsul.Contains(searchString) || x.Receiver.Contains(searchString));
            }
            switch (sort)
            {
                case "createdDate":
                    transactions = transactions.OrderBy(x => x.CreatedDate);
                    break;
                case "createdDateDesc":
                    transactions = transactions.OrderByDescending(x => x.CreatedDate);
                    break;
                case "sender":
                    transactions = transactions.OrderBy(x => x.Sender);
                    break;
                case "receiver":
                    transactions = transactions.OrderBy(x => x.Receiver);
                    break;
                case "productCount":
                    transactions = transactions.OrderBy(x => x.Miqdar);
                    break;
                default:
                    break;
            }

            var returnedTransaction = transactions.Skip((pFilter.PageNumber - 1) * (pFilter.PageSize)).Take(pFilter.PageSize);
            
            return Ok(new PagedResponse<object>(returnedTransaction, pFilter.PageNumber, pFilter.PageSize, totalTransaction));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTransactionById(int id)
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            var Transactions = await (from x in _context.Transactions
                                      where x.Id == id
                                      select new
                                      {
                                          x.Id,
                                          x.sender_id,
                                          x.receiver_id,
                                          x.ProductId,
                                          x.Count
                                      }).ToListAsync();

            if (Transactions.Count == 0)
            {
                return NotFound();
            }

            return Ok(Transactions);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost, Route("PostTransaction")]
        public async Task<ActionResult> PostTransaction( AdminTransactionCreateDto model)
        {

            //bool isTransactionNoExist = _context.Transactions.Any(x => x.TransactionNo == model.TransactionNo);
            model.sender_id = model.sender_id == 0 ? null : model.sender_id;

            bool isProductExist = _context.Products.Any(x => x.Id == model.ProductId);
            bool isSenderExist = _context.Anbars.Any(x => x.Id == model.sender_id);
            bool isReceiverExist = _context.Anbars.Any(x => x.Id == model.receiver_id);


                
            if (model.sender_id == model.receiver_id)  return BadRequest( new Response<object> { Message = "Eyni anbarlar arasi gonderim ede bilmezsiniz" });
            if (!isProductExist) return BadRequest( new Response<object> { Message= "Anbarda bu mehsuldan yoxdur" });
            if (!isSenderExist && model.sender_id != null) return NotFound(new Response<object> { Message = "Mehsulu gonderen anbar Tapilmadi" });
            if (!isReceiverExist) return NotFound(new Response<object> { Message= "Mehsulu qebul eden anbar tapilmadi" });
            

            Transaction newTransaction = _mapper.Map<Transaction>(model);
            newTransaction.UserId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            newTransaction.TransactionNo = Guid.NewGuid().ToString();


            if (isSenderExist)
            {
                int inProduct = (from t in _context.Transactions
                                 where t.ProductId == model.ProductId && t.receiver_id == model.sender_id
                                 select t).Sum(x => x.Count);

                int outProduct = (from t in _context.Transactions
                                  where t.ProductId == model.ProductId && t.sender_id == model.sender_id 
                                  select t).Sum(x => x.Count);

                if (inProduct - outProduct < model.Count)
                {
                    return NotFound(new Response<object>() { Message = "Anbarda kifayet qeder mehsul yoxdur" });
                    
                }

                _context.Transactions.Add(newTransaction);
            }
            _context.Transactions.Add(newTransaction);
            await _context.SaveChangesAsync();

            object returnedTransaction = new
            {
                Id = newTransaction.Id,
                TransactionNo = newTransaction.TransactionNo,
                Sender = newTransaction.sender_id,
                Receiver = newTransaction.receiver_id,
                Mehsul = newTransaction.ProductId,
                Miqdar = newTransaction.Count
            };

            return Ok(new Response<object>(returnedTransaction) { Message = "Mehsul gonderildi"});

        }

       
       
    }
}

