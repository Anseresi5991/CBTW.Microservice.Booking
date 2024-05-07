using CBTW.Microservice.Booking.Domain.Entities;
using CBTW.Microservice.Booking.Domain.Reporitories;
using MongoDB.Driver;

namespace CBTW.Microservice.Booking.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IMongoCollection<Employee> _employees;

        public EmployeeRepository(IMongoDatabase database)
        {
            _employees = database.GetCollection<Employee>("Employees");
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            return await _employees.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employees.Find(_ => true).ToListAsync();
        }

        public async Task<Employee> AddAsync(Employee Employee)
        {
            await _employees.InsertOneAsync(Employee);
            return Employee;
        }

        public async Task UpdateAsync(Employee Employee)
        {
            await _employees.ReplaceOneAsync(a => a.Id == Employee.Id, Employee);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _employees.DeleteOneAsync(a => a.Id == id);
        }
    }
}
