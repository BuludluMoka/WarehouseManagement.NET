using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Warehouse.Data.Dto;
using Warehouse.Data.Models;
using Warehouse.Data.Models.Common.Authentication;

namespace Warehouse.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly WarehouseDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public UserController(WarehouseDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetLastTransactions()
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var userTransaction = await (from tr in _context.Transactions
                                         where tr.receiver_id == currentUser.AnbarId
                                         orderby tr.CreatedDate
                                         select new
                                         {
                                             tr.Id,
                                             TransactionNo = tr.TransactionNo,
                                             Hardan = tr.Sender.Name == null ? "Import" : tr.Sender.Name,
                                             Mehsul = tr.Product.Name,
                                             Kateqoriyasi = tr.Product.Category.Name,
                                             Miqdar = tr.Count,
                                             Veziyyeti = tr.Status == false ? "Gozlemede" : "Qebul edildi",
                                             Nevaxt = tr.CreatedDate.ToString("yyyy-MM-dd : HH-mm-ss"),
                                             Kim = tr.sender_id == null ? "Import" : tr.User.Email

                                         }).ToListAsync();

            if (userTransaction == null) return NotFound();
            return Ok(userTransaction);
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> UserPostTransaction(TransactionCreateDto model)
        {
            //get user info
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

            bool isProductExist = _context.Products.Any(x => x.Id == model.productId);
            bool isTransactionNoExist = _context.Transactions.Any(x => x.TransactionNo == model.TransactionNo);
            bool isSenderExist = _context.Anbars.Any(x => x.Id == currentUser.AnbarId);

            if (currentUser.AnbarId == model.Receiver) return BadRequest(new { ErrorMessage = "Eyni anbarlar arasi gonderim ede bilmezsiniz" });
            model.Receiver = model.Sender == false ? currentUser.AnbarId : model.Receiver;

            //check if valid
            if (model.Sender == true)
            {
                bool isReceiverExist = _context.Anbars.Any(x => x.Id == model.Receiver);
                if (!isReceiverExist) return NotFound(new { ErrorMessage = "Mehsul gondermek isdediyiniz anbar tapilmadi" });
            }
            if (isTransactionNoExist) return BadRequest(new { ErrorMessage = "Transaction Nomresi movcuddur" });
            if (!isProductExist) return NotFound(new { ErrorMessage = "Anbarda bu mehsuldan yoxdur" });

            //crate new product
            Transaction newTransaction = new Transaction()
            {
                TransactionNo = model.TransactionNo,
                sender_id = model.Sender == true ? currentUser.AnbarId : null,
                receiver_id = model.Receiver,
                ProductId = model.productId,
                UserId = currentUser.Id,
                Count = model.Count
            };
            //check if product count available
            if (model.Sender)
            {
                int inProduct = (from t in _context.Transactions
                                 where t.ProductId == model.productId && t.receiver_id == currentUser.AnbarId && t.Status != false
                                 select t).Sum(x => x.Count);

                int outProduct = (from t in _context.Transactions
                                  where t.ProductId == model.productId && t.sender_id == currentUser.AnbarId && t.Status != false
                                  select t).Sum(x => x.Count);

                if (inProduct - outProduct < model.Count)
                {
                    return BadRequest(new { ErrorMessage = "Anbarda kifayet qeder mehsul yoxdur" });
                }

                _context.Transactions.Add(newTransaction);
            }
            //save
            _context.Transactions.Add(newTransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("UserPostTransaction", model);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> AcceptTransaction(int id, bool accept)
        {
            Transaction transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return NotFound(new { ErrorMessage = $"{id} nomreli transaction tapilmadi" });
            if (accept)
            {
                transaction.Status = true;
                _context.Entry(transaction).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                return NoContent();
            }



            return Ok(new { Success = "Mehsulu qebul etdiniz.." });
        }

    }
}
