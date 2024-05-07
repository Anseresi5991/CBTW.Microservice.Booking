using AutoMapper;
using CBTW.Microservice.Booking.Application.DTOs;
using CBTW.Microservice.Booking.Application.Services.Implementations;
using CBTW.Microservice.Booking.Domain.Entities;
using CBTW.Microservice.Booking.Domain.Reporitories;
using Moq;

namespace CBTW.Microservice.Booking.Application.Tests.Services.Implementations
{
    public class AppointmentServiceTests
    {
        [Fact]
        public async Task GetAllAppointmentsAsync_ShouldReturnAllAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
    {
        new Appointment { Id = Guid.NewGuid(), DateTime = DateTime.Now,  ClientId = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), Status = AppointmentStatus.Scheduled },
        new Appointment { Id = Guid.NewGuid(), DateTime = DateTime.Now.AddHours(2), ClientId = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), Status = AppointmentStatus.Completed }
    };

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            appointmentRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(appointments);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<AppointmentDTO>>(It.IsAny<IEnumerable<Appointment>>()))
                      .Returns((IEnumerable<Appointment> appointments) => appointments.Select(appointment => new AppointmentDTO
                      {
                          Id = appointment.Id,
                          DateTime = appointment.DateTime,
                          ClientId = appointment.ClientId,
                          EmployeeId = appointment.EmployeeId,
                          Status = appointment.Status.ToString()
                      }));

            var appointmentService = new AppointmentService(appointmentRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await appointmentService.GetAllAppointmentsAsync();

            // Assert
            Assert.Equal(appointments.Count, result.ToList().Count);

            // Verificación
            appointmentRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }


        [Fact]
        public async Task GetAppointmentByIdAsync_ExistingId_ShouldReturnAppointment()
        {
            // Arrange
            var id = Guid.NewGuid();
            var appointment = new Appointment { Id = id, DateTime = DateTime.Now, ClientId = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), Status = AppointmentStatus.Scheduled };

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(appointment);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<AppointmentDTO>(It.IsAny<Appointment>()))
                      .Returns((Appointment appointment) => new AppointmentDTO
                      {
                          Id = appointment.Id,
                          DateTime = appointment.DateTime,
                          ClientId = appointment.ClientId,
                          EmployeeId = appointment.EmployeeId,
                          Status = appointment.Status.ToString()
                      });

            var appointmentService = new AppointmentService(appointmentRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await appointmentService.GetAppointmentByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_NonExistingId_ShouldReturnNull()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistingId)).ReturnsAsync((Appointment)null);

            var mapperMock = new Mock<IMapper>();

            var appointmentService = new AppointmentService(appointmentRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await appointmentService.GetAppointmentByIdAsync(nonExistingId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAppointmentAsync_ValidData_ShouldReturnCreatedAppointment()
        {
            // Arrange
            var appointmentDto = new AppointmentDTO
            {
                DateTime = DateTime.Now,
                ClientId = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                Status = "Scheduled"
            };

            var appointment = new Appointment { Id = Guid.NewGuid() };

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            appointmentRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Appointment>())).ReturnsAsync(appointment);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<Appointment>(appointmentDto)).Returns(appointment);

            var appointmentService = new AppointmentService(appointmentRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await appointmentService.CreateAppointmentAsync(appointmentDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(appointment.Id, result.Id);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_ExistingAppointment_ShouldUpdateAppointment()
        {
            // Arrange
            var appointmentDto = new AppointmentDTO
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                ClientId = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                Status = "Completed"
            };

            var existingAppointment = new Appointment { Id = appointmentDto.Id };

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(appointmentDto.Id)).ReturnsAsync(existingAppointment);

            var mapperMock = new Mock<IMapper>();

            var appointmentService = new AppointmentService(appointmentRepositoryMock.Object, mapperMock.Object);

            // Act
            await appointmentService.UpdateAppointmentAsync(appointmentDto);

            // Assert
            appointmentRepositoryMock.Verify(repo => repo.UpdateAsync(existingAppointment), Times.Once);
            Assert.Equal(AppointmentStatus.Completed, existingAppointment.Status);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_NonExistingAppointment_ShouldNotUpdateAppointment()
        {
            // Arrange
            var nonExistingAppointmentDto = new AppointmentDTO
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                ClientId = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                Status = "Completed"
            };

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistingAppointmentDto.Id)).ReturnsAsync((Appointment)null);

            var mapperMock = new Mock<IMapper>();

            var appointmentService = new AppointmentService(appointmentRepositoryMock.Object, mapperMock.Object);

            // Act
            await appointmentService.UpdateAppointmentAsync(nonExistingAppointmentDto);

            // Assert
            appointmentRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_ExistingId_ShouldDeleteAppointment()
        {
            // Arrange
            var idToDelete = Guid.NewGuid();

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            var mapperMock = new Mock<IMapper>();

            var appointmentService = new AppointmentService(appointmentRepositoryMock.Object, mapperMock.Object);

            // Act
            await appointmentService.DeleteAppointmentAsync(idToDelete);

            // Assert
            appointmentRepositoryMock.Verify(repo => repo.DeleteAsync(idToDelete), Times.Once);
        }
    }
}
