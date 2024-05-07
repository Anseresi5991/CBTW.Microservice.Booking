using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTW.Microservice.Booking.Application.DTOs
{
    public class AppointmentDTO
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public Guid ClientId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Status { get; set; }
    }
}
