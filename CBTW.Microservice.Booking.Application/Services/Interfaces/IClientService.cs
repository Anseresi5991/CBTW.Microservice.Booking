using CBTW.Microservice.Booking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTW.Microservice.Booking.Application.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDTO>> GetAllClientsAsync();
        Task<ClientDTO> GetClientByIdAsync(Guid id);
        Task<ClientDTO> CreateClientAsync(ClientDTO clientDto);
        Task UpdateClientAsync(ClientDTO clientDto);
        Task DeleteClientAsync(Guid id);
    }
}
