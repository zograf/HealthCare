using HealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Interfaces
{
    public interface ISurveyService
    {
        public Task<IEnumerable<AnswerStatsDomainModel>> GetHospitalStats();
        public Task<IEnumerable<AnswerStatsDomainModel>> GetDoctorStats(decimal doctorId);
        public Task<IEnumerable<DoctorStatsDomainModel>> GetBestDoctors();
        public Task<IEnumerable<DoctorStatsDomainModel>> GetWorstDoctors();
    }
}
