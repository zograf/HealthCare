using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;
using HealthCare.Domain.Services;
using HealthCare.Repositories;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.OpenApi.Any;

namespace HealthCare.Domain.Interfaces;

public class OperationService : IOperationService
{
    private IOperationRepository _operationRepository;

    private IRoomService _roomService;
    private IAvailabilityService _availabilityService;

    public OperationService(IOperationRepository operationRepository,
                            IAvailabilityService availabilityService,
                            IRoomService roomService)
    {
        _operationRepository = operationRepository;
        _availabilityService = availabilityService;
        _roomService = roomService;
    }

    public async Task<IEnumerable<OperationDomainModel>> ReadAll()
    {
        IEnumerable<OperationDomainModel> operations = await GetAll();

        List<OperationDomainModel> result = new List<OperationDomainModel>();
        foreach (OperationDomainModel item in operations)
        {
            if (!item.IsDeleted) result.Add(item);
        }
        return result;
    }
    public async Task<IEnumerable<OperationDomainModel>> GetAll()
    {
        IEnumerable<Operation> data = await _operationRepository.GetAll();
        if (data == null)
            return new List<OperationDomainModel>();

        List<OperationDomainModel> results = new List<OperationDomainModel>();
        foreach (Operation item in data)
        {
            results.Add(ParseToModel(item));
        }

        return results;
    }

    public async Task<IEnumerable<OperationDomainModel>> GetAllForDoctor(decimal id)
    {
        IEnumerable<Operation> data = await _operationRepository.GetAllByDoctorId(id);
        if (data == null)
            throw new DataIsNullException();

        List<OperationDomainModel> results = new List<OperationDomainModel>();
        foreach (Operation item in data)
        {
            results.Add(ParseToModel(item));
        }

        return results;
    }

    public async Task<OperationDomainModel> Create(CUOperationDTO dto)
    {
        await _availabilityService.ValidateUserInput(dto);

        decimal roomId = await _roomService.GetAvailableRoomId(dto.StartTime, "operation", dto.Duration);
        if (roomId == -1)
            throw new NoFreeRoomsException();

        Operation newOperation = ParseFromDTO(dto);
        newOperation.RoomId = roomId;

        _ = _operationRepository.Post(newOperation);
        _operationRepository.Save();

        return ParseToModel(newOperation);
    }

    public async Task<OperationDomainModel> Update(CUOperationDTO dto)
    {
        await _availabilityService.ValidateUserInput(dto);

        Operation operation = await _operationRepository.GetById(dto.Id);

        if (operation == null)
            throw new DataIsNullException();

        decimal roomId = await _roomService.GetAvailableRoomId(dto.StartTime, "operation", dto.Duration);
        if (roomId == -1)
            throw new NoFreeRoomsException();

        operation.PatientId = dto.PatientId;
        operation.DoctorId = dto.DoctorId;
        operation.Duration = dto.Duration;
        operation.StartTime = UtilityService.RemoveSeconds(dto.StartTime);

        _ = _operationRepository.Update(operation);
        _operationRepository.Save();

        return ParseToModel(operation);
    }

    public async Task<OperationDomainModel> Delete(decimal id)
    {
        Operation operation = await _operationRepository.GetById(id);

        // logical delete
        operation.IsDeleted = true;
        _ = _operationRepository.Update(operation);
        _operationRepository.Save();

        return ParseToModel(operation);
    }

    public static OperationDomainModel ParseToModel(Operation operation)
    {
        OperationDomainModel operationModel = new OperationDomainModel
        {
            Id = operation.Id,
            StartTime = operation.StartTime,
            Duration = operation.Duration,
            RoomId = operation.RoomId,
            DoctorId = operation.DoctorId,
            PatientId = operation.PatientId,
            IsDeleted = operation.IsDeleted,
            IsEmergency = operation.IsEmergency
        };

        return operationModel;
    }

    public static Operation ParseFromModel(OperationDomainModel operationModel)
    {
        Operation operation = new Operation
        {
            Id = operationModel.Id,
            StartTime = operationModel.StartTime,
            Duration = operationModel.Duration,
            RoomId = operationModel.RoomId,
            DoctorId = operationModel.DoctorId,
            PatientId = operationModel.PatientId,
            IsDeleted = operationModel.IsDeleted,
            IsEmergency = operationModel.IsEmergency
        };

        return operation;
    }

    public static Operation ParseFromDTO(CUOperationDTO dto)
    {
        return new Operation
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            StartTime = UtilityService.RemoveSeconds(dto.StartTime),
            Duration = dto.Duration,
            IsDeleted = false
        };
    }

}