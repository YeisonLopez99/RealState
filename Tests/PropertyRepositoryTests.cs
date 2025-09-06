using Azure.Core;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class PropertyRepositoryTests
    {
        private ApplicationDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddAsyncShouldPersistProperty()
        {
            //Arrange
            var context = GetDbContext("TestDb_Add");
            var repository = new EfRepository<Property>(context);
            var entity = new Property("Casa Bonita", "Calle 123", 250000, Guid.Parse("1F9E5C5D-9989-4C16-88CE-36063B0A457E"),"13254687");

            //Act
            await repository.AddAsync(entity);
            await context.SaveChangesAsync();

            //Assert
            var saved = await context.Properties.FirstOrDefaultAsync(p => p.IdProperty == entity.IdProperty);
            saved.Should().NotBeNull();
            saved!.Name.Should().Be("Casa Bonita");

        }

        [Fact]
        public async Task GetAllAsyncShouldReturnPropertis()
        {
            var context = GetDbContext("TestDb_GetAll");
            context.Properties.Add(new Property("Apartamento Centro", "Calle 123", 250000, Guid.Parse("1F9E5C5D-9989-4C16-88CE-36063B0A457E"), "13254687"));
            context.Properties.Add(new Property("Casa Norte", "Calle 123", 250000, Guid.Parse("1F9E5C5D-9989-4C16-88CE-36063B0A457E"), "13254687"));
            await context.SaveChangesAsync();

            var repository = new EfRepository<Property>(context);

            // Act
            var result = await repository.ListAsync(new CancellationToken());

            // Assert
            result.Should().HaveCount(2);
            result[0].Name.Should().Contain("Apartamento Centro");
            result[1].Name.Should().Contain("Casa Norte");
        }

    }
}
