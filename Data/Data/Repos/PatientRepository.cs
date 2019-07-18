using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Data.Repos.Abstractions;
using Data.Sources;
using Microsoft.EntityFrameworkCore;

namespace Data.Repos
{
    public class PatientRepository : IPatientRepository
    {
        private readonly Context _context;
        public PatientRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Patient>> GetAll()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task Create(Patient patient)
        {
            patient.Activated = true;
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<Patient> GetByPhone(string phone)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Phone == phone);
        }

        public async Task<Patient> GetByUserId(string userId)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<List<Patient>> GetByFilter(Expression<Func<Patient, bool>> filter, params Expression<Func<Patient, object>>[] includeProperties)
        {
            IQueryable<Patient> query = _context.Patients;

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }
    }
}
