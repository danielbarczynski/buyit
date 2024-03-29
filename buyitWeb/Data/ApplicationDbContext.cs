﻿using buyitWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace buyitWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<CoverTypeModel> CoverTypes { get; set; }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<CartModel> Cart{ get; set; }
        public DbSet<OrderHeaderModel> OrderHeaders { get; set; }
        public DbSet<OrderDetailModel> OrderDetails { get; set; }
    }
}