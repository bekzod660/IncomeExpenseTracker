﻿
using IncomeExpenseTracker.Data;
using IncomeExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IncomeExpenseTracker.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        private int count = 0;
        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            ViewBag.SubCategories = _context.Category.Where(x => x.ParentCategoryId != null).ToList();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var applicationDbContext = _context.Transaction.Where(x => x.UserId == userId).Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transaction/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Transaction == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transaction/Create
        public async Task<IActionResult> Create()
        {
            if (count == 0)
            {
                //List<Category> defaultCategory = new List<Category>()
                //{
                //   new Category()
                //   {
                //       id =1,
                //       Name = "Доходы"
                //   },
                //    new Category()
                //   {
                //       id = 2,
                //       Name = "Затраты"
                //   },
                //    new Category()
                //   {
                //       id = 3,
                //       Name = "Заработная плата",
                //       ParentCategoryId = 1
                //   },
                //     new Category()
                //   {
                //       id =4,
                //       Name = "Дохода с сдачи в аренду недвижимости",
                //        ParentCategoryId = 1
                //   },
                //   new Category()
                //   {
                //       id = 5,
                //       Name = "Иные доходы",
                //        ParentCategoryId = 1
                //   },
                //    new Category()
                //   {
                //       id = 6,
                //       Name = "Продукты питания",
                //       ParentCategoryId = 2
                //   },
                //   new Category()
                //   {
                //       id = 7,
                //       Name = "Транспорт",
                //        ParentCategoryId = 2
                //   },
                //   new Category()
                //   {
                //       id = 8,
                //       Name = "Мобильная связь",
                //        ParentCategoryId =2
                //   },
                //    new Category()
                //   {
                //       id = 9,
                //       Name = "Интернет",
                //        ParentCategoryId =2
                //   },
                //    new Category()
                //   {
                //       id = 10,
                //       Name = "Развлечения",
                //        ParentCategoryId = 2
                //   },
                //    new Category()
                //   {
                //       id = 11,
                //       Name = "Другое",
                //        ParentCategoryId = 2
                //   }
                // };
                //_context.Category.AddRange(defaultCategory);
                //await _context.SaveChangesAsync();
                //count++;
            }
            ViewBag.Categories = _context.Category.Where(x => x.ParentCategoryId == null).ToList();
            ViewBag.SubCategories = _context.Category.Where(x => x.ParentCategoryId != null).ToList();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.UserId = userId;
            var transaction = new Transaction { UserId = userId };

            return View(transaction);
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.Id = Guid.NewGuid();
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", transaction.UserId);
            return View(transaction);
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Transaction == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", transaction.UserId);
            return View(transaction);
        }

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DateAdded,Type,CategoryId,Amount,Comment,UserId")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", transaction.UserId);
            return View(transaction);
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Transaction == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Transaction == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Transaction'  is null.");
            }
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction != null)
            {
                _context.Transaction.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(Guid id)
        {
            return (_context.Transaction?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
