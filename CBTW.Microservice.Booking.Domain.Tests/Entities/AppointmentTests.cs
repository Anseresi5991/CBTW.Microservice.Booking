using CBTW.Microservice.Booking.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTW.Microservice.Booking.Domain.Tests.Entities
{
    public class AppointmentTests
    {
        [Fact]
        public void CreateAppointment_ValidData_ShouldSucceed()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dateTime = DateTime.Now;
            var duration = TimeSpan.FromHours(1);
            var clientId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();
            var status = AppointmentStatus.Scheduled;

            // Act
            var appointment = new Appointment
            {
                Id = id,
                DateTime = dateTime,
                ClientId = clientId,
                EmployeeId = employeeId,
                Status = status
            };

            // Assert
            appointment.Id.Should().Be(id);
            appointment.DateTime.Should().Be(dateTime);
            appointment.ClientId.Should().Be(clientId);
            appointment.EmployeeId.Should().Be(employeeId);
            appointment.Status.Should().Be(status);
        }

    }
}
