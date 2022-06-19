using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Interfaces
{
    public interface IFilteringExaminationService
    {
        public Task<IEnumerable<ExaminationDomainModel>> GetAllForPatient(decimal id);
        public Task<IEnumerable<ExaminationDomainModel>> GetAllForPatientSorted(SortExaminationDTO dto);
        public Task<IEnumerable<ExaminationDomainModel>> GetAllForDoctor(decimal id);
        public Task<IEnumerable<ExaminationDomainModel>> SearchByAnamnesis(SearchByNameDTO dto);
    }
}
