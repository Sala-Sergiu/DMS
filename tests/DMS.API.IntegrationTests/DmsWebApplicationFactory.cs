using DMS.DAL.Persistence;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.API.IntegrationTests;

public class DmsWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = "DmsIntegrationTestDb_" + Guid.NewGuid();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DmsDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<DmsDbContext>(options =>
                options.UseInMemoryDatabase(_dbName));
        });
    }

    public void SeedDatabase()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DmsDbContext>();
        db.Database.EnsureCreated();

        if (db.Devices.Any()) return;

        db.Devices.AddRange(
            new Device("iPhone 15 Pro", "SN-001", "AT-001", "Apple", "iPhone 15 Pro", DeviceType.Smartphone),
            new Device("Samsung Galaxy S24", "SN-002", "AT-002", "Samsung", "Galaxy S24", DeviceType.Smartphone),
            new Device("iPad Pro 12.9", "SN-003", "AT-003", "Apple", "iPad Pro M2", DeviceType.Tablet),
            new Device("Samsung Galaxy Tab S9", "SN-004", "AT-004", "Samsung", "Galaxy Tab S9", DeviceType.Tablet),
            new Device("Dell Latitude 5540", "SN-005", "AT-005", "Dell", "Latitude 5540", DeviceType.Laptop)
        );

        db.SaveChanges();
    }
}