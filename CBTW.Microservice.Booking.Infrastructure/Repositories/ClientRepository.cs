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
    public class ClientRepository : IClientRepository
    {
        private readonly IMongoCollection<Client> _clients;

        public ClientRepository(IMongoDatabase database)
        {
            _clients = database.GetCollection<Client>("Clients");
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            return await _clients.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _clients.Find(_ => true).ToListAsync();
        }

        public async Task<Client> AddAsync(Client Client)
        {
            await _clients.InsertOneAsync(Client);
            return Client;
        }

        public async Task UpdateAsync(Client Client)
        {
            await _clients.ReplaceOneAsync(a => a.Id == Client.Id, Client);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _clients.DeleteOneAsync(a => a.Id == id);
        }
    }
}
