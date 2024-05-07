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
    public class EmployeeServiceTests
    {
        [Fact]
        public async Task GetAllEmployeesAsync_ShouldReturnAllEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = Guid.NewGuid(), Name = "Employee 1", Email = "employee1@example.com", Phone = "123456789", Position = "Developer" },
                new Employee { Id = Guid.NewGuid(), Name = "Employee 2", Email = "employee2@example.com", Phone = "987654321", Position = "Manager" }
            };

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(employees);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<EmployeeDTO>>(It.IsAny<IEnumerable<Employee>>()))
                      .Returns((IEnumerable<Employee> employees) => employees.Select(employee => new EmployeeDTO
                      {
                          Id = employee.Id,
                          Name = employee.Name,
                          Email = employee.Email,
                          Phone = employee.Phone,
                          Position = employee.Position
                      }));

            var employeeService = new EmployeeService(employeeRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await employeeService.GetAllEmployeesAsync();

            // Assert
            Assert.Equal(employees.Count, result.ToList().Count);
            employeeRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ExistingId_ShouldReturnEmployee()
        {
            // Arrange
            var id = Guid.NewGuid();
            var employee = new Employee { Id = id, Name = "Employee 1", Email = "employee1@example.com", Phone = "123456789", Position = "Developer" };

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(employee);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<EmployeeDTO>(It.IsAny<Employee>()))
                      .Returns((Employee employee) => new EmployeeDTO
                      {
                          Id = employee.Id,
                          Name = employee.Name,
                          Email = employee.Email,
                          Phone = employee.Phone,
                          Position = employee.Position
                      });

            var employeeService = new EmployeeService(employeeRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await employeeService.GetEmployeeByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_NonExistingId_ShouldReturnNull()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistingId)).ReturnsAsync((Employee)null);

            var mapperMock = new Mock<IMapper>();

            var employeeService = new EmployeeService(employeeRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await employeeService.GetEmployeeByIdAsync(nonExistingId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateEmployeeAsync_ValidData_ShouldReturnCreatedEmployee()
        {
            // Arrange
            var employeeDto = new EmployeeDTO
            {
                Name = "Employee 1",
                Email = "employee1@example.com",
                Phone = "123456789",
                Position = "Developer"
            };

            var employee = new Employee { Id = Guid.NewGuid() };

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Employee>())).ReturnsAsync(employee);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<Employee>(employeeDto)).Returns(employee);

            var employeeService = new EmployeeService(employeeRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await employeeService.CreateEmployeeAsync(employeeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employee.Id, result.Id);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_NonExistingEmployee_ShouldNotUpdateEmployee()
        {
            // Arrange
            var nonExistingEmployeeDto = new EmployeeDTO
            {
                Id = Guid.NewGuid(),
                Name = "Employee 1",
                Email = "employee1@example.com",
                Phone = "123456789",
                Position = "Developer"
            };

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistingEmployeeDto.Id)).ReturnsAsync((Employee)null);

            var mapperMock = new Mock<IMapper>();

            var employeeService = new EmployeeService(employeeRepositoryMock.Object, mapperMock.Object);

            // Act
            await employeeService.UpdateEmployeeAsync(nonExistingEmployeeDto);

            // Assert
            employeeRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ExistingId_ShouldDeleteEmployee()
        {
            // Arrange
            var idToDelete = Guid.NewGuid();

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var mapperMock = new Mock<IMapper>();

            var employeeService = new EmployeeService(employeeRepositoryMock.Object, mapperMock.Object);

            // Act
            await employeeService.DeleteEmployeeAsync(idToDelete);

            // Assert
            employeeRepositoryMock.Verify(repo => repo.DeleteAsync(idToDelete), Times.Once);
        }
    }
}
