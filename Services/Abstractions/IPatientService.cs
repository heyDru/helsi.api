using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Enums;
using Common.Models.ServiceReponses;
using Services.DtoModels;

namespace Services.Abstractions
{
    public interface IPatientService
    {
        Task<ServiceBaseResult<SearchOperationStatus, List<PatientDto>>> GetAll();

        Task<ServiceBaseResult<CreateOperationStatus>> CreatePatient(PatientDto patient);

        Task<ServiceBaseResult<UpdateStatus>> UpdatePatient(PatientDto patient);

        Task<ServiceBaseResult<PatientActivatingStatus>> DeactivatePatient(string userId);

        //This method added just in case if we need some special logic or buissnes procces for activating 
        Task<ServiceBaseResult<PatientActivatingStatus>> ActivatePatient(string userId);

        Task<ServiceBaseResult<SearchOperationStatus, IReadOnlyCollection<PatientDto>>> Search(string query, int page, int pageSize);

        Task ReIndex();
    }
}
