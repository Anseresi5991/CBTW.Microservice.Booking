using AutoMapper;
using CBTW.Microservice.Booking.Application.DTOs;
using CBTW.Microservice.Booking.Application.Services.Implementations;
using CBTW.Microservice.Booking.Domain.Entities;
using CBTW.Microservice.Booking.Domain.Reporitories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTW.Microservice.Booking.Application.Tests.Services.Implementations
{
    public class ClientServiceTests
    {
        [Fact]
        public async Task GetAllClientsAsync_ShouldReturnAllClients()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client { Id = Guid.NewGuid(), Name = "Client 1", Email = "client1@example.com", Phone = "123456789" },
                new Client { Id = Guid.NewGuid(), Name = "Client 2", Email = "client2@example.com", Phone = "987654321" }
            };

            var clientRepositoryMock = new Mock<IClientRepository>();
            clientRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(clients);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<ClientDTO>>(It.IsAny<IEnumerable<Client>>()))
                      .Returns((IEnumerable<Client> clients) => clients.Select(client => new ClientDTO
                      {
                          Id = client.Id,
                          Name = client.Name,
                          Email = client.Email,
                          Phone = client.Phone
                      }));

            var clientService = new ClientService(clientRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await clientService.GetAllClientsAsync();

            // Assert
            Assert.Equal(clients.Count, result.ToList().Count);
            clientRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetClientByIdAsync_ExistingId_ShouldReturnClient()
        {
            // Arrange
            var id = Guid.NewGuid();
            var client = new Client { Id = id, Name = "Client 1", Email = "client1@example.com", Phone = "123456789" };

            var clientRepositoryMock = new Mock<IClientRepository>();
            clientRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(client);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<ClientDTO>(It.IsAny<Client>()))
                      .Returns((Client client) => new ClientDTO
                      {
                          Id = client.Id,
                          Name = client.Name,
                          Email = client.Email,
                          Phone = client.Phone
                      });

            var clientService = new ClientService(clientRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await clientService.GetClientByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetClientByIdAsync_NonExistingId_ShouldReturnNull()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            var clientRepositoryMock = new Mock<IClientRepository>();
            clientRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistingId)).ReturnsAsync((Client)null);

            var mapperMock = new Mock<IMapper>();

            var clientService = new ClientService(clientRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await clientService.GetClientByIdAsync(nonExistingId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateClientAsync_ValidData_ShouldReturnCreatedClient()
        {
            // Arrange
            var clientDto = new ClientDTO
            {
                Name = "Client 1",
                Email = "client1@example.com",
                Phone = "123456789"
            };

            var client = new Client { Id = Guid.NewGuid() };

            var clientRepositoryMock = new Mock<IClientRepository>();
            clientRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Client>())).ReturnsAsync(client);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<Client>(clientDto)).Returns(client);

            var clientService = new ClientService(clientRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await clientService.CreateClientAsync(clientDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(client.Id, result.Id);
        }
    }
}
