using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Models;


namespace Abstractions.Repos
{
    public interface IPatientRepository
    {
        Task Create(Patient patient);
        Task Update(Patient patient);
        Task<Patient> GetByPhone(string phone);
        Task<Patient> GetByUserId(string userId);
        Task<Patient> GetByFilter(Expression<Func<Patient, bool>> filter, params Expression<Func<Patient, object>>[] includeProperties);
        Task<Patient> Deactivate(string userId);
    }
}
