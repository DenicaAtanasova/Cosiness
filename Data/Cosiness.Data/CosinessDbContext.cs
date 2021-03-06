﻿namespace Cosiness.Data
{
    using Cosiness.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class CosinessDbContext 
        : IdentityDbContext<CosinessUser, CosinessRole, string>
    {
        public CosinessDbContext(DbContextOptions<CosinessDbContext> options)
            :base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrdersProducrts { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<ShoppingCartProduct> ShoppingCartsProducts { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Set> Sets { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<ProductColor> ProductsColors { get; set; }

        public DbSet<ProductMaterial> ProductsMaterials { get; set; }

        public DbSet<Material> Materials { get; set; }

        public DbSet<Dimension> Dimensions { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Storage> Storages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            builder.Entity<ShoppingCartProduct>()
                .HasKey(scp => new { scp.ShoppingCartId, scp.ProductId });

            builder.Entity<ProductColor>()
                .HasKey(pc => new { pc.ProductId, pc.ColorId });

            builder.Entity<ProductMaterial>()
                .HasKey(pm => new { pm.ProductId, pm.MaterialId});

            base.OnModelCreating(builder);
        }
    }
}