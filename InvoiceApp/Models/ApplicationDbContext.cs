using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<CompanyDetails> CompanyDetails { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Company>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<Company>()
                .HasIndex(c => c.VATNumber)
                .IsUnique();

            builder.Entity<Company>()
                .HasMany(c => c.Invoices)
                .WithOne(i => i.Company);

            builder.Entity<Invoice>()
                .HasMany(i => i.InvoiceItems)
                .WithOne(ii => ii.Invoice);

            builder.Entity<InvoiceItem>()
                .Property(ii => ii.UnitPriceWithoutTax).HasPrecision(18, 2);

            base.OnModelCreating(builder);
        }
    }
}
