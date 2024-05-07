using CBTW.Microservice.Booking.Domain.Entities;
using CBTW.Microservice.Booking.Infrastructure.Repositories;
using FluentAssertions;
using Mongo2Go;
using MongoDB.Driver;

namespace CBTW.Microservice.Booking.Infrastructure.Tests.Repositories
{
    public class ClientRepositoryTests : IDisposable
    {
        private MongoDbRunner _runner;
        private IMongoDatabase _database;
        private IMongoCollection<Client> _collection;
        private ClientRepository _repository;

        public ClientRepositoryTests()
        {
            _runner = MongoDbRunner.Start();
            var client = new MongoClient(_runner.ConnectionString);
            _database = client.GetDatabase("TestDatabase");
            _collection = _database.GetCollection<Client>("Clients");
            _repository = new ClientRepository(_database);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }

        [Fact]
        public async Task AddAsync_ShouldAddClient()
        {
            var client = new Client { Id = Guid.NewGuid(), Name = "Andres Rebolledo", Email = "asrs@example.com" };

            await _repository.AddAsync(client);

            var result = await _collection.Find(c => c.Id == client.Id).FirstOrDefaultAsync();
            result.Should().BeEquivalentTo(client);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnClient_WhenClientExists()
        {
            var client = new Client { Id = Guid.NewGuid(), Name = "Andres Rebolledo", Email = "asrs@example.com" };
            await _collection.InsertOneAsync(client);

            var result = await _repository.GetByIdAsync(client.Id);

            result.Should().BeEquivalentTo(client);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveClient()
        {
            var client = new Client { Id = Guid.NewGuid(), Name = "Andres Rebolledo", Email = "asrs@example.com" };
            await _collection.InsertOneAsync(client);

            await _repository.DeleteAsync(client.Id);

            var result = await _collection.Find(c => c.Id == client.Id).FirstOrDefaultAsync();
            result.Should().BeNull();
        }
    }
}
