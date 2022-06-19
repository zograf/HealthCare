using HealthCare.Data.Entities;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Services
{
    public class SurveyService : ISurveyService
    {
        private IAnswerRepository _answerRepository;
        public SurveyService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<IEnumerable<DoctorStatsDomainModel>> GetBestDoctors()
        {
            IEnumerable<DoctorStatsDomainModel> result = await GetDoctorsAverage();
            return result.Take(3);
        }
        public async Task<IEnumerable<DoctorStatsDomainModel>> GetWorstDoctors()
        {
            IEnumerable<DoctorStatsDomainModel> result = await GetDoctorsAverage();
            return result.TakeLast(3);
        }

        public async Task<IEnumerable<DoctorStatsDomainModel>> GetDoctorsAverage()
        {
            IEnumerable<Answer> answers = await _answerRepository.GetAllDoctor();
            IEnumerable<DoctorStatsDomainModel> result = answers
                .GroupBy(a => new { a.Doctor })
                .Select(x => new DoctorStatsDomainModel
                {
                    Doctor = x.Key.Doctor,
                    Average = x.Average(a => a.Evaluation)
                }).OrderBy(ds => ds.Average);
            return result;
        }

        public async Task<IEnumerable<AnswerStatsDomainModel>> GetDoctorStats(decimal doctorId)
        {
            IEnumerable<Answer> answers = await _answerRepository.GetForDoctor(doctorId);
            return await GetStats(answers);
        }

        public async Task<IEnumerable<AnswerStatsDomainModel>> GetHospitalStats()
        {
            IEnumerable<Answer> answers = await _answerRepository.GetForHospital();
            return await GetStats(answers);
        }

        public async Task<IEnumerable<AnswerStatsDomainModel>> GetStats(IEnumerable<Answer> answers)
        {
            IEnumerable<AnswerStatsDomainModel> stats = answers.GroupBy(a => new { a.Question })
                .Select(x => new AnswerStatsDomainModel
            {
                Count = x.Count(),
                Average = x.Average(a => a.Evaluation),
                question = x.Key.Question,
                Comments = x.Select(a => a.AnswerText).ToList(),
            });

            return stats;
        }
    }
}
