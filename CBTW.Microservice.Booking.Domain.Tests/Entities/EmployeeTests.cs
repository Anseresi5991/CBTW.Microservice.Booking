using CBTW.Microservice.Booking.Domain.Entities;
using FluentAssertions;

namespace CBTW.Microservice.Booking.Domain.Tests.Entities
{
    public class EmployeeTests
    {
        [Fact]
        public void CreateEmployee_ValidData_ShouldSucceed()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Jane Smith";
            var email = "jane.smith@example.com";
            var phone = "987654321";
            var position = "Manager";

            // Act
            var employee = new Employee
            {
                Id = id,
                Name = name,
                Email = email,
                Phone = phone,
                Position = position
            };

            // Assert
            employee.Id.Should().Be(id);
            employee.Name.Should().Be(name);
            employee.Email.Should().Be(email);
            employee.Phone.Should().Be(phone);
            employee.Position.Should().Be(position);
        }
    }
}
