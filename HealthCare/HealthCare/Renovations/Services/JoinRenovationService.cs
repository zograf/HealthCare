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
    public class JoinRenovationService : IJoinRenovationService
    {
        private IJoinRenovationRepository _joinRenovationRepository;
        private IRoomRepository _roomRepository;
        private IRoomTypeRepository _roomTypeRepository;
        private IRenovationService _renovationService;
        public JoinRenovationService(IJoinRenovationRepository joinRenovationRepository,
            IRoomRepository roomRepository,
            IRoomTypeRepository roomTypeRepository,
            IRenovationService renovationService)
        {
            _joinRenovationRepository = joinRenovationRepository;
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
            _renovationService = renovationService;
        }
        public async Task<IEnumerable<JoinRenovationDomainModel>> GetAll()
        {
            IEnumerable<JoinRenovation> joinRenovations = await _joinRenovationRepository.GetAll();
            return ParseToModel(joinRenovations);
        }
        public async Task<bool> validateJoinRenovation(CreateJoinRenovationDTO dto)
        {
            if (dto.StartDate >= dto.EndDate)
                throw new Exception("Start is equal or after end");

            if (dto.resultRoomName.Trim().Equals(String.Empty))
                throw new Exception("Invalid room name given");

            Room join1 = await _roomRepository.GetRoomById(dto.JoinRoomId1);
            Room join2 = await _roomRepository.GetRoomById(dto.JoinRoomId2);
            if (join1 == null || join2 == null)
                throw new Exception("Non existant room");

            RoomType roomType = await _roomTypeRepository.GetById(dto.roomTypeId);
            if (roomType == null)
                throw new Exception("Non existant room type");

            if (!_renovationService.IsAvaliable(join1, dto).Result || !_renovationService.IsAvaliable(join2, dto).Result)
                throw new Exception("Room is already renovating in that period");

            return true;
        }
        public async Task<JoinRenovationDomainModel> Create(CreateJoinRenovationDTO dto)
        {
            if (validateJoinRenovation(dto).Result)
            {

                Room result = new Room
                {
                    RoomName = dto.resultRoomName,
                    RoomTypeId = dto.roomTypeId,
                    IsDeleted = false,
                    IsFormed = false
                };

                _roomRepository.Post(result);
                _roomRepository.Save();

                JoinRenovation newJoinRenovation = new JoinRenovation
                {
                    EndDate = dto.EndDate,
                    StartDate = dto.StartDate,
                    JoinRoomId1 = dto.JoinRoomId1,
                    JoinRoomId2 = dto.JoinRoomId2,
                    ResultRoomId = result.Id,
                };
                _joinRenovationRepository.Post(newJoinRenovation);
                _joinRenovationRepository.Save();
                return ParseToModel(newJoinRenovation);
            }
            return null;
        }
        public JoinRenovationDomainModel ParseToModel(JoinRenovation joinRenovation)
        {
            return new JoinRenovationDomainModel
            {
                Id = joinRenovation.Id,
                EndDate = joinRenovation.EndDate,
                StartDate = joinRenovation.StartDate,
                JoinRoomId1 = joinRenovation.JoinRoomId1,
                JoinRoomId2 = joinRenovation.JoinRoomId2,
                ResultRoomId = joinRenovation.ResultRoomId
            };
        }
        public IEnumerable<JoinRenovationDomainModel> ParseToModel(IEnumerable<JoinRenovation> renovations)
        {
            List<JoinRenovationDomainModel> renovationModels = new List<JoinRenovationDomainModel>();
            foreach (var renovation in renovations)
            {
                renovationModels.Add(ParseToModel(renovation));
            }
            return renovationModels;
        }
    }
}
