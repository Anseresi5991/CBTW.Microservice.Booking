using CBTW.Microservice.Booking.Domain.Entities;
using CBTW.Microservice.Booking.Domain.Reporitories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTW.Microservice.Booking.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IMongoCollection<Appointment> _appointments;

        public AppointmentRepository(IMongoDatabase database)
        {
            _appointments = database.GetCollection<Appointment>("Appointments");
        }

        public async Task<Appointment> GetByIdAsync(Guid id)
        {
            return await _appointments.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _appointments.Find(_ => true).ToListAsync();
        }

        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            await _appointments.InsertOneAsync(appointment);
            return appointment;
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            await _appointments.ReplaceOneAsync(a => a.Id == appointment.Id, appointment);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _appointments.DeleteOneAsync(a => a.Id == id);
        }
    }
}
