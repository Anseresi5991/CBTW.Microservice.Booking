using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTW.Microservice.Booking.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public Guid ClientId { get; set; }
        public Guid EmployeeId { get; set; }
        public AppointmentStatus Status { get; set; }
    }

    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled,
        NoShow
    }
}
