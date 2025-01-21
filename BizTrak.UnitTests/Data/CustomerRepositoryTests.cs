using System;
using System.Linq.Expressions;
using BizTrak.Data;
using BizTrak.Domain.Entities;
using BizTrak.DTO;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BizTrak.UnitTests.Data;

public class CustomerRepositoryTests : IDisposable
{
    private readonly DbContextOptions<BizTrakDbContext> _options;
    private readonly BizTrakDbContext _seedContext;

    public CustomerRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<BizTrakDbContext>()
           .UseInMemoryDatabase(databaseName: $"BizTrakTestDb_{Guid.NewGuid()}")
           .Options;
        _seedContext = new BizTrakDbContext(_options);
        _seedContext.Database.EnsureCreated();
        InMemoryData.SeedDatabase(_seedContext);
        _seedContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetCustomerAsync_ShouldReturnCustomerById_WhenCalled()
    {
        using (var context = new BizTrakDbContext(_options))
        {
            var repository = new CustomerRepository(context);
            var result = await repository.GetCustomerAsync(101);

            Assert.NotNull(result);

            Assert.Equal(101, result.Id);
        }
    }

    [Fact]
    public async Task AddAsync_AddsACustomerToTheDatabase_WhenCalled()
    {
        using (var context = new BizTrakDbContext(_options))
        {
            var customer = new Customer { Name = "Customer 202", Address = "Address 202" };
            var repository = new CustomerRepository(context);

            await repository.AddAsync(customer);

            Assert.Single(context.Customers.Local);
            Assert.Same(customer, context.Customers.Local.First());
        }
    }

    [Fact]
    public async Task UpdateAsync_UpdatesACustomer_WhenCalled()
    {
        using var context = new BizTrakDbContext(_options);

        var repository = new CustomerRepository(context);

        var customer = await repository.GetCustomerAsync(101);
        var updatedCustomer = new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address,
            Email = customer.Email,
            Phone = customer.Phone
        };

        updatedCustomer.Name = "Updated Customer Name";
        repository.UpdateAsync(customer, updatedCustomer.Adapt<Customer>());
        var entry = context.Entry(customer);
        var nameProperty = entry.Property(p => p.Name);

        Assert.Equal(1, entry.Properties.Count(p => p.IsModified == true));
        Assert.True(nameProperty.IsModified);
        Assert.Equal("Updated Customer Name", customer.Name);
    }

    public void Dispose()
    {
        _seedContext.Database.EnsureDeleted();
        _seedContext.Dispose();
    }
}
