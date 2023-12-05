using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.CustomValidation;


namespace ExpenseTracker.Models
{
    public class Catagory
    {
        public Catagory()
        {
            this.Expenses = new List<Expense>();
        }
        public int CatagoryId { get; set; }
      
        [Required(ErrorMessage ="Catagory name required"),StringLength(50),Display(Name ="Expense Catagory")]
        public string CatagoryName { get; set; }
        //nev
        public virtual ICollection<Expense> Expenses { get; set; }

    }
    public class Expense
    {
        public int ExpenseId { get; set; }
        [Required(ErrorMessage = "Item name required"), StringLength(50), Display(Name = "Item Name")]
        public string ItemName { get; set; }
        [Required(ErrorMessage = "Amount required"), Display(Name = "Amount")]

        public int Amount { get; set; }
        [Required(ErrorMessage ="Expense date required")]
        [DataType(DataType.Date), Display(Name = "Expense Date")]
        [CustomExpenseDate(ErrorMessage = "Expense Date must be less than or equal to Today's Date.")]
        public DateTime ExpenseDate { get; set; }
        //fk
        [ForeignKey("Catagory")]
        public int CatagoryId { get; set; }
        //nev
        public virtual Catagory Catagory { get; set; }
    }
    public class ExpenseDbContext:DbContext
    {
       
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options) { }
        public DbSet<Catagory> Catagories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Catagory>(entity => {
                entity.HasIndex(e => e.CatagoryName).IsUnique();
            });
        }

    }
   


}
