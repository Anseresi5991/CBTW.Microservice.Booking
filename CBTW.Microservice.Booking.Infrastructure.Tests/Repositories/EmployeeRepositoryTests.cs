using CBTW.Microservice.Booking.Domain.Entities;
using CBTW.Microservice.Booking.Infrastructure.Repositories;
using FluentAssertions;
using Mongo2Go;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTW.Microservice.Booking.Infrastructure.Tests.Repositories
{
    public class EmployeeRepositoryTests : IDisposable
    {
        private MongoDbRunner _runner;
        private IMongoDatabase _database;
        private IMongoCollection<Employee> _collection;
        private EmployeeRepository _repository;

        public EmployeeRepositoryTests()
        {
            _runner = MongoDbRunner.Start();
            var client = new MongoClient(_runner.ConnectionString);
            _database = client.GetDatabase("TestDatabase");
            _collection = _database.GetCollection<Employee>("Employees");
            _repository = new EmployeeRepository(_database);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }

        [Fact]
        public async Task AddAsync_ShouldAddEmployee()
        {
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Andres Rebolledo", Email = "asrs@example.com" };

            await _repository.AddAsync(employee);

            var result = await _collection.Find(e => e.Id == employee.Id).FirstOrDefaultAsync();
            result.Should().BeEquivalentTo(employee);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
        {
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Andres Rebolledo", Email = "asrs@example.com" };
            await _collection.InsertOneAsync(employee);

            var result = await _repository.GetByIdAsync(employee.Id);

            result.Should().BeEquivalentTo(employee);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEmployee()
        {
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Andres Rebolledo", Email = "asrs@example.com" };
            await _collection.InsertOneAsync(employee);

            await _repository.DeleteAsync(employee.Id);

            var result = await _collection.Find(e => e.Id == employee.Id).FirstOrDefaultAsync();
            result.Should().BeNull();
        }
    }
}
