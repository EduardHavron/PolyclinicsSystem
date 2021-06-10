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

        [HttpGet]
        [Route("getAppointmentsPatient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsForPatient(string patientId, bool includeDiagnose)
        {
            var result = await _appointmentService.GetAppointmentsForPatient(patientId, includeDiagnose);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }
        
        [HttpPost]
        [Authorize(Roles = "Patient,Admin")]
        [Route("create")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentDtoPost appointmentDtoPost)
        {
            appointmentDtoPost.AppointmentDate = appointmentDtoPost.AppointmentDate.ToLocalTime();
            var result = await _appointmentService.CreateAppointment(appointmentDtoPost);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }
        
        [HttpPatch]
        [Authorize(Roles = "Patient,Admin")]
        [Route("reschedule/{appointmentId}")]
        public async Task<IActionResult> RescheduleAppointment(int appointmentId, [FromBody] RescheduleAppointmentDto newDate)
        {
            newDate.NewDate = newDate.NewDate.ToLocalTime();
            var result = await _appointmentService.RescheduleAppointment(appointmentId, newDate.NewDate);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }
        
        [HttpPatch]
        [Authorize(Roles = "Doctor,Admin")]
        [Route("start/{appointmentId}")]
        public async Task<IActionResult> StartAppointment(int appointmentId)
        {
            var result = await _appointmentService.StartAppointment(appointmentId);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }

        [HttpPatch]
        [Authorize(Roles = "Doctor,Admin")]
        [Route("finalize/{appointmentId}")]
        public async Task<IActionResult> FinalizeAppointment(int appointmentId)
        {
            var result = await _appointmentService.FinalizeAppointment(appointmentId);
            return result.IsSuccess ? Ok(result.Result) : BadRequest(result.Errors);
        }

        [HttpDelete]
        [Authorize(Roles = "Patient,Admin")]
        [Route("cancel/{appointmentId}")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var result = await _appointmentService.CancelAppointment(appointmentId);
            return result ? Ok() : BadRequest();
        }
    }
}