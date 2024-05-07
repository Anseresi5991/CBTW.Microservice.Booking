using CBTW.Microservice.Booking.Domain.Entities;
using CBTW.Microservice.Booking.Infrastructure.Repositories;
using FluentAssertions;
using Mongo2Go;
using MongoDB.Driver;

namespace CBTW.Microservice.Booking.Infrastructure.Tests.Repositories
{
    public class AppointmentRepositoryTests : IDisposable
    {
        private MongoDbRunner _runner;
        private IMongoDatabase _database;
        private IMongoCollection<Appointment> _collection;
        private AppointmentRepository _repository;

        public AppointmentRepositoryTests()
        {
            _runner = MongoDbRunner.Start();
            var client = new MongoClient(_runner.ConnectionString);
            _database = client.GetDatabase("TestDatabase");
            _collection = _database.GetCollection<Appointment>("Appointments");
            _repository = new AppointmentRepository(_database);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }

        [Fact]
        public async Task AddAsync_ShouldAddAppointment()
        {
            var appointment = new Appointment { Id = Guid.NewGuid(), Status = AppointmentStatus.Scheduled };

            await _repository.AddAsync(appointment);

            var result = await _collection.Find(a => a.Id == appointment.Id).FirstOrDefaultAsync();
            result.Should().BeEquivalentTo(appointment);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAppointment_WhenAppointmentExists()
        {
            var appointment = new Appointment { Id = Guid.NewGuid(), Status = AppointmentStatus.Scheduled };
            await _collection.InsertOneAsync(appointment);

            var result = await _repository.GetByIdAsync(appointment.Id);

            result.Should().BeEquivalentTo(appointment);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveAppointment()
        {
            var appointment = new Appointment { Id = Guid.NewGuid(), Status = AppointmentStatus.Scheduled };
            await _collection.InsertOneAsync(appointment);

            await _repository.DeleteAsync(appointment.Id);

            var result = await _collection.Find(a => a.Id == appointment.Id).FirstOrDefaultAsync();
            result.Should().BeNull();
        }
    }
}
