using System.Linq.Expressions;
using HealthCare.Data.Entities;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
using HealthCare.Domain.DTOs;

namespace HealthCare.Domain.Services;

public class ExaminationService : IExaminationService
{
    private IExaminationRepository _examinationRepository;
    private IExaminationApprovalRepository _examinationApprovalRepository;
    private IAnamnesisRepository _anamnesisRepository;

    private IRoomService _roomService;
    private IAvailabilityService _availabilityService;
    private IAntiTrollService _antiTrollService;

    public ExaminationService(IExaminationRepository examinationRepository,
                              IExaminationApprovalRepository examinationApprovalRepository,
                              IAnamnesisRepository anamnesisRepository,
                              IRoomService roomService,
                              IAvailabilityService availabilityService,
                              IAntiTrollService antiTrollService)
    {
        _examinationRepository = examinationRepository;
        _examinationApprovalRepository = examinationApprovalRepository;
        _anamnesisRepository = anamnesisRepository;
        _roomService = roomService;
        _availabilityService = availabilityService;
        _antiTrollService = antiTrollService;
    }

    public static ExaminationDomainModel ParseToModel(Examination examination)
    {
        ExaminationDomainModel examinationModel = new ExaminationDomainModel
        {
            Id = examination.Id,
            StartTime = examination.StartTime,
            DoctorId = examination.DoctorId,
            IsDeleted = examination.IsDeleted,
            PatientId = examination.PatientId,
            RoomId = examination.RoomId,
            IsEmergency = examination.IsEmergency,
            Duration = 15
        };

        if (examination.Anamnesis != null)
            examinationModel.Anamnesis = AnamnesisService.ParseToModel(examination.Anamnesis);

        return examinationModel;
    }

    public static Examination ParseFromModel(ExaminationDomainModel examinationModel)
    {
        Examination examination = new Examination
        {
            Id = examinationModel.Id,
            StartTime = examinationModel.StartTime,
            DoctorId = examinationModel.DoctorId,
            IsDeleted = examinationModel.IsDeleted,
            PatientId = examinationModel.PatientId,
            RoomId = examinationModel.RoomId,
            IsEmergency = examinationModel.IsEmergency
        };

        if (examinationModel.Anamnesis != null)
            examination.Anamnesis = AnamnesisService.ParseFromModel(examinationModel.Anamnesis);

        return examination;
    }
    public async Task<IEnumerable<ExaminationDomainModel>> GetAll()
    {
        IEnumerable<Examination> data = await _examinationRepository.GetAll();
        if (data == null)
            return new List<ExaminationDomainModel>();

        List<ExaminationDomainModel> results = new List<ExaminationDomainModel>();
        foreach (Examination item in data)
        {
            results.Add(ParseToModel(item));
        }

        return results;
    }

    public async Task<IEnumerable<ExaminationDomainModel>> ReadAll()
    {
        IEnumerable<ExaminationDomainModel> examinations = await GetAll();
        List<ExaminationDomainModel> result = new List<ExaminationDomainModel>();
        foreach (ExaminationDomainModel item in examinations)
        {
            if (!item.IsDeleted) result.Add(item);
        }
        return result;
    }

    public async Task<ExaminationDomainModel> Delete(DeleteExaminationDTO dto)
    {
        if (dto.IsPatient && await _antiTrollService.AntiTrollCheck(dto.PatientId, false))
            throw new DataIsNullException();

        Examination examination = await _examinationRepository.GetExamination(dto.ExaminationId);
        double daysUntilExamination = (examination.StartTime - DateTime.Now).TotalDays;

        if (daysUntilExamination > 1 || !dto.IsPatient)
            DeleteExamination(examination);
        else
            CreateExaminationApproval(examination.Id, examination.Id);

        if (dto.IsPatient)
            _antiTrollService.WriteToAntiTroll(examination.PatientId, "deleted");

        return ParseToModel(examination);
    }

    public void DeleteExamination(Examination examination)
    {
        examination.IsDeleted = true;
        _ = _examinationRepository.Update(examination);
        _examinationRepository.Save();

        // anamnesis can't exist without its examination
        // check if anamnesis exists
        if (examination.Anamnesis == null) return;

        examination.Anamnesis.IsDeleted = true;
        _ = _anamnesisRepository.Update(examination.Anamnesis);
        _anamnesisRepository.Save();
    }

    public async Task<ExaminationDomainModel> Create(CUExaminationDTO dto)
    {
        await _availabilityService.ValidateUserInput(dto);

        if (dto.IsPatient && await _antiTrollService.AntiTrollCheck(dto.PatientId, true))
            throw new AntiTrollException();

        decimal roomId = await _roomService.GetAvailableRoomId(dto.StartTime, "examination");
        if (roomId == -1)
            throw new NoFreeRoomsException();

        if (dto.IsPatient)
            _antiTrollService.WriteToAntiTroll(dto.PatientId, "create");

        Examination newExamination = new Examination
        {
            PatientId = dto.PatientId,
            RoomId = roomId,
            DoctorId = dto.DoctorId,
            StartTime = UtilityService.RemoveSeconds(dto.StartTime),
            IsDeleted = false,
            Anamnesis = null
        };
        _ = _examinationRepository.Post(newExamination);
        _examinationRepository.Save();

        return ParseToModel(newExamination);
    }

    public async Task<ExaminationDomainModel> Update(CUExaminationDTO dto)
    {
        await _availabilityService.ValidateUserInput(dto);

        // One patient can't change other patient's appointment
        // so the patient will always match examinationModel.PatientId
        if (dto.IsPatient && await _antiTrollService.AntiTrollCheck(dto.PatientId, false))
            throw new AntiTrollException();

        Examination examination = await _examinationRepository.GetExaminationWithoutAnamnesis(dto.ExaminationId);
        double daysUntilExamination = (examination.StartTime - DateTime.Now).TotalDays;

        decimal roomId = await _roomService.GetAvailableRoomId(dto.StartTime, "examination");
        if (roomId == -1)
            throw new NoFreeRoomsException();

        if (daysUntilExamination > 1 || !dto.IsPatient)
            UpdateExamination(dto, roomId, examination);
        else
        {
            Examination newExamination = CreateExamination(dto, roomId);
            Examination createdExamination = await _examinationRepository.GetByParams(newExamination.DoctorId, newExamination.RoomId, newExamination.PatientId, UtilityService.RemoveSeconds(newExamination.StartTime));
            // Make an approval request
            CreateExaminationApproval(examination.Id, createdExamination.Id);
        }


        if (dto.IsPatient)
            _antiTrollService.WriteToAntiTroll(dto.PatientId, "update");

        return ParseToModel(examination);
    }

    public void UpdateExamination(CUExaminationDTO dto, decimal roomId, Examination examination)
    {
        examination.RoomId = roomId;
        examination.DoctorId = dto.DoctorId;
        examination.PatientId = dto.PatientId;
        examination.StartTime = UtilityService.RemoveSeconds(dto.StartTime);
        //update
        _ = _examinationRepository.Update(examination);
        _examinationRepository.Save();
    }

    public Examination CreateExamination(CUExaminationDTO dto, decimal roomId)
    {
        Examination newExamination = new Examination
        {
            PatientId = dto.PatientId,
            RoomId = roomId,
            DoctorId = dto.DoctorId,
            StartTime = dto.StartTime,
            IsDeleted = true,
            Anamnesis = null
        };

        _ = _examinationRepository.Post(newExamination);
        _examinationRepository.Save();
        return newExamination;
    }

    public void CreateExaminationApproval(decimal oldId, decimal newId)
    {
        ExaminationApproval examinationApproval = new ExaminationApproval
        {
            State = "created",
            IsDeleted = false,
            NewExaminationId = newId,
            OldExaminationId = oldId
        };
        _ = _examinationApprovalRepository.Post(examinationApproval);
        _examinationApprovalRepository.Save();
    }
}
