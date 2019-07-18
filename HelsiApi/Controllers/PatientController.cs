using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Enums;
using Common.Models.ServiceReponses;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Services.Abstractions;
using Services.DtoModels;

namespace HelsiApi.Controllers
{
    [Route("patients")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(ServiceBaseResult<SearchOperationStatus, List<PatientDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ServiceBaseResult<CreateOperationStatus>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ServiceBaseResult<CreateOperationStatus>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var result = await _patientService.GetAll();
            switch (result.OperationStatus)
            {
                case SearchOperationStatus.Success:
                    return Ok(result);
                case SearchOperationStatus.NotFound:
                    return NotFound(result);
                default:
                    return BadRequest(result);
            }
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(ServiceBaseResult<CreateOperationStatus>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ServiceBaseResult<CreateOperationStatus>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] PatientDto patientRequst)
        {
            var result = await _patientService.CreatePatient(patientRequst);

            switch (result.OperationStatus)
            {
                case CreateOperationStatus.Ok:
                    return Ok(result);
                default:
                    return BadRequest(result);
            }
        }

        [HttpPut("")]
        [ProducesResponseType(typeof(ServiceBaseResult<UpdateStatus>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ServiceBaseResult<UpdateStatus>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ServiceBaseResult<UpdateStatus>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Put([FromBody] PatientDto updateRequest)
        {
            var result = await _patientService.UpdatePatient(updateRequest);
            switch (result.OperationStatus)
            {
                case UpdateStatus.Ok:
                    return Ok(result);
                case UpdateStatus.NotFound:
                    return NotFound(result);
                default:
                    return BadRequest(result);
            }
        }

        [ProducesResponseType(typeof(ServiceBaseResult<UpdateStatus>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ServiceBaseResult<UpdateStatus>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ServiceBaseResult<UpdateStatus>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Diactivate(string userId)
        {
            var result = await _patientService.DeactivatePatient(userId);

            switch (result.OperationStatus)
            {
                case PatientActivatingStatus.Diactivated:
                    return Ok(result);
                case PatientActivatingStatus.NotFound:
                    return NotFound(result);
                default:
                    return BadRequest(result);
            }
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(ServiceBaseResult<SearchOperationStatus, List<PatientDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ServiceBaseResult<CreateOperationStatus>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ServiceBaseResult<CreateOperationStatus>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Search(string query, int page, int pageSize)
        {
            var result = await _patientService.Search(query, page, pageSize);
            switch (result.OperationStatus)
            {
                case SearchOperationStatus.Success:
                    return Ok(result);
                case SearchOperationStatus.NotFound:
                    return NotFound(result);
                default:
                    return BadRequest(result);
            }
        }
    }
}