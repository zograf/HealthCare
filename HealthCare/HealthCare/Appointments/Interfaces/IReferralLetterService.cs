using HealthCare.Domain.Models;
using HealthCare.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Domain.Services;

namespace HealthCare.Domain.Interfaces
{
    public interface IReferralLetterService : IService<ReferralLetterDomainModel>
    {
        public Task<ReferralLetterDomainModel> Create(ReferralLetterDTO referralDTO);

        public Task<ReferralLetterDomainModel> CreateAppointment(CreateAppointmentDTO dto);
    }
}
