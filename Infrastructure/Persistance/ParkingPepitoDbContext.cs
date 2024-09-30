using Core.Entities;
using Core.Views;
using Domain.Entities;
using Domain.Interfaces.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

public partial class ParkingPepitoDbContext(DbContextOptions<ParkingPepitoDbContext> options) : DbContext(options), IParkingPepitoDbContext
{
    public virtual DbSet<CostType> CostType { get; set; }

    public virtual DbSet<Employee> Employee { get; set; }

    public virtual DbSet<ResidentDebts> ResidentDebts { get; set; }

    public virtual DbSet<Stay> Stay { get; set; }

    public virtual DbSet<Vehicle> Vehicle { get; set; }

    public virtual DbSet<VehicleType> VehicleType { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CostType>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Id, "CostType_Index_1");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Id, "Employee_Index_1");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasColumnType("numeric(18, 0)");
        });

        modelBuilder.Entity<ResidentDebts>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ResidentDebts");

            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.PlateNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.TotalCost).HasColumnType("money");
        });

        modelBuilder.Entity<Stay>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Id, "Index_1");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.DeleteDate).HasColumnType("datetime");
            entity.Property(e => e.FinalDate).HasColumnType("datetime");
            entity.Property(e => e.InitialDate).HasColumnType("datetime");

            entity.HasOne(d => d.DeleteEmployee).WithMany(p => p.StayDeleteEmployee)
                .HasForeignKey(d => d.DeleteEmployeeId)
                .HasConstraintName("FK_4");

            entity.HasOne(d => d.Employee).WithMany(p => p.StayEmployee)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_2");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Stay)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_3");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Id, "Index_1");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.PlateNumber).HasMaxLength(50);

            entity.HasOne(d => d.VehicleType).WithMany(p => p.Vehicle)
                .HasForeignKey(d => d.VehicleTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_1");
        });

        modelBuilder.Entity<VehicleType>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Id, "Index_1");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.CostType).WithMany(p => p.InverseCostType)
                .HasForeignKey(d => d.CostTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VehicleType_CostType_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
