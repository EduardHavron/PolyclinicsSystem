using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyclinicsSystemBackend.Dtos.Appointment;
using PolyclinicsSystemBackend.Dtos.Generics;
using PolyclinicsSystemBackend.Services.Appointment.Interface;

namespace PolyclinicsSystemBackend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("appointment")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
        }

        [HttpGet]
        [Route("getAppointment/{appointmentId}")]
        public async Task<IActionResult> GetAppointment(int appointmentId, bool includeDiagnose)
        {
            var result = await _appointmentService.GetAppointment(appointmentId, includeDiagnose);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }
        
        [HttpGet]
        [Route("getAppointmentsDoctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsForDoctor(string doctorId,
            bool includeDiagnose)
        {
            var result = await _appointmentService.GetAppointmentsForDoctor(doctorId, includeDiagnose);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }
        
        [HttpPost]
        [Authorize(Roles = "Patient")]
        [Route("create")]
        public async Task<IActionResult> CreateAppointment(string doctorId, string patientId, DateTime appointmentDate)
        {
            var result = await _appointmentService.CreateAppointment(doctorId, patientId, appointmentDate);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }
        
        [HttpPatch]
        [Authorize(Roles = "Patient")]
        [Route("reschedule")]
        public async Task<IActionResult> RescheduleAppointment(int appointmentId, DateTime newDate)
        {
            var result = await _appointmentService.RescheduleAppointment(appointmentId, newDate);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }
        
        [HttpPatch]
        [Authorize(Roles = "Doctor")]
        [Route("start")]
        public async Task<IActionResult> StartAppointment(int appointmentId)
        {
            var result = await _appointmentService.StartAppointment(appointmentId);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }

        [HttpPatch]
        [Authorize(Roles = "Doctor")]
        [Route("finalize")]
        public async Task<IActionResult> FinalizeAppointment(int appointmentId)
        {
            var result = await _appointmentService.FinalizeAppointment(appointmentId);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }

        [HttpDelete]
        [Authorize(Roles = "Patient")]
        [Route("cancel")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var result = await _appointmentService.CancelAppointment(appointmentId);
            return result ? Ok() : BadRequest();
        }
    }
}