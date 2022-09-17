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
    public class TransactionsController : ControllerBase
    {
        private readonly WarehouseDbContext _context;

        public TransactionsController(WarehouseDbContext context)
        {
            _context = context;
        }

        // GET: api/Transactionss
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransactions(int id)
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            var Transactions = await _context.Transactions.FindAsync(id);

            if (Transactions == null)
            {
                return NotFound();
            }

            return Transactions;
        }



        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransactions(TransactionCreateDto model)
        {
            int? senderIdNull = model.senderId == 0 ? null : model.senderId;

            bool isProductExist = _context.Products.Any(x => x.Id == model.productId);
            bool isSenderExist = _context.Ambars.Any(x=> x.Id == model.senderId);
            bool isReceiverExist = _context.Ambars.Any(x=>x.Id == model.receiverId);


            if (model.senderId == model.receiverId) { return SentMessage("Ambarlar eyni ola bilmez",false); }
            if (!isProductExist) { return SentMessage("Anbarda bu mehsuldan yoxdur", false); }
            if (!isReceiverExist) { return SentMessage("Mehsul gondermek isdediyiniz ambar tapilmadi", false); }

            Transaction newTransaction = new();

            if (isSenderExist)
            {
                
                int inProduct = (from t in _context.Transactions
                                 where t.product_id == model.productId && t.receiver_id == model.senderId
                                 select t).Sum(x => x.Count);


                int outProduct = (from t in _context.Transactions
                                  where t.product_id == model.productId && t.sender_id == model.senderId
                                  select t).Sum(x => x.Count);

                if (inProduct - outProduct < model.Count)
                {
                    return SentMessage("Anbarda kifayet qeder mehsul yoxdur", false);
                }

                newTransaction.product_id = model.productId;
                newTransaction.sender_id = model.senderId;
                newTransaction.receiver_id = model.receiverId;
                newTransaction.Count = model.Count;

                _context.Transactions.Add(newTransaction);
                await _context.SaveChangesAsync();
            }
            else
            {
                newTransaction.product_id = model.productId;
                newTransaction.sender_id = null;
                newTransaction.receiver_id = model.receiverId;
                newTransaction.Count = model.Count;

                _context.Transactions.Add(newTransaction);
                await _context.SaveChangesAsync();
            }

            return SentMessage("Mehsul gonderildi",true);
        }

       

        private bool TransactionsExists(int id)
        {
            return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private BadRequestObjectResult SentMessage(string msj, bool isSucces)
        {
            return BadRequest(new
            {
                succes = isSucces,
                message = msj
            });
        }
    }
}
