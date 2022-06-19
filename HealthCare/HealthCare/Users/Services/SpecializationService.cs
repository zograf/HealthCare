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
    public class SpecializationService : ISpecializationService
    {
        private ISpecializationRepository _specializationRepository;

        public SpecializationService(ISpecializationRepository specializationRepository)
        {
            _specializationRepository = specializationRepository;
        }

        public static SpecializationDomainModel ParseToModel(Specialization specialization)
        {
            SpecializationDomainModel specializationModel = new SpecializationDomainModel
            {
                Id = specialization.Id,
                Name = specialization.Name
            };
            return specializationModel;
        }

        public static Specialization ParseFromModel(SpecializationDomainModel specializationModel)
        {
            Specialization specialization = new Specialization
            {
                Id = specializationModel.Id,
                Name = specializationModel.Name
            };
            return specialization;
        }

        public async Task<IEnumerable<SpecializationDomainModel>> GetAll()
        {
            IEnumerable<Specialization> data = await _specializationRepository.GetAll();
            if (data == null)
                return new List<SpecializationDomainModel>();

            List<SpecializationDomainModel> results = new List<SpecializationDomainModel>();
            foreach (Specialization item in data)
            {
                results.Add(ParseToModel(item));
            }

            return results;
        }
    }
}
