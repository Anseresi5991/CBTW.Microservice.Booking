using CBTW.Microservice.Booking.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTW.Microservice.Booking.Domain.Tests.Entities
{
    public class ClientTests
    {
        [Fact]
        public void CreateClient_ValidData_ShouldSucceed()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "John Doe";
            var email = "john.doe@example.com";
            var phone = "123456789";

            // Act
            var client = new Client
            {
                Id = id,
                Name = name,
                Email = email,
                Phone = phone
            };

            // Assert
            client.Id.Should().Be(id);
            client.Name.Should().Be(name);
            client.Email.Should().Be(email);
            client.Phone.Should().Be(phone);
        }
    }
}
