using HealthCare.Domain.Models;
using HealthCare.Repositories;
using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Services;

namespace HealthCare.Domain.Interfaces;

public class MedicalRecordService : IMedicalRecordService 
{
    private IMedicalRecordRepository _medicalRecordRepository;

    public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository) 
    {
        _medicalRecordRepository = medicalRecordRepository;
    }

    public async Task<IEnumerable<MedicalRecordDomainModel>> ReadAll()
    {
        IEnumerable<MedicalRecordDomainModel> medicalRecords = await GetAll();
        List<MedicalRecordDomainModel> result = new List<MedicalRecordDomainModel>();
        foreach (MedicalRecordDomainModel item in medicalRecords)
        {
            if (!item.IsDeleted) result.Add(item);
        }
        return result;
    } 
    public async Task<IEnumerable<MedicalRecordDomainModel>> GetAll()
    {
        IEnumerable<MedicalRecord> data = await _medicalRecordRepository.GetAll();
        if (data == null)
            return new List<MedicalRecordDomainModel>();

        List<MedicalRecordDomainModel> results = new List<MedicalRecordDomainModel>();
        MedicalRecordDomainModel medicalRecordModel;
        foreach (MedicalRecord item in data)
        {
            results.Add(ParseToModel(item));
        }

        return results;
    } 

    public async Task<MedicalRecordDomainModel> GetForPatient(decimal id)
    {
        MedicalRecord data =  await _medicalRecordRepository.GetByPatientId(id);

        if (data == null) 
            throw new DataIsNullException();
        
        return ParseToModel(data);
    }

    public async Task<MedicalRecordDomainModel> Update(CUMedicalRecordDTO dto)
    {
        MedicalRecord medicalRecord = await _medicalRecordRepository.GetByPatientId(dto.PatientId);
        medicalRecord.Weight = dto.Weight;
        medicalRecord.Height = dto.Height;
        medicalRecord.BedriddenDiseases = dto.BedriddenDiseases;
        medicalRecord.IsDeleted = dto.IsDeleted;
        MedicalRecord updatedMedicalRecord = _medicalRecordRepository.Update(medicalRecord);
        _medicalRecordRepository.Save();
        return ParseToModel(updatedMedicalRecord);
    }

    public static MedicalRecordDomainModel ParseToModel(MedicalRecord medicalRecord)
    {
        MedicalRecordDomainModel medicalRecordModel = new MedicalRecordDomainModel 
        {
            Height = medicalRecord.Height,
            Weight = medicalRecord.Weight,
            BedriddenDiseases = medicalRecord.BedriddenDiseases,
            PatientId = medicalRecord.PatientId,
            IsDeleted = medicalRecord.IsDeleted
        };

        medicalRecordModel.AllergiesList = new List<AllergyDomainModel>();
        if (medicalRecord.AllergiesList != null)
            foreach (Allergy item in medicalRecord.AllergiesList)
                medicalRecordModel.AllergiesList.Add(AllergyService.ParseToModel(item));

        medicalRecordModel.ReferralLetters = new List<ReferralLetterDomainModel>();
        if (medicalRecord.ReferralLetters != null)
            foreach (ReferralLetter item in medicalRecord.ReferralLetters)
                medicalRecordModel.ReferralLetters.Add(ReferralLetterService.ParseToModel(item));

        medicalRecordModel.Prescriptions = new List<PrescriptionDomainModel>();
        if (medicalRecord.Prescriptions != null)
            foreach (Prescription item in medicalRecord.Prescriptions)
                medicalRecordModel.Prescriptions.Add(PrescriptionService.ParseToModel(item));
                
        return medicalRecordModel;
    }

    public static MedicalRecord ParseFromModel(MedicalRecordDomainModel medicalRecordModel)
    {
        MedicalRecord medicalRecord = new MedicalRecord 
        {
            Height = medicalRecordModel.Height,
            Weight = medicalRecordModel.Weight,
            BedriddenDiseases = medicalRecordModel.BedriddenDiseases,
            PatientId = medicalRecordModel.PatientId,
            IsDeleted = medicalRecordModel.IsDeleted
        };

        medicalRecord.AllergiesList = new List<Allergy>();
        if (medicalRecordModel.AllergiesList != null)
            foreach (AllergyDomainModel item in medicalRecordModel.AllergiesList)
                medicalRecord.AllergiesList.Add(AllergyService.ParseFromModel(item));

        medicalRecord.ReferralLetters = new List<ReferralLetter>();
        if (medicalRecordModel.ReferralLetters != null)
            foreach(ReferralLetterDomainModel item in medicalRecordModel.ReferralLetters)
                medicalRecord.ReferralLetters.Add(ReferralLetterService.ParseFromModel(item));

        medicalRecord.Prescriptions = new List<Prescription>();
        if (medicalRecordModel.Prescriptions != null)
            foreach(PrescriptionDomainModel item in medicalRecordModel.Prescriptions)
                medicalRecord.Prescriptions.Add(PrescriptionService.ParseFromModel(item));
        
        return medicalRecord;
    }
}
