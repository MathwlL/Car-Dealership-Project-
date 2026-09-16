using CarMark.Models;
using Microsoft.EntityFrameworkCore;

namespace CarMark.Data;

public class CarMarkDbContext(DbContextOptions<CarMarkDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<CarImage> CarImages => Set<CarImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.Email).HasMaxLength(160);
            entity.Property(user => user.Name).HasMaxLength(100);
        });
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasOne(car => car.Seller).WithMany().HasForeignKey(car => car.SellerId).OnDelete(DeleteBehavior.Restrict);
            entity.Property(car => car.Company).HasMaxLength(80);
            entity.Property(car => car.Model).HasMaxLength(100);
        });
        modelBuilder.Entity<CarImage>().HasOne(image => image.Car).WithMany(car => car.Images).HasForeignKey(image => image.CarId);
    }
}
