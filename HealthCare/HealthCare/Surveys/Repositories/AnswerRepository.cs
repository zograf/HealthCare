using HealthCare.Data.Context;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repositories
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        public Task<IEnumerable<Answer>> GetForHospital();
        public Task<IEnumerable<Answer>> GetForDoctor(decimal id);
        public Task<IEnumerable<Answer>> GetAllDoctor();
        public Task<IEnumerable<Answer>> GetForQuestion(decimal questionId);
        public Answer Post(Answer answer);
        
    }
    public class AnswerRepository : IAnswerRepository
    {
        private readonly HealthCareContext _healthCareContext;

        public AnswerRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Answer>> GetAll()
        {
            return await _healthCareContext.Answers
                .Include(x => x.Doctor)
                .Include(x => x.Question)
                .ToListAsync();
        }

        public async Task<IEnumerable<Answer>> GetForQuestion(decimal questionId)
        {
            return await _healthCareContext.Answers
                .Where(a => a.QuestionId == questionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Answer>> GetForDoctor(decimal id)
        {
            return await _healthCareContext.Answers
                .Include(x => x.Question)
                .Where(a => a.DoctorId == id)
                .ToListAsync();
        }
        public async Task<IEnumerable<Answer>> GetForHospital()
        {
            return await _healthCareContext.Answers
                .Include(x => x.Question)
                .Where(a => a.DoctorId == null)
                .ToListAsync();
        }

        public Answer Post(Answer answer)
        {
            EntityEntry<Answer> result = _healthCareContext.Answers.Add(answer);
            return result.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }

        public async Task<IEnumerable<Answer>> GetAllDoctor()
        {
            return await _healthCareContext.Answers
                .Include(x => x.Doctor)
                .Include(x => x.Question)
                .Where(x => x.Doctor != null)
                .ToListAsync();
        }
    }
}
