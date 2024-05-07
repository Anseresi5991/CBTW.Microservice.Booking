using AutoMapper;
using CBTW.Microservice.Booking.Application.DTOs;
using CBTW.Microservice.Booking.Application.Services.Interfaces;
using CBTW.Microservice.Booking.Domain.Entities;
using CBTW.Microservice.Booking.Domain.Reporitories;

namespace CBTW.Microservice.Booking.Application.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientService(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDTO>> GetAllClientsAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClientDTO>>(clients);
        }

        public async Task<ClientDTO> GetClientByIdAsync(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            return client == null ? null : _mapper.Map<ClientDTO>(client);
        }

        public async Task<ClientDTO> CreateClientAsync(ClientDTO clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            await _clientRepository.AddAsync(client);
            clientDto.Id = client.Id;
            return clientDto;
        }

        public async Task UpdateClientAsync(ClientDTO clientDto)
        {
            var client = await _clientRepository.GetByIdAsync(clientDto.Id);
            if (client != null)
            {
                _mapper.Map(clientDto, client);
                await _clientRepository.UpdateAsync(client);
            }
        }

        public async Task DeleteClientAsync(Guid id)
        {
            await _clientRepository.DeleteAsync(id);
        }
    }
}
