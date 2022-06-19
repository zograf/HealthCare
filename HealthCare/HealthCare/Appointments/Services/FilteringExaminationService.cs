using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
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
    public class FilteringExaminationService : IFilteringExaminationService
    {
        private IExaminationRepository _examinationRepository;

        private IDoctorService _doctorService;

        public FilteringExaminationService(IExaminationRepository examinationRepository,
            IDoctorService doctorService)
        {
            _examinationRepository = examinationRepository;
            _doctorService = doctorService;
        }

        public async Task<IEnumerable<ExaminationDomainModel>> GetAllForPatient(decimal id)
        {
            IEnumerable<Examination> data = await _examinationRepository.GetAllByPatientId(id);
            if (data == null)
                throw new DataIsNullException();

            List<ExaminationDomainModel> results = new List<ExaminationDomainModel>();
            foreach (Examination item in data)
            {
                results.Add(ExaminationService.ParseToModel(item));
            }

            return results;
        }

        public async Task<IEnumerable<ExaminationDomainModel>> GetAllForPatientSorted(SortExaminationDTO dto)
        {
            List<ExaminationDomainModel> examinations;
            try
            {
                examinations = (List<ExaminationDomainModel>)await GetAllForPatient(dto.PatientId);
            }
            catch (Exception)
            {
                throw new DataIsNullException();
            }

            if (dto.SortParam.Equals("date"))
                return examinations.OrderBy(x => x.StartTime);

            if (dto.SortParam.Equals("doctor"))
                return examinations.OrderBy(x => x.DoctorId);

            Dictionary<decimal, decimal> doctorsSpecialisations = await MapSpecializations(examinations);
            return examinations.OrderBy(x => doctorsSpecialisations[x.DoctorId]);
        }

        public async Task<Dictionary<decimal, decimal>> MapSpecializations(List<ExaminationDomainModel> examinations)
        {
            Dictionary<decimal, decimal> result = new Dictionary<decimal, decimal>();
            foreach (var examination in examinations)
            {
                if (result.ContainsKey(examination.DoctorId)) continue;
                DoctorDomainModel doctor = await _doctorService.GetById(examination.DoctorId);
                result.Add(examination.DoctorId, doctor.SpecializationId);
            }

            return result;
        }

        public async Task<IEnumerable<ExaminationDomainModel>> GetAllForDoctor(decimal id)
        {
            IEnumerable<Examination> data = await _examinationRepository.GetAllByDoctorId(id);
            if (data == null)
                throw new DataIsNullException();

            List<ExaminationDomainModel> results = new List<ExaminationDomainModel>();
            foreach (Examination item in data)
            {
                results.Add(ExaminationService.ParseToModel(item));
            }

            return results;
        }


        public bool IsInAnamnesis(Anamnesis anamnesis, string subString)
        {
            return anamnesis != null && anamnesis.Description.ToLower().Contains(subString);
        }
        public async Task<IEnumerable<ExaminationDomainModel>> SearchByAnamnesis(SearchByNameDTO dto)
        {
            dto.Substring = dto.Substring.ToLower();
            IEnumerable<Examination> examinations = await _examinationRepository.GetByPatientId(dto.PatientId);
            if (examinations == null)
                throw new DataIsNullException();

            List<ExaminationDomainModel> results = new List<ExaminationDomainModel>();

            foreach (Examination item in examinations)
                if (IsInAnamnesis(item.Anamnesis, dto.Substring))
                    results.Add(ExaminationService.ParseToModel(item));

            return results;

        }
    }
}
