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
using Warehouse.Core.RequestParameters;
using Warehouse.Data.Dto;
using Warehouse.Data.Dto.Transactions;
using Warehouse.Data.Models;
using Warehouse.Data.Models.Common.Authentication;

namespace Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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
        public  ActionResult GetTransactions([FromQuery] Pagination pagination)
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            var totalTransaction = _context.Transactions.Count();
            var transactions = (from x in _context.Transactions
                                select new
                                {
                                    Sender = x.Sender.Name,
                                    Receiver = x.Receiver.Name,
                                    Mehsul = x.Product.Name,
                                    Miqdar = x.Count
                                }).Skip((pagination.Page - 1) * (pagination.Size)).Take(pagination.Size);

            return Ok(new { totalTransaction, transactions});
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
        public async Task<ActionResult<Transaction>> PostTransaction( AdminTransactionCreateDto model)
        {


            bool isTransactionNoExist = _context.Transactions.Any(x => x.TransactionNo == model.TransactionNo);
            model.sender_id = model.sender_id == 0 ? null : model.sender_id;

            bool isProductExist = _context.Products.Any(x => x.Id == model.ProductId);
            bool isSenderExist = _context.Anbars.Any(x => x.Id == model.sender_id);
            bool isReceiverExist = _context.Anbars.Any(x => x.Id == model.receiver_id);


                
            if (model.sender_id == model.receiver_id) { return SentMessage("Eyni anbarlar arasi gonderim ede bilmezsiniz", false); }
            if (!isProductExist) { return SentMessage("Anbarda bu mehsuldan yoxdur", false); }
            if (!isSenderExist && model.sender_id != null) { return SentMessage("Mehsulu gonderen anbar Tapilmadi", false); }
            if (!isReceiverExist) { return SentMessage("Mehsulu qebul eden anbar tapilmadi", false); }
            if (isTransactionNoExist) return BadRequest(new { ErrorMessage = "Transaction Nomresi movcuddur" });

            Transaction newTransaction = _mapper.Map<Transaction>(model);
            newTransaction.UserId = (await _userManager.GetUserAsync(HttpContext.User)).Id;


            if (isSenderExist)
            {
                int inProduct = (from t in _context.Transactions
                                 where t.ProductId == model.ProductId && t.receiver_id == model.sender_id && t.Status != false
                                 select t).Sum(x => x.Count);

                int outProduct = (from t in _context.Transactions
                                  where t.ProductId == model.ProductId && t.sender_id == model.sender_id && t.Status != false
                                  select t).Sum(x => x.Count);

                if (inProduct - outProduct < model.Count)
                {
                    return SentMessage("Anbarda kifayet qeder mehsul yoxdur", false);
                }

                _context.Transactions.Add(newTransaction);
            }
            _context.Transactions.Add(newTransaction);
            await _context.SaveChangesAsync();

            return SentMessage($"Mehsul gonderildi", true, model);

        }

        [NonAction]
        private bool TransactionsExists(int id)
        {
            return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [NonAction]
        private ObjectResult SentMessage(string msj, bool isSucces, object model = null)
        {
            if (isSucces == true)
            {
                return Ok(new
                {
                    Transaction = model,
                    succes = isSucces,
                    message = msj
                });
            }
            return BadRequest(new
            {
                success = isSucces,
                message = msj
            });
        }
    }
}

