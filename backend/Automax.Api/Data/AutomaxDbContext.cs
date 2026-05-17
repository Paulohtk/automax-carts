using Automax.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Automax.Api.Data;

public sealed class AutomaxDbContext(DbContextOptions<AutomaxDbContext> options) : DbContext(options)
{
    public DbSet<Cart> Carts => Set<Cart>();

    public DbSet<CartProduct> CartProducts => Set<CartProduct>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(cart => cart.Id);
            entity.Property(cart => cart.Id).ValueGeneratedNever();
            entity.Property(cart => cart.Date).IsRequired();
            entity.HasMany(cart => cart.Products)
                .WithOne(product => product.Cart)
                .HasForeignKey(product => product.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartProduct>(entity =>
        {
            entity.HasKey(product => product.Id);
            entity.Property(product => product.ProductId).IsRequired();
            entity.Property(product => product.Quantity).IsRequired();
        });
    }
}
