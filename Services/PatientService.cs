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
            //var search = new SearchDescriptor<PatientDto>()
            //    .Query(qu => qu
            //        .QueryString(queryString => queryString
            //            .Query(query)));

            //var result = _elasticClient.Search<Patient>(search);

            //;


            //var search2 = new SearchDescriptor<PatientDto>()
            //    .Query(qu => qu
            //        .QueryString(queryString => queryString
            //            .Query(query)))
            //    .Aggregations(ag => ag
            //        .Terms("firstName", term => term
            //            .Field(field => field.FirstName)));

            //var result2 = _elasticClient.Search<PatientDto>(search2);

            //;
            //var searchResult3 = await _elasticClient.SearchAsync<PatientDto>(s => s
            //    .From((page - 1) * pageSize)
            //    .Size(pageSize)
            //    .Query(qry => qry
            //        .MultiMatch(b =>
            //            b.MinimumShouldMatch(query))));
            //;

            //var searchResult4 = await _elasticClient.SearchAsync<PatientDto>(s => s
            //    .From((page - 1) * pageSize)
            //    .Size(pageSize)
            //    .Query(qry => qry
            //        .MultiMatch(b =>b.Query(query))));

            //var searchResult = await _elasticClient.SearchAsync<PatientDto>(s => s
            //    .From((page - 1) * pageSize)
            //    .Size(pageSize)
            //    .Query(qry => qry
            //        .Fuzzy(c => c
            //            .Name("named_query")
            //            .Boost(1.1)
            //            .Field(p => p.FirstName)
            //            .Field(p => p.LastName)
            //            .Field(p => p.Phone)
            //            .Fuzziness(Fuzziness.Auto)
            //            .Value(query)
            //            .MaxExpansions(100)
            //            .PrefixLength(3)
            //            .Rewrite(MultiTermQueryRewrite.ConstantScore)
            //            .Transpositions()
            //        )));
            //        //.MultiMatch(m => m.Query(query).Fuzziness(Fuzziness.AutoLength(3,250)).Operator(Operator.And))));





            //var searchResult = await _elasticClient.SearchAsync<PatientDto>(s => s
            //    .From((page - 1) * pageSize)
            //    .Size(pageSize)
            //    .Query(qry => qry
            //        .Match(m => m.Query(query).Fuzziness(Fuzziness.Auto).Operator(Operator.And))));


            //.Query(q => q
            //    .Wildcard(f => f.LastName, "to*", Boost: 1.0)

            var searchResult = await _elasticClient.SearchAsync<PatientDto>(s => s
                .From((page - 1) * pageSize)
                .Size(pageSize)
                .Query(qry =>
                    qry.Wildcard(w =>
                        w.Field(f => f.FirstName).Boost(1.0).Value(query + "*")
                            .Rewrite(MultiTermQueryRewrite.TopTermsBoost(10)))
                    //|| qry.Wildcard(w =>
                    //    w.Field(f => f.FirstName).Boost(1.0).Value(query + "*")
                    //        .Rewrite(MultiTermQueryRewrite.TopTermsBoost(10)))
                            ));

            var t3 = await _elasticClient.SearchAsync<PatientDto>(s => s
                .Query(q => q.Prefix(m => m.Field(f => f.FirstName).Value("one")
                ))
            );


            var searchResult2 = await _elasticClient.SearchAsync<PatientDto>(s => s
                .From((page - 1) * pageSize)
                .Size(pageSize)
                .Query(qry =>
                    qry.Bool(b =>
                        b.Should(m =>
                            m.Term(f => f.Field(n => n.FirstName).Value(query))))));

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
            try
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
            var result = await _elasticClient.IndexDocumentAsync(patient);
            return new ServiceBaseResult<CreateOperationStatus>(CreateOperationStatus.Ok,
                CreateOperationStatus.Ok.GetDescription());

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ServiceBaseResult<UpdateStatus>> UpdatePatient(PatientDto patientDto)
        {
            //TODO Validate patientDto
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
            await _elasticClient.DeleteByQueryAsync<Patient>(q => q.MatchAll());

            var allPatients = await _patientRepository.GetAll();

            foreach (var patient in allPatients)
            {
                await _elasticClient.IndexDocumentAsync(patient);
            }

        }
    }
}
