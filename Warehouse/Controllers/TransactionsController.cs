using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Dto;
using Warehouse.Data.Dto.Transactions;
using Warehouse.Data.Models;

namespace Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly WarehouseDbContext _context;
        private readonly IMapper  _mapper;

        public TransactionsController(WarehouseDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetTransactions()
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            var transactions= await (from x in _context.Transactions
                                select new
                                {
                                   Sender = x.Sender.Name,
                                   Receiver =  x.Receiver.Name,
                                   Mehsul = x.Product.Name,
                                   Miqdar =  x.Count
                                }).ToListAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTransactionsById(int id)
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
                                    x.product_id,
                                    x.Count
                                }).ToListAsync();

            if (Transactions.Count == 0)
            {
                return NotFound();
            }

            return Ok(Transactions);
        }



        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransactions([FromForm]TransactionCreateDto model)
        {
            int? senderIdNull = model.senderId == 0 ? null : model.senderId;

            bool isProductExist = _context.Products.Any(x => x.Id == model.productId);
            bool isSenderExist = _context.Ambars.Any(x=> x.Id == model.senderId);
            bool isReceiverExist = _context.Ambars.Any(x=>x.Id == model.receiverId);


            if (model.senderId == model.receiverId) { return SentMessage("Eyni anbarlar arasi gonderim ede bilmresiniz",false); }
            if (!isProductExist) { return SentMessage("Anbarda bu mehsuldan yoxdur", false); }
            if (!isReceiverExist) { return SentMessage("Mehsul gondermek isdediyiniz ambar tapilmadi", false); }

            Transaction newTransaction = _mapper.Map<Transaction>(model);

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

                _context.Transactions.Add(newTransaction);
            }
            _context.Transactions.Add(newTransaction);
            await _context.SaveChangesAsync();

            return SentMessage("Mehsul gonderildi",true);
        }

       

        private bool TransactionsExists(int id)
        {
            return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private ObjectResult SentMessage(string msj, bool isSucces)
        {
            if (isSucces == true)
            {
                return Ok(new
                {
                    succes = isSucces,
                    message = msj
                });
            }
            return BadRequest(new
            {
                succes = isSucces,
                message = msj
            });
        }
    }
}
