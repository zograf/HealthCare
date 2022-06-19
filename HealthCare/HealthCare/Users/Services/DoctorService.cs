using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class DoctorService : IDoctorService
{
    private IDoctorRepository _doctorRepository;

    private IAnswerService _answerService;

    public DoctorService(IDoctorRepository doctorRepository,
        IAnswerService answerService) 
    {
        _doctorRepository = doctorRepository;
        _answerService = answerService;
    }

    public static Doctor ParseFromModel(DoctorDomainModel doctorModel)
    {
        Doctor doctor = new Doctor 
        {
            IsDeleted = doctorModel.IsDeleted,
            BirthDate = doctorModel.BirthDate,
            Email = doctorModel.Email,
            Id = doctorModel.Id,
            Name = doctorModel.Name,
            Phone = doctorModel.Phone,
            Surname = doctorModel.Surname,
            SpecializationId = doctorModel.SpecializationId
        };
        
        if (doctorModel.Credentials != null)
            doctor.Credentials = CredentialsService.ParseFromModel(doctorModel.Credentials);
        
        if(doctorModel.Specialization != null)
            doctor.Specialization = SpecializationService.ParseFromModel(doctorModel.Specialization);
        
        doctor.Examinations = new List<Examination>();
        doctor.Operations = new List<Operation>();
        
        if (doctorModel.Examinations != null) 
            foreach (ExaminationDomainModel examinationModel in doctorModel.Examinations)
                doctor.Examinations.Add(ExaminationService.ParseFromModel(examinationModel));
        
        if(doctorModel.Operations != null) 
            foreach (OperationDomainModel operationModel in doctorModel.Operations) 
                doctor.Operations.Add(OperationService.ParseFromModel(operationModel));
        
        return doctor;
    }
    public static DoctorDomainModel ParseToModel(Doctor doctor)
    {
        DoctorDomainModel doctorModel = new DoctorDomainModel 
        {
            IsDeleted = doctor.IsDeleted,
            BirthDate = doctor.BirthDate,
            //Credentials = item.Credentials,
            Email = doctor.Email,
            Id = doctor.Id,
            Name = doctor.Name,
            Phone = doctor.Phone,
            Surname = doctor.Surname,
            SpecializationId = doctor.SpecializationId
        };
        if (doctor.Credentials != null)
            doctorModel.Credentials = CredentialsService.ParseToModel(doctor.Credentials);

        if (doctor.Specialization != null)
            doctorModel.Specialization = SpecializationService.ParseToModel(doctor.Specialization); 
            
        doctorModel.Examinations = new List<ExaminationDomainModel>();
        doctorModel.Operations = new List<OperationDomainModel>();
        if (doctor.Examinations != null) 
            foreach (Examination examination in doctor.Examinations) 
                doctorModel.Examinations.Add(ExaminationService.ParseToModel(examination));
                
        if(doctor.Operations != null) 
            foreach (Operation operation in doctor.Operations) 
                doctorModel.Operations.Add(OperationService.ParseToModel(operation));
        return doctorModel;
    }
    
    public async Task<IEnumerable<DoctorDomainModel>> GetAll()
    {
        IEnumerable<Doctor> data = await _doctorRepository.GetAll();
        if (data == null)
            return new List<DoctorDomainModel>();

        List<DoctorDomainModel> results = new List<DoctorDomainModel>();
        foreach (Doctor item in data) 
        {
            results.Add(ParseToModel(item));
        }
        return results;
    }


    public async Task<IEnumerable<DoctorDomainModel>> GetAllBySpecialization(decimal id)
    {
        IEnumerable<Doctor> data = await _doctorRepository.GetBySpecialization(id);
        if (data == null)
            return new List<DoctorDomainModel>();

        List<DoctorDomainModel> results = new List<DoctorDomainModel>();
        foreach (Doctor item in data)
        {
            results.Add(ParseToModel(item));
        }
        return results;
    }

    public async Task<DoctorDomainModel> GetById(decimal id)
    {
        Doctor data = await _doctorRepository.GetById(id);
        if (data == null)
            return null;
        return ParseToModel(data);
    }

    public async Task<IEnumerable<DoctorDomainModel>> ReadAll()
    {
        IEnumerable<DoctorDomainModel> doctors = await GetAll();
        List<DoctorDomainModel> result = new List<DoctorDomainModel>();
        foreach (DoctorDomainModel doctor in doctors)
        {
            if (!doctor.IsDeleted) result.Add(doctor);
        }
        return result;
    }
    
    private DateTime removeSeconds(DateTime dateTime)
    {
        int year = dateTime.Year;
        int month = dateTime.Month;
        int day = dateTime.Day;
        int hour = dateTime.Hour;
        int minute = dateTime.Minute;
        int second = 0;
        return new DateTime(year, month, day, hour, minute, second);
    }

    public async Task<IEnumerable<KeyValuePair<DateTime, DateTime>>> GetAvailableSchedule(decimal doctorId, decimal duration=15)
    {
        Doctor doctor = await _doctorRepository.GetById(doctorId);
        DoctorDomainModel doctorModel = ParseToModel(doctor);
        List<KeyValuePair<DateTime, DateTime>> schedule = new List<KeyValuePair<DateTime, DateTime>>();
        DateTime timeStart, timeEnd;
        // Go through examinations
        foreach (ExaminationDomainModel item in  doctorModel.Examinations)
        {
            if (item.IsDeleted) continue;
            timeStart = removeSeconds(item.StartTime);
            timeEnd = removeSeconds(item.StartTime).AddMinutes(15);
            schedule.Add(new KeyValuePair<DateTime, DateTime>(timeStart, timeEnd));
        }
        // Go through operations
        foreach (OperationDomainModel item in  doctorModel.Operations)
        {
            if (item.IsDeleted) continue;
            timeStart = removeSeconds(item.StartTime);
            timeEnd = removeSeconds(item.StartTime).AddMinutes((double) item.Duration);
            schedule.Add(new KeyValuePair<DateTime, DateTime>(timeStart, timeEnd));
        }
        // Sort the list
        schedule.Sort((x, y) => x.Key.CompareTo(y.Key));
        // Generate available time
        List<KeyValuePair<DateTime, DateTime>> result = new List<KeyValuePair<DateTime, DateTime>>();
        if (result.Count == 0)
        {
            result.Add(new KeyValuePair<DateTime, DateTime>(DateTime.Now, new DateTime(9999, 12, 31)));
            return result;
        }
        KeyValuePair<DateTime, DateTime> first = schedule[0];
        for (int i = 1; i < schedule.Count; i++)
        {
            var currentFirst = first.Value.AddMinutes((double)duration);
            var currentSecond = schedule[i].Key;
            if (currentFirst <= currentSecond)  result.Add(new KeyValuePair<DateTime, DateTime>(first.Value, currentSecond.AddMinutes((double) -duration)));
            first = schedule[i];
        }
        result.Add(new KeyValuePair<DateTime, DateTime>(schedule[schedule.Count - 1].Value, new DateTime(9999, 12, 31)));
        
        return result;
    }

    public async Task<IEnumerable<KeyValuePair<DateTime, DateTime>>> GetBusySchedule(decimal doctorId)
    {
        // TODO: This and the above function can work together
        Doctor doctor = await _doctorRepository.GetById(doctorId);
        DoctorDomainModel doctorModel = ParseToModel(doctor);
        List<KeyValuePair<DateTime, DateTime>> schedule = new List<KeyValuePair<DateTime, DateTime>>();
        DateTime timeStart, timeEnd;
        // Go through examinations
        foreach (ExaminationDomainModel item in  doctorModel.Examinations)
        {
            if (item.IsDeleted) continue;
            timeStart = removeSeconds(item.StartTime);
            timeEnd = removeSeconds(item.StartTime).AddMinutes(15);
            schedule.Add(new KeyValuePair<DateTime, DateTime>(timeStart, timeEnd));
        }
        // Go through operations
        foreach (OperationDomainModel item in  doctorModel.Operations)
        {
            if (item.IsDeleted) continue;
            timeStart = removeSeconds(item.StartTime);
            timeEnd = removeSeconds(item.StartTime).AddMinutes((double) item.Duration);
            schedule.Add(new KeyValuePair<DateTime, DateTime>(timeStart, timeEnd));
        }
        // Sort the list
        schedule.Sort((x, y) => x.Key.CompareTo(y.Key));
        // Generate busy time
        return schedule;
    }

    private bool IsInName(Doctor doctor, string subString)
    {
        if (string.IsNullOrEmpty(subString)) return true;
        return doctor.Name.ToLower().Contains(subString.ToLower());
    }

    private bool IsInSurname(Doctor doctor, string subString)
    {
        if (string.IsNullOrEmpty(subString)) return true;
        return doctor.Surname.ToLower().Contains(subString.ToLower());
    }

    private bool IsInSpecialization(Doctor doctor, string subString)
    {
        if (string.IsNullOrEmpty(subString)) return true;
        return doctor.Specialization.Name.ToLower().Contains(subString.ToLower());
    }
    public async Task<IEnumerable<DoctorDomainModel>> Search(SearchDoctorsDTO dto)
    {
        IEnumerable<Doctor> doctors = await _doctorRepository.GetAll();
        List<DoctorDomainModel> results = new List<DoctorDomainModel>();
        foreach(Doctor doctor in doctors)
        {
            if (IsInName(doctor, dto.Name) && IsInSurname(doctor, dto.Surname) && IsInSpecialization(doctor, dto.Specialization))
                results.Add(ParseToModel(doctor));
        }
        if (dto.SortParam.ToLower().Equals("name"))
            return results.OrderBy(x => x.Name);
        if (dto.SortParam.ToLower().Equals("surname"))
            return results.OrderBy(x => x.Surname);
        if (dto.SortParam.ToLower().Equals("specialization"))
            return results.OrderBy(x => x.Specialization.Name);
        if (dto.SortParam.ToLower().Equals("rating"))
        {
            foreach (DoctorDomainModel doctor in results)
            {
                doctor.Rating = await _answerService.GetAverageRating(doctor.Id);
            }
            return results.OrderByDescending(x => x.Rating);
        }
        return results;
    }
}