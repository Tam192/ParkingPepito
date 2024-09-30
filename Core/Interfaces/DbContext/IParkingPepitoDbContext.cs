using Core.Entities;
using Core.Views;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces.DbContext
{
    public interface IParkingPepitoDbContext : IDisposable, IAsyncDisposable
    {
        DbSet<CostType> CostType { get; set; }

        DbSet<Employee> Employee { get; set; }

        DbSet<ResidentDebts> ResidentDebts { get; set; }

        DbSet<Stay> Stay { get; set; }

        DbSet<Vehicle> Vehicle { get; set; }

        DbSet<VehicleType> VehicleType { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
