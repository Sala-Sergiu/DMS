using DMS.DAL.Persistence.Configurations;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Persistence;

public class DmsDbContext : DbContext
{
    public DmsDbContext(DbContextOptions<DmsDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<DeviceAssignment> DeviceAssignments => Set<DeviceAssignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new DeviceConfiguration());
        modelBuilder.ApplyConfiguration(new DeviceAssignmentConfiguration());
    }
}