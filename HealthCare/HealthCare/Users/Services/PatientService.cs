using System.Collections;
using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;
using Microsoft.AspNetCore.JsonPatch;

namespace HealthCare.Domain.Services;

public class PatientService : IPatientService
{
    private IPatientRepository _patientRepository;
    private ICredentialsRepository _credentialsRepository;
    private IMedicalRecordRepository _medicalRecordRepository;
    private IUserRoleRepository _userRoleRepository;

    public PatientService(IPatientRepository patientRepository,
                          ICredentialsRepository credentialsRepository,
                          IMedicalRecordRepository medicalRecordRepository,
                          IUserRoleRepository userRoleRepository)
    {
        _patientRepository = patientRepository;
        _credentialsRepository = credentialsRepository;
        _medicalRecordRepository = medicalRecordRepository;
        _userRoleRepository = userRoleRepository;
    }

    public static PatientDomainModel ParseToModel(Patient patient)
    {
        PatientDomainModel patientModel = new PatientDomainModel
        {
            IsDeleted = patient.IsDeleted,
            BirthDate = patient.BirthDate,
            BlockedBy = patient.BlockedBy,
            BlockingCounter = patient.BlockingCounter,
            Email = patient.Email,
            Id = patient.Id,
            Name = patient.Name,
            Surname = patient.Surname,
            Phone = patient.Phone,
            NotificationOffset = patient.NotificationOffset
        };

        if (patient.Credentials != null)
            patientModel.Credentials = CredentialsService.ParseToModel(patient.Credentials);

        if (patient.MedicalRecord != null)
            patientModel.MedicalRecord = MedicalRecordService.ParseToModel(patient.MedicalRecord);

        patientModel.Examinations = new List<ExaminationDomainModel>();
        patientModel.Operations = new List<OperationDomainModel>();
        if (patient.Examinations != null)
            foreach (Examination examination in patient.Examinations)
                patientModel.Examinations.Add(ExaminationService.ParseToModel(examination));

        if (patient.Operations != null)
            foreach (Operation operation in patient.Operations)
                patientModel.Operations.Add(OperationService.ParseToModel(operation));

        return patientModel;
    }

    public static Patient ParseFromModel(PatientDomainModel patientModel)
    {
        Patient patient = new Patient
        {
            IsDeleted = patientModel.IsDeleted,
            BirthDate = patientModel.BirthDate,
            BlockedBy = patientModel.BlockedBy,
            BlockingCounter = patientModel.BlockingCounter,
            Email = patientModel.Email,
            Id = patientModel.Id,
            Name = patientModel.Name,
            Surname = patientModel.Surname,
            Phone = patientModel.Phone,
            NotificationOffset = patientModel.NotificationOffset
        };
        if (patientModel.Credentials != null)
            patient.Credentials = CredentialsService.ParseFromModel(patientModel.Credentials);

        if (patientModel.MedicalRecord != null)
            patient.MedicalRecord = MedicalRecordService.ParseFromModel(patientModel.MedicalRecord);

        patient.Examinations = new List<Examination>();
        patient.Operations = new List<Operation>();
        if (patientModel.Examinations != null)
            foreach (ExaminationDomainModel examinationModel in patientModel.Examinations)
                patient.Examinations.Add(ExaminationService.ParseFromModel(examinationModel));

        if (patientModel.Operations != null)
            foreach (OperationDomainModel operationModel in patientModel.Operations)
                patient.Operations.Add(OperationService.ParseFromModel(operationModel));

        return patient;
    }

    public async Task<bool> IsPatientBlocked(decimal patientId)
    {
        Patient patient = await _patientRepository.GetPatientById(patientId);
        if (patient.BlockedBy != null && !patient.BlockedBy.Equals(""))
            return true;

        return false;
    }


    // Async awaits info from database
    // GetAll is the equivalent of SELECT *
    public async Task<IEnumerable<PatientDomainModel>> GetAll()
    {
        IEnumerable<Patient> data = await _patientRepository.GetAll();
        if (data == null)
            return new List<PatientDomainModel>();

        List<PatientDomainModel> results = new List<PatientDomainModel>();
        foreach (Patient item in data)
        {
            results.Add(ParseToModel(item));
        }

        return results;
    }

    public async Task<IEnumerable<PatientDomainModel>> ReadAll()
    {
        IEnumerable<PatientDomainModel> patients = await GetAll();
        List<PatientDomainModel> result = new List<PatientDomainModel>();
        foreach (PatientDomainModel patientModel in patients)
        {
            if (!patientModel.IsDeleted) result.Add(patientModel);
        }
        return result;
    }

    public async Task<PatientDomainModel> Block(decimal patientId)
    {
        // Secretary block
        Patient patient = await _patientRepository.GetPatientById(patientId);
        if (patient.IsDeleted || !patient.BlockedBy.Equals("")) throw new DataIsNullException();
        patient.BlockedBy = "Secretary";
        patient.BlockingCounter++;
        _ = _patientRepository.Update(patient);
        _patientRepository.Save();
        return ParseToModel(patient);
    }

    public async Task<PatientDomainModel> Unblock(decimal patientId)
    {
        // Secretary Unblock
        Patient patient = await _patientRepository.GetPatientById(patientId);
        if (patient.IsDeleted) throw new DataIsNullException();
        patient.BlockedBy = "";
        _ = _patientRepository.Update(patient);
        _patientRepository.Save();
        return ParseToModel(patient);
    }

    public Patient CreateDefaultPatient(CUPatientDTO dto)
    {
        Patient patient = new Patient
        {
            IsDeleted = false,
            BirthDate = dto.BirthDate,
            Email = dto.Email,
            Name = dto.Name,
            Surname = dto.Surname,
            Phone = dto.Phone,
            BlockedBy = "",
            NotificationOffset = 30
        };
        return patient;
    }

    public MedicalRecord CreateDefaultMedicalRecord(CUPatientDTO dto, decimal patientId)
    {
        MedicalRecord medicalRecord = new MedicalRecord
        {
            Height = dto.MedicalRecordDTO.Height,
            Weight = dto.MedicalRecordDTO.Weight,
            BedriddenDiseases = dto.MedicalRecordDTO.BedriddenDiseases,
            IsDeleted = dto.MedicalRecordDTO.IsDeleted,
            PatientId = patientId
        };
        return medicalRecord;
    }

    public Credentials CreateDefaultCredentials(CUPatientDTO dto, decimal patientId, decimal userRoleId)
    {
        Credentials credentials = new Credentials
        {
            Username = dto.LoginDTO.Username,
            Password = dto.LoginDTO.Password,
            DoctorId = null,
            SecretaryId = null,
            ManagerId = null,
            PatientId = patientId,
            UserRoleId = userRoleId,
            IsDeleted = false
        };
        return credentials;
    }

    public async Task<PatientDomainModel> Create(CUPatientDTO dto)
    {
        Patient newPatient = CreateDefaultPatient(dto);
        Patient insertedPatient = _patientRepository.Post(newPatient);
        _patientRepository.Save();
        MedicalRecord medicalRecord = CreateDefaultMedicalRecord(dto, insertedPatient.Id);
        UserRole userRole = await _userRoleRepository.GetByRoleName("patient");
        Credentials credentials = CreateDefaultCredentials(dto, insertedPatient.Id, userRole.Id);

        _ = _medicalRecordRepository.Post(medicalRecord);
        _ = _credentialsRepository.Post(credentials);
        _medicalRecordRepository.Save();
        _credentialsRepository.Save();

        return ParseToModel(insertedPatient);
    }

    public async Task<Patient> UpdatePatientInfo(CUPatientDTO dto)
    {
        Patient patient = await _patientRepository.GetPatientById(dto.Id);
        patient.Name = dto.Name;
        patient.Surname = dto.Surname;
        patient.Email = dto.Email;
        patient.BirthDate = dto.BirthDate;
        patient.Phone = dto.Phone;
        _ = _patientRepository.Update(patient);
        _patientRepository.Save();
        return patient;
    }

    public async Task<MedicalRecordDomainModel> UpdateMedicalRecordInfo(CUPatientDTO dto, decimal patientId)
    {
        MedicalRecord medicalRecord = await _medicalRecordRepository.GetByPatientId(patientId);
        medicalRecord.Height = dto.MedicalRecordDTO.Height;
        medicalRecord.Weight = dto.MedicalRecordDTO.Weight;
        medicalRecord.BedriddenDiseases = dto.MedicalRecordDTO.BedriddenDiseases;
        _ = _medicalRecordRepository.Update(medicalRecord);
        _medicalRecordRepository.Save();
        return MedicalRecordService.ParseToModel(medicalRecord);
    }

    public async Task<CredentialsDomainModel> UpdateCredentialsInfo(CUPatientDTO dto, decimal patientId)
    {
        Credentials credentials = await _credentialsRepository.GetCredentialsByPatientId(patientId);
        credentials.Username = dto.LoginDTO.Username;
        credentials.Password = dto.LoginDTO.Password;
        _ = _credentialsRepository.Update(credentials);
        _credentialsRepository.Save();
        return CredentialsService.ParseToModel(credentials);
    }

    public async Task<PatientDomainModel> Update(CUPatientDTO dto)
    {
        Patient patient = await UpdatePatientInfo(dto);
        _ = await UpdateMedicalRecordInfo(dto, patient.Id);
        _ = await UpdateCredentialsInfo(dto, patient.Id);
        return ParseToModel(patient);
    }

    public async Task<PatientDomainModel> UpdateNotificationOffset(NotificationOffsetDTO dto)
    {
        Patient patient = await _patientRepository.GetPatientById(dto.PatientId);
        patient.NotificationOffset = dto.NotificationOffset;
        _ = _patientRepository.Update(patient);
        _patientRepository.Save();
        return ParseToModel(patient);
    }

    public async Task<Patient> DeletePatientInfo(decimal patientId)
    {
        Patient patient = await _patientRepository.GetPatientById(patientId);
        patient.IsDeleted = true;
        _ = _patientRepository.Update(patient);
        _patientRepository.Save();
        return patient;
    }
    public async Task<MedicalRecordDomainModel> DeleteMedicalRecordInfo(decimal patientId)
    {
        MedicalRecord medicalRecord = await _medicalRecordRepository.GetByPatientId(patientId);
        medicalRecord.IsDeleted = true;
        _ = _medicalRecordRepository.Update(medicalRecord);
        _medicalRecordRepository.Save();
        return MedicalRecordService.ParseToModel(medicalRecord);
    }

    public async Task<CredentialsDomainModel> DeleteCredentialsInfo(decimal patientId)
    {
        Credentials credentials = await _credentialsRepository.GetCredentialsByPatientId(patientId);
        credentials.IsDeleted = true;
        _ = _credentialsRepository.Update(credentials);
        _credentialsRepository.Save();
        return CredentialsService.ParseToModel(credentials);
    }
    

    public async Task<PatientDomainModel> Delete(decimal patientId)
    {
        Patient patient = await DeletePatientInfo(patientId);
        _ = await DeleteMedicalRecordInfo(patientId);
        _ = await DeleteCredentialsInfo(patientId);
        
        return ParseToModel(patient);
    }

    public async Task<IEnumerable<PatientDomainModel>> GetBlockedPatients()
    {
        IEnumerable<PatientDomainModel> patients = await GetAll();
        List<PatientDomainModel> blockedPatients = new List<PatientDomainModel>();
        foreach (PatientDomainModel patientModel in patients)
            if (patientModel.BlockedBy != null && !patientModel.BlockedBy.Equals(""))
                blockedPatients.Add(patientModel);

        return blockedPatients;
    }


    public async Task<PatientDomainModel> GetWithMedicalRecord(decimal id)
    {
        Patient patient = await _patientRepository.GetPatientById(id);
        if (patient == null)
            throw new DataIsNullException();

        return ParseToModel(patient);
    }

    public async Task<IEnumerable<KeyValuePair<DateTime, DateTime>>> GetSchedule(decimal id)
    {
        return null;
    }
}
