using HealthCare.Data.Entities;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Domain.DTOs;
using HealthCare.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace HealthCare.Domain.Services
{
    public class ReferralLetterService : IReferralLetterService
    {
        private IReferralLetterRepository _referralLetterRepository;
        private IDoctorRepository _doctorRepository;
        private ISpecializationRepository _specializationRepository;
        private IPatientRepository _patientRepository;

        private IExaminationService _examinationService;


        public ReferralLetterService(IReferralLetterRepository referralLetterRepository, 
                                    IDoctorRepository doctorRepository,
                                    ISpecializationRepository specializationRepository, 
                                    IPatientRepository patientRepository,
                                    IExaminationService examinationService)
        {
            _referralLetterRepository = referralLetterRepository;
            _doctorRepository = doctorRepository;
            _specializationRepository = specializationRepository;
            _patientRepository = patientRepository;
            _examinationService = examinationService;
        }

        public static ReferralLetterDomainModel ParseToModel(ReferralLetter referralLetter)
        {
            ReferralLetterDomainModel referralLetterModel = new ReferralLetterDomainModel
            {
                Id = referralLetter.Id,
                FromDoctorId = referralLetter.FromDoctorId,
                ToDoctorId = referralLetter.ToDoctorId,
                PatientId = referralLetter.PatientId,
                SpecializationId = referralLetter.SpecializationId,
                State = referralLetter.State
            };
            if (referralLetter.Specialization != null)
                referralLetterModel.Specialization = SpecializationService.ParseToModel(referralLetter.Specialization); 
                    
            return referralLetterModel;
        }

        public static ReferralLetter ParseFromModel(ReferralLetterDomainModel referralLetterModel)
        {
            ReferralLetter referralLetter = new ReferralLetter
            {
                Id = referralLetterModel.Id,
                FromDoctorId = referralLetterModel.FromDoctorId,
                ToDoctorId = referralLetterModel.ToDoctorId,
                PatientId = referralLetterModel.PatientId,
                SpecializationId = referralLetterModel.SpecializationId,
                State = referralLetterModel.State
            };
            if (referralLetterModel.Specialization != null)
                referralLetter.Specialization = SpecializationService.ParseFromModel(referralLetterModel.Specialization); 
                    
            return referralLetter;
        }

        public async Task<IEnumerable<ReferralLetterDomainModel>> GetAll()
        {
            IEnumerable<ReferralLetter> data = await _referralLetterRepository.GetAll();
            if (data == null)
                return new List<ReferralLetterDomainModel>();

            List<ReferralLetterDomainModel> results = new List<ReferralLetterDomainModel>();
            foreach (ReferralLetter item in data)
            {
                results.Add(ParseToModel(item));
            }

            return results;
        }

        public async Task<Boolean> TryCreateExamination(CUExaminationDTO dto)
        {
            try
            {
                await _examinationService.Create(dto);
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }

        public CUExaminationDTO MakeMockup(decimal patientId, DateTime startTime)
        {
            CUExaminationDTO examinationModel = new CUExaminationDTO
            {
                PatientId = patientId,
                StartTime = startTime,
                IsPatient = false
            };
            return examinationModel;
        }

        public async Task<ReferralLetterDomainModel?> DoctorSpecifiedAppointment(CUExaminationDTO examinationModel, 
            ReferralLetterDomainModel referralLetterModel, ReferralLetter referralLetter)
        {
            examinationModel.DoctorId = referralLetterModel.ToDoctorId.GetValueOrDefault();
            Boolean flag = await TryCreateExamination(examinationModel);
            if (flag == false) return null;
            UpdateReferralLetter(referralLetter);
            return referralLetterModel;
        }

        public void UpdateReferralLetter(ReferralLetter referralLetter)
        {
            referralLetter.State = "accepted";
            _ = _referralLetterRepository.Update(referralLetter);
            _referralLetterRepository.Save();
        }
        public async Task<ReferralLetterDomainModel?> DoctorNotSpecifiedAppointment(CUExaminationDTO examinationModel, 
            ReferralLetterDomainModel referralLetterModel, ReferralLetter referralLetter)
        {
            IEnumerable<Doctor> allDoctors = await _doctorRepository.GetAll();
            Boolean created = false;
            foreach (Doctor doctor in allDoctors)
            {
                if (doctor.Id == referralLetterModel.FromDoctorId) continue;

                if (doctor.SpecializationId == referralLetterModel.SpecializationId)
                {
                    examinationModel.DoctorId = doctor.Id;
                    created = await TryCreateExamination(examinationModel);
                    if (created)
                    {
                        referralLetter.ToDoctorId = doctor.Id;
                        UpdateReferralLetter(referralLetter);
                        break;
                    }
                }
            }

            if (!created) return null;
            return referralLetterModel;
        }

        public async Task<ReferralLetterDomainModel> CreateAppointment(CreateAppointmentDTO dto)
        {
            ReferralLetter referralLetter = await _referralLetterRepository.GetById(dto.ReferralId);
            ReferralLetterDomainModel referralLetterModel = ParseToModel(referralLetter);
            if (!referralLetterModel.State.Equals("created")) throw new ReferralCannotBeUsedException();
            if (dto.StartTime < DateTime.Now) throw new DateInPastExeption();
            
            ReferralLetterDomainModel? returnModel;
            CUExaminationDTO examinationModel = MakeMockup(referralLetterModel.PatientId, dto.StartTime);
            if (referralLetterModel.ToDoctorId != null)
            {
                returnModel = await DoctorSpecifiedAppointment(examinationModel, referralLetterModel, referralLetter);
                if (returnModel == null) throw new NoFreeRoomsException();
            }
            else
            {
                returnModel = await DoctorNotSpecifiedAppointment(examinationModel, referralLetterModel, referralLetter);
                if (returnModel == null) throw new NoAvailableSpecialistsException();
            }

            return returnModel;
        }

        private async Task<bool> isPatientBlocked(decimal patientId)
        {
            Patient patient = await _patientRepository.GetPatientById(patientId);
            if (patient.BlockedBy != null && !patient.BlockedBy.Equals(""))
                return true;

            return false;
        }

        public async Task<ReferralLetterDomainModel> Create(ReferralLetterDTO referralDTO)
        {
    
            if (referralDTO.FromDoctorId == referralDTO.ToDoctorId)
                throw new ReferredYourselfException();

            if (await isPatientBlocked(referralDTO.PatientId))
                throw new PatientIsBlockedException();

            ReferralLetter newReferral = new ReferralLetter 
            {
                FromDoctorId = referralDTO.FromDoctorId,
                PatientId = referralDTO.PatientId,
                ToDoctorId = referralDTO.ToDoctorId,
                SpecializationId = referralDTO.SpecializationId,
                State = "created"
            };

            if (referralDTO.SpecializationId != null)
            {
                newReferral.Specialization = await _specializationRepository.GetById(referralDTO.SpecializationId.Value);
            }

            _referralLetterRepository.Post(newReferral);
            _referralLetterRepository.Save();

            return ParseToModel(newReferral);
        }
    }
}
