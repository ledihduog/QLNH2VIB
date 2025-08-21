using Microsoft.EntityFrameworkCore;
using System.Data;
using System;
using QuanLyNhaHang.Models.QLNHModels;
using QuanLyNhaHang.Common;

namespace QuanLyNhaHang.Models.Context
{
    public class QLNHContext : DbContext
    {
        public QLNHContext()
        {
        }

        public QLNHContext(DbContextOptions<QLNHContext> options)
            : base(options)
        {
        }
        // Code này dùng code first (migration) chứ k theo kiểu bd first (scraffold)
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserLogin> UserLogin { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        //public DbSet<OrderItem> OrderItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Constants.SQL_QLNH);
            }
        }
    }
}
