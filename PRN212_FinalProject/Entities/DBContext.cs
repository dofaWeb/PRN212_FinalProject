using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PRN212_FinalProject.Entities;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountState> AccountStates { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderState> OrderStates { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductItem> ProductItems { get; set; }

    public virtual DbSet<RoleName> RoleNames { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Variation> Variations { get; set; }

    public virtual DbSet<VariationOption> VariationOptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Load configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Get the connection string from the configuration
            var connectionString = config.GetConnectionString("PRN212");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Account");

            entity.HasIndex(e => e.RoleId, "role_id");

            entity.HasIndex(e => e.StateId, "state_id");

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId)
                .HasMaxLength(8)
                .HasColumnName("role_id");
            entity.Property(e => e.StateId)
                .HasMaxLength(8)
                .HasColumnName("state_id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("Account_ibfk_1");

            entity.HasOne(d => d.State).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("Account_ibfk_2");
        });

        modelBuilder.Entity<AccountState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Account_State");

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Category");

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Order");

            entity.HasIndex(e => e.StateId, "state_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.StateId)
                .HasMaxLength(8)
                .HasColumnName("state_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .HasColumnName("user_id");

            entity.HasOne(d => d.State).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("Order_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Order_ibfk_1");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Order_Item");

            entity.HasIndex(e => e.OrderId, "order_id");

            entity.HasIndex(e => e.ProductItemId, "product_item_id");

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .HasColumnName("id");
            entity.Property(e => e.OrderId)
                .HasMaxLength(8)
                .HasColumnName("order_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductItemId)
                .HasMaxLength(8)
                .HasColumnName("product_item_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("Order_Item_ibfk_1");

            entity.HasOne(d => d.ProductItem).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductItemId)
                .HasConstraintName("Order_Item_ibfk_2");
        });

        modelBuilder.Entity<OrderState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Order_State");

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Product");

            entity.HasIndex(e => e.CategoryId, "category_id");

            entity.HasIndex(e => e.SupplierId, "supplier_id");

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .HasColumnName("id");
            entity.Property(e => e.CategoryId)
                .HasMaxLength(8)
                .HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Picture)
                .HasMaxLength(50)
                .HasColumnName("picture");
            entity.Property(e => e.SupplierId)
                .HasMaxLength(8)
                .HasColumnName("supplier_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("Product_ibfk_1");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("Product_ibfk_2");
        });

        modelBuilder.Entity<ProductItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Product_Item");

            entity.HasIndex(e => e.ProductId, "product_id");

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .HasColumnName("id");
            entity.Property(e => e.Discount)
                .HasPrecision(10, 2)
                .HasColumnName("discount");
            entity.Property(e => e.ImportPrice).HasColumnName("import_price");
            entity.Property(e => e.ProductId)
                .HasMaxLength(8)
                .HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SellingPrice).HasColumnName("selling_price");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("Product_Item_ibfk_1");

            entity.HasMany(d => d.VariationOptions).WithMany(p => p.ProductItems)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductConfiguration",
                    r => r.HasOne<VariationOption>().WithMany()
                        .HasForeignKey("VariationOptionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Product_Configuration_ibfk_2"),
                    l => l.HasOne<ProductItem>().WithMany()
                        .HasForeignKey("ProductItemId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Product_Configuration_ibfk_1"),
                    j =>
                    {
                        j.HasKey("ProductItemId", "VariationOptionId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("Product_Configuration");
                        j.HasIndex(new[] { "VariationOptionId" }, "variation_option_id");
                        j.IndexerProperty<string>("ProductItemId")
                            .HasMaxLength(8)
                            .HasColumnName("product_item_id");
                        j.IndexerProperty<string>("VariationOptionId")
                            .HasMaxLength(8)
                            .HasColumnName("variation_option_id");
                    });
        });

        modelBuilder.Entity<RoleName>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Role_Name");

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Supplier");

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.ContactInfo)
                .HasMaxLength(100)
                .HasColumnName("contact_info");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Variation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Variation");

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<VariationOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Variation_Option");

            entity.HasIndex(e => e.VariationId, "variation_id");

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .HasColumnName("id");
            entity.Property(e => e.Value)
                .HasMaxLength(50)
                .HasColumnName("value");
            entity.Property(e => e.VariationId)
                .HasMaxLength(8)
                .HasColumnName("variation_id");

            entity.HasOne(d => d.Variation).WithMany(p => p.VariationOptions)
                .HasForeignKey(d => d.VariationId)
                .HasConstraintName("Variation_Option_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
