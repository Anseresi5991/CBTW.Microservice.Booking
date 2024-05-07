using CBTW.Microservice.Booking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTW.Microservice.Booking.Application.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsAsync();
        Task<AppointmentDTO> GetAppointmentByIdAsync(Guid id);
        Task<AppointmentDTO> CreateAppointmentAsync(AppointmentDTO appointmentDto);
        Task UpdateAppointmentAsync(AppointmentDTO appointmentDto);
        Task DeleteAppointmentAsync(Guid id);
    }
}
