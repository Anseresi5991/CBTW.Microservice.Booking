using AutoMapper;
using CBTW.Microservice.Booking.Application.DTOs;
using CBTW.Microservice.Booking.Application.Services.Interfaces;
using CBTW.Microservice.Booking.Domain.Entities;
using CBTW.Microservice.Booking.Domain.Reporitories;

namespace CBTW.Microservice.Booking.Application.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
            return result;
        }

        public async Task<AppointmentDTO> GetAppointmentByIdAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                return null;

            return _mapper.Map<AppointmentDTO>(appointment);
        }

        public async Task<AppointmentDTO> CreateAppointmentAsync(AppointmentDTO appointmentDto)
        {
            var appointment = _mapper.Map<Appointment>(appointmentDto);
            await _appointmentRepository.AddAsync(appointment);
            appointmentDto.Id = appointment.Id;
            return appointmentDto;
        }

        public async Task UpdateAppointmentAsync(AppointmentDTO appointmentDto)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentDto.Id);
            if (appointment == null)
                return;

            appointment.DateTime = appointmentDto.DateTime;
            appointment.ClientId = appointmentDto.ClientId;
            appointment.EmployeeId = appointmentDto.EmployeeId;
            appointment.Status = Enum.Parse<AppointmentStatus>(appointmentDto.Status);

            await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task DeleteAppointmentAsync(Guid id)
        {
            await _appointmentRepository.DeleteAsync(id);
        }
    }
}
