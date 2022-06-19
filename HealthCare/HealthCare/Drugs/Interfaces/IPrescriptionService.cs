using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Interfaces
{
    public interface IPrescriptionService : IService<PrescriptionDomainModel>
    {
        public Task<PrescriptionDomainModel> Create(PrescriptionDTO prescriptionDTO);
        public Task<List<string>> GetAllReminders();


    }
}
