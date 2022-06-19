using HealthCare.Data.Context;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repositories
{
    public interface IQuestionRepository : IRepository<Question>
    {

    }
    public class QuestionRepository : IQuestionRepository
    {
        private readonly HealthCareContext _healthCareContext;
        public QuestionRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Question>> GetAll()
        {
            return await _healthCareContext.Questions.ToListAsync();
        }
        public async Task<IEnumerable<Question>> GetDoctorQuestions()
        {
            return await _healthCareContext.Questions.Where(q => q.IsForDoctor).ToListAsync();
        }
        public async Task<IEnumerable<Question>> GetHospitalQuestions()
        {
            return await _healthCareContext.Questions.Where(q => !q.IsForDoctor).ToListAsync();
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }

    }
}
