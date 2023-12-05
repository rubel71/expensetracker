using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController : Controller
    {
        ExpenseDbContext db;
        public ExpenseController(ExpenseDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View(db.Expenses.Include(x => x.Catagory).ToList());
        }
        public IActionResult Create()
        {
            ViewBag.catagory = db.Catagories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                db.Add(expense);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.catagory = db.Catagories.ToList();
            return View(expense);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await db.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewData["CatagoryId"] = new SelectList(db.Catagories, "CatagoryId", "CatagoryName", expense.CatagoryId);
            return View(expense);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpenseId,ItemName,Amount,ExpenseDate,CatagoryId")] Expense expense)
        {
            if (id != expense.ExpenseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(expense);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.ExpenseId))
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
            ViewData["CatagoryId"] = new SelectList(db.Expenses, "CatagoryId", "CatagoryName", expense.CatagoryId);
            return View(expense);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }
            db.Remove(expense);
            if (db.SaveChanges() > 0)
            {

                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }
        private bool ExpenseExists(int id)
        {
            return db.Expenses.Any(e => e.ExpenseId == id);
        }

    }
}
