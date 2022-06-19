using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;
using HealthCareAPI.Renovations.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Services
{
    public class SplitRenovationService : ISplitRenovationService
    {
        private ISplitRenovationRepository _splitRenovationRepository;
        private IRoomRepository _roomRepository;
        private IRoomTypeRepository _roomTypeRepository;
        private IRenovationService _renovationService;
        public SplitRenovationService(ISplitRenovationRepository splitRenovationRepository,
            IRoomRepository roomRepository,
            IRoomTypeRepository roomTypeRepository,
            IRenovationService renovationService)
        {
            _splitRenovationRepository = splitRenovationRepository;
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
            _renovationService = renovationService;
        }
        public async Task<IEnumerable<SplitRenovationDomainModel>> GetAll()
        {
            IEnumerable<SplitRenovation> splitRenovations = await _splitRenovationRepository.GetAll();
            return ParseToModel(splitRenovations);
        }
        public async Task<bool> validateSplitRenovation(CreateSplitRenovationDTO dto)
        {
            if (dto.resultRoomName1.Trim().Equals(String.Empty) || dto.resultRoomName2.Trim().Equals(String.Empty))
                throw new Exception("Invalid room name given");

            RoomType roomType1 = await _roomTypeRepository.GetById(dto.roomTypeId1);
            RoomType roomType2 = await _roomTypeRepository.GetById(dto.roomTypeId2);
            if (roomType1 == null || roomType2 == null)
                throw new Exception("No room type with such id exists");

            if (dto.StartDate >= dto.EndDate)
                throw new Exception("Start is equal or after end");
            Room split = await _roomRepository.GetRoomById(dto.SplitRoomId);
            if (split == null)
                throw new Exception("Non existant room");

            if (!_renovationService.IsAvaliable(split, dto).Result)
                throw new Exception("Room is already renovating in that period");
            return true;
        }
        public async Task<SplitRenovationDomainModel> Create(CreateSplitRenovationDTO dto)
        {
            if (validateSplitRenovation(dto).Result)
            {

                Room result1 = new Room
                {
                    RoomName = dto.resultRoomName1,
                    RoomTypeId = dto.roomTypeId1,
                    IsDeleted = false,
                    IsFormed = false,
                };
                _roomRepository.Post(result1);

                Room result2 = new Room
                {
                    RoomName = dto.resultRoomName2,
                    RoomTypeId = dto.roomTypeId2,
                    IsDeleted = false,
                    IsFormed = false,
                };
                _roomRepository.Post(result2);
                _roomRepository.Save();

                SplitRenovation newRenovation = new SplitRenovation
                {
                    EndDate = dto.EndDate,
                    StartDate = dto.StartDate,
                    ResultRoomId1 = result1.Id,
                    ResultRoomId2 = result2.Id,
                    SplitRoomId = dto.SplitRoomId,
                };
                _splitRenovationRepository.Post(newRenovation);
                _splitRenovationRepository.Save();
                return ParseToModel(newRenovation);
            }
            return null;
        }
        public SplitRenovationDomainModel ParseToModel(SplitRenovation splitRenovation)
        {
            return new SplitRenovationDomainModel
            {
                Id = splitRenovation.Id,
                EndDate = splitRenovation.EndDate,
                StartDate = splitRenovation.StartDate,
                ResultRoomId1 = splitRenovation.ResultRoomId1,
                ResultRoomId2 = splitRenovation.ResultRoomId2,
                SplitRoomId = splitRenovation.SplitRoomId
            };
        }
        public IEnumerable<SplitRenovationDomainModel> ParseToModel(IEnumerable<SplitRenovation> renovations)
        {
            List<SplitRenovationDomainModel> renovationModels = new List<SplitRenovationDomainModel>();
            foreach (var renovation in renovations)
            {
                renovationModels.Add(ParseToModel(renovation));
            }
            return renovationModels;
        }
    }
}
