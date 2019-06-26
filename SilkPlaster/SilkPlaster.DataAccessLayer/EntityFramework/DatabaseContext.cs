using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DatabaseContext")
        {
            Database.SetInitializer(new MyInitializer());
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Slider> Sliders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;

            //Fluent API

            modelBuilder.Entity<Member>()
                .HasMany(n => n.Orders)
                .WithRequired(n => n.Member)
                .WillCascadeOnDelete(false);
        }
    }
}
