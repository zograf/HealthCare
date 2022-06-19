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
    public class AvailabilityService : IAvailabilityService
    {
        private IExaminationRepository _examinationRepository;
        private IOperationRepository _operationRepository;

        private IPatientService _patientService;
        private IAppointmentService _appointmentService;

        public AvailabilityService(IExaminationRepository examinationRepository,
                                  IOperationRepository operationRepository,
                                  IPatientService patientService,
                                  IAppointmentService appointmentService)
        {
            _examinationRepository = examinationRepository;
            _operationRepository = operationRepository;
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        private async Task<bool> isPatientOnExamination(CUExaminationDTO dto)
        {
            IEnumerable<Examination> patientsExaminations = await _examinationRepository.GetAllByPatientId(dto.PatientId);
            foreach (Examination examination in patientsExaminations)
            {
                if (examination.Id != dto.ExaminationId)
                {
                    double difference = (dto.StartTime - examination.StartTime).TotalMinutes;
                    if (difference <= 15 && difference >= -15)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private async Task<bool> isPatientOnOperation(CUExaminationDTO dto)
        {
            IEnumerable<Operation> patientsOperations = await _operationRepository.GetAllByPatientId(dto.PatientId);
            foreach (Operation operation in patientsOperations)
            {
                double difference = (dto.StartTime - operation.StartTime).TotalMinutes;
                if (difference <= (double)operation.Duration && difference >= -15)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> isDoctorOnExamination(CUExaminationDTO dto)
        {
            IEnumerable<Examination> doctorsExaminations = await _examinationRepository.GetAllByDoctorId(dto.DoctorId);
            if (doctorsExaminations == null)
            {
                return false;
            }
            foreach (Examination examination in doctorsExaminations)
            {
                if (examination.Id != dto.ExaminationId)
                {
                    double difference = (dto.StartTime - examination.StartTime).TotalMinutes;
                    if (difference <= 15 && difference >= -15)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private async Task<bool> isDoctorOnOperation(CUExaminationDTO dto)
        {
            IEnumerable<Operation> doctorsOperations = await _operationRepository.GetAllByDoctorId(dto.DoctorId);
            foreach (Operation operation in doctorsOperations)
            {
                double difference = (dto.StartTime - operation.StartTime).TotalMinutes;
                if (difference <= (double)operation.Duration && difference >= -15)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> isDoctorAvailable(CUExaminationDTO dto)
        {
            return !(await isDoctorOnExamination(dto) ||
                     await isDoctorOnOperation(dto));
        }

        private async Task<bool> isPatientAvailable(CUExaminationDTO dto)
        {
            return !(await isPatientOnExamination(dto) ||
                     await isPatientOnOperation(dto));
        }
        public async Task ValidateUserInput(CUExaminationDTO dto)
        {
            if (dto.StartTime <= DateTime.Now)
                throw new DateInPastExeption();
            if (await _patientService.IsPatientBlocked(dto.PatientId))
                throw new PatientIsBlockedException();

            bool doctorAvailable = await isDoctorAvailable(dto);
            if (!doctorAvailable)
                throw new DoctorNotAvailableException();

            bool patientAvailable = await isPatientAvailable(dto);
            if (!patientAvailable)
                throw new PatientNotAvailableException();
        }

        private async Task<bool> isPatientOnExamination(CUOperationDTO dto)
        {
            IEnumerable<Examination> patientsExaminations = await _examinationRepository.GetAllByPatientId(dto.PatientId);
            foreach (Examination examination in patientsExaminations)
            {
                double difference = (dto.StartTime - examination.StartTime).TotalMinutes;
                if (difference <= 15 && difference >= -(double)dto.Duration)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> isPatientOnOperation(CUOperationDTO dto)
        {
            IEnumerable<Operation> patientsOperations = await _operationRepository.GetAllByPatientId(dto.PatientId);
            foreach (Operation operation in patientsOperations)
            {
                if (operation.Id != dto.Id)
                {
                    double difference = (dto.StartTime - operation.StartTime).TotalMinutes;
                    if (difference <= (double)operation.Duration && difference >= -(double)dto.Duration)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private async Task<bool> isDoctorOnExamination(CUOperationDTO dto)
        {
            IEnumerable<Examination> doctorsExaminations = await _examinationRepository.GetAllByDoctorId(dto.DoctorId);
            if (doctorsExaminations == null)
                return false;

            foreach (Examination examination in doctorsExaminations)
            {
                double difference = (dto.StartTime - examination.StartTime).TotalMinutes;
                if (difference <= 15 && difference >= -(double)dto.Duration)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> isDoctorOnOperation(CUOperationDTO dto)
        {
            IEnumerable<Operation> doctorsOperations = await _operationRepository.GetAllByDoctorId(dto.DoctorId);
            foreach (Operation operation in doctorsOperations)
            {
                if (operation.Id != dto.Id)
                {
                    double difference = (dto.StartTime - operation.StartTime).TotalMinutes;
                    if (difference <= (double)operation.Duration && difference >= -(double)dto.Duration)
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        private async Task<bool> isDoctorAvailable(CUOperationDTO dto)
        {
            return !(await isDoctorOnOperation(dto) ||
                     await isDoctorOnExamination(dto));
        }

        private async Task<bool> isPatientAvailable(CUOperationDTO dto)
        {
            return !(await isPatientOnOperation(dto) ||
                     await isPatientOnExamination(dto));
        }

        public async Task ValidateUserInput(CUOperationDTO dto)
        {
            if (dto.StartTime <= DateTime.Now)
                throw new DateInPastExeption();

            if (await _patientService.IsPatientBlocked(dto.PatientId))
                throw new PatientIsBlockedException();

            bool doctorAvailable = await isDoctorAvailable(dto);
            if (!doctorAvailable)
                throw new DoctorNotAvailableException();

            bool patientAvailable = await isPatientAvailable(dto);
            if (!patientAvailable)
                throw new PatientNotAvailableException();
        }

        public async Task<bool> IsDoctorFreeOnDay(decimal doctorId, DateTime singleDate)
        {
            IEnumerable<AppointmentDomainModel> appointments = await _appointmentService.GetAllForDoctor(new DoctorsScheduleDTO
            {
                DoctorId = doctorId,
                Date = singleDate,
                ThreeDays = false
            });

            return appointments.Count() == 0 ? true : false;
        }

        public async Task IsDoctorFreeOnDateRange(DateTime from, DateTime to, decimal doctorId)
        {
            DateTime singleDate = from;
            while (singleDate <= to)
            {
                if (!(await IsDoctorFreeOnDay(doctorId, singleDate)))
                    throw new DoctorIsNotFreeOnDayException();
                singleDate = singleDate.AddDays(1);
            }
        }
    }
}
