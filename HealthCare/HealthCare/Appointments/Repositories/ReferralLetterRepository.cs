using HealthCare.Data.Context;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HealthCare.Repositories
{
    public interface IReferralLetterRepository : IRepository<ReferralLetter>
    {
        public ReferralLetter Post(ReferralLetter referralLetter);
        public ReferralLetter Update(ReferralLetter referralLetter);
        public Task<ReferralLetter> GetById(decimal id);
    }
    public class ReferralLetterRepository : IReferralLetterRepository
    {
        private readonly HealthCareContext _healthCareContext;

        public ReferralLetterRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }

        public async Task<IEnumerable<ReferralLetter>> GetAll()
        {
            return await _healthCareContext.ReferralLetters.Include(x => x.MedicalRecord).Include(x => x.Specialization).ToListAsync();
        }
        
        public async Task<ReferralLetter> GetById(decimal id)
        {
            return await _healthCareContext.ReferralLetters
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public ReferralLetter Post(ReferralLetter referralLetter)
        {
            EntityEntry<ReferralLetter> result = _healthCareContext.ReferralLetters.Add(referralLetter);
            return result.Entity;
        }

        public ReferralLetter Update(ReferralLetter referralLetter)
        {
            EntityEntry<ReferralLetter> updatedEntry = _healthCareContext.ReferralLetters.Attach(referralLetter);
            _healthCareContext.Entry(referralLetter).State = EntityState.Modified;
            return updatedEntry.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }
}
