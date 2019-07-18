using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Repos.Abstractions
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAll();
        Task Create(Patient patient);
        Task Update(Patient patient);
        Task<Patient> GetByPhone(string phone);
        Task<Patient> GetByUserId(string userId);
        Task<List<Patient>> GetByFilter(Expression<Func<Patient, bool>> filter, params Expression<Func<Patient, object>>[] includeProperties);
    }
}