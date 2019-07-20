using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Enums;
using Common.Extensions;
using Common.Models.SearchModels;
using Common.Models.ServiceReponses;
using Data.Repos.Abstractions;
using Services.Abstractions;
using Services.DtoModels;
using Data.Models;

namespace Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly ISearchable<PatientSearchDocument> _patientSearch;

        public PatientService(IPatientRepository patientRepository, IMapper mapper, ISearchable<PatientSearchDocument> patientSearch)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _patientSearch = patientSearch;
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

        public async Task<ServiceBaseResult<SearchOperationStatus, IReadOnlyCollection<PatientSearchDocument>>> Search(string query, int page, int pageSize)
        {
            var searchResult = await _patientSearch.Search(query, page, pageSize, x => x.FirstName, x => x.Birthday,
                x => x.LastName, x => x.Phone);

            return searchResult;
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

            var searchDoc = _mapper.Map<PatientSearchDocument>(patient);
            await _patientSearch.AddToSearch(searchDoc);

            return new ServiceBaseResult<CreateOperationStatus>(CreateOperationStatus.Ok,
                CreateOperationStatus.Ok.GetDescription());
        }

        public async Task<ServiceBaseResult<UpdateStatus>> UpdatePatient(PatientDto patientDto)
        {
            var patient = await _patientRepository.GetByUserId(patientDto.UserId);
            if (patient == null)
            {
                return new ServiceBaseResult<UpdateStatus>(UpdateStatus.NotFound,
                    UpdateStatus.NotFound.GetDescription());
            }

            //patient = _mapper.Map<Patient>(patientDto);
            patient = _mapper.Map(patientDto, patient);

            await _patientRepository.Update(patient);

            var searchDoc = _mapper.Map<PatientSearchDocument>(patient);
            await _patientSearch.UpdateSearchDoc(searchDoc);

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

            var searchDoc = _mapper.Map<PatientSearchDocument>(patient);
            await _patientSearch.UpdateSearchDoc(searchDoc);

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

            var searchDoc = _mapper.Map<PatientSearchDocument>(patient);
            await _patientSearch.UpdateSearchDoc(searchDoc);

            return new ServiceBaseResult<PatientActivatingStatus>(PatientActivatingStatus.Diactivated,
                PatientActivatingStatus.Diactivated.GetDescription());
        }

        public async Task ReIndex()
        {
            var allPatients = await _patientRepository.GetAll();
            var documentsModel = _mapper.Map<List<PatientSearchDocument>>(allPatients);
            await _patientSearch.ReIndex(documentsModel);
        }
    }
}