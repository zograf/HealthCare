using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Interfaces
{
    public interface IAvailabilityService
    {
        public Task ValidateUserInput(CUExaminationDTO dto);
        public Task ValidateUserInput(CUOperationDTO dto);
        public Task<bool> IsDoctorFreeOnDay(decimal doctorId, DateTime singleDate);
        public Task IsDoctorFreeOnDateRange(DateTime from, DateTime to, decimal doctorId);
    }
}
