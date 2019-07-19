using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.Enums;
using Common.Extensions;
using Common.Models.ServiceReponses;
using Data.Models;
using Data.Repos.Abstractions;
using Nest;
using Services.Abstractions;
using Services.DtoModels;
using Patient = Data.Models.Patient;

namespace Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly IElasticClient _elasticClient;

        public PatientService(IPatientRepository patientRepository, IMapper mapper, IElasticClient elasticClient)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _elasticClient = elasticClient;
        }

        public async Task<ServiceBaseResult<SearchOperationStatus, List<PatientDto>>> GetAll()
        {
            var patientList = await _patientRepository.GetAll();

            if (!patientList.Any())
            {
                return new ServiceBaseResult<SearchOperationStatus, List<PatientDto>>(SearchOperationStatus.NotFound,
                    SearchOperationStatus.NotFound.GetDescription());
            }
            var patientDto = _mapper.Map<List<PatientDto>>(patientList);

            return new ServiceBaseResult<SearchOperationStatus, List<PatientDto>>(SearchOperationStatus.Success,
                SearchOperationStatus.Success.GetDescription(), patientDto);
        }

        public async Task<ServiceBaseResult<SearchOperationStatus, IReadOnlyCollection<PatientDto>>> Search(string query, int page, int pageSize)
        {
            var searchResult = await _elasticClient.SearchAsync<PatientDto>(s => s
                .From((page - 1) * pageSize)
                .Size(pageSize)
                .Query(qry =>
                        qry.QueryString(q =>
                            q.Fields(f =>
                                f.Fields(a => a.FirstName, a => a.LastName, a => a.Phone)).Query("*" + query + "*"))
                ));

            if (!searchResult.Documents.Any())
            {
                return new ServiceBaseResult<SearchOperationStatus, IReadOnlyCollection<PatientDto>>(SearchOperationStatus.NotFound,
                    SearchOperationStatus.NotFound.GetDescription());
            }

            return new ServiceBaseResult<SearchOperationStatus, IReadOnlyCollection<PatientDto>>(SearchOperationStatus.Success,
                SearchOperationStatus.Success.GetDescription(), searchResult.Documents); ;
        }

        public async Task<ServiceBaseResult<CreateOperationStatus>> CreatePatient(PatientDto patientDto)
        {
            var checkPatient = await _patientRepository.GetByPhone(patientDto.Phone);
            if (checkPatient != null)
            {
                return new ServiceBaseResult<CreateOperationStatus>(CreateOperationStatus.PatientExists,
                    CreateOperationStatus.PatientExists.GetDescription().Replace("<PHONE>", patientDto.Phone));
            }

            var patient = _mapper.Map<Patient>(patientDto);
            patient.UserId = Guid.NewGuid().ToString();
            await _patientRepository.Create(patient);
            await _elasticClient.IndexDocumentAsync(patientDto);
            return new ServiceBaseResult<CreateOperationStatus>(CreateOperationStatus.Ok,
                CreateOperationStatus.Ok.GetDescription());
        }

        public async Task<ServiceBaseResult<UpdateStatus>> UpdatePatient(PatientDto patientDto)
        {
            var checkPatient = await _patientRepository.GetByUserId(patientDto.UserId);
            if (checkPatient == null)
            {
                return new ServiceBaseResult<UpdateStatus>(UpdateStatus.NotFound,
                    UpdateStatus.NotFound.GetDescription());
            }

            patientDto.Activated = checkPatient.Activated;
            var patient = _mapper.Map<Patient>(patientDto);
            await _patientRepository.Update(patient);
            await _elasticClient.UpdateAsync<Patient>(patient, u => u.Doc(patient));
            return new ServiceBaseResult<UpdateStatus>(UpdateStatus.Ok,
                UpdateStatus.Ok.GetDescription());
        }

        public async Task<ServiceBaseResult<PatientActivatingStatus>> DeactivatePatient(string userId)
        {
            var patient = await _patientRepository.GetByUserId(userId);
            if (patient == null)
            {
                return new ServiceBaseResult<PatientActivatingStatus>(PatientActivatingStatus.NotFound,
                    PatientActivatingStatus.NotFound.GetDescription());
            }

            patient.Activated = false;
            await _patientRepository.Update(patient);
            await _elasticClient.UpdateAsync<Patient>(patient, u => u.Doc(patient));
            return new ServiceBaseResult<PatientActivatingStatus>(PatientActivatingStatus.Diactivated,
                PatientActivatingStatus.Diactivated.GetDescription());
        }

        public async Task<ServiceBaseResult<PatientActivatingStatus>> ActivatePatient(string userId)
        {
            //This method added just in case if we need some special logic or business process for activating 

            var patient = await _patientRepository.GetByUserId(userId);
            if (patient == null)
            {
                return new ServiceBaseResult<PatientActivatingStatus>(PatientActivatingStatus.NotFound,
                    PatientActivatingStatus.NotFound.GetDescription());
            }

            patient.Activated = true;
            await _patientRepository.Update(patient);
            await _elasticClient.UpdateAsync<Patient>(patient, u => u.Doc(patient));
            return new ServiceBaseResult<PatientActivatingStatus>(PatientActivatingStatus.Diactivated,
                PatientActivatingStatus.Diactivated.GetDescription());
        }

        public async Task ReIndex()
        {
            await _elasticClient.DeleteByQueryAsync<PatientDto>(q => q.MatchAll());

            var allPatients = await _patientRepository.GetAll();

            var dtos = _mapper.Map<List<PatientDto>>(allPatients);

            foreach (var patient in dtos)
            {
                await _elasticClient.IndexDocumentAsync(patient);
            }

        }
    }
}
