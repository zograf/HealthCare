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
    public class SimpleRenovationService : ISimpleRenovationService
    {
        private ISimpleRenovationRepository _simpleRenovationRepository;
        private IRoomRepository _roomRepository;
        private IRoomTypeRepository _roomTypeRepository;
        private IRenovationService _renovationService;
        public SimpleRenovationService(ISimpleRenovationRepository simpleRenovationRepository, 
            IRoomRepository roomRepository,
            IRoomTypeRepository roomTypeRepository,
            IRenovationService renovationService)
        {
            _simpleRenovationRepository = simpleRenovationRepository;
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
            _renovationService = renovationService;

        }

        public async Task<IEnumerable<SimpleRenovationDomainModel>> GetAll()
        {
            IEnumerable<SimpleRenovation> simpleRenovations = await _simpleRenovationRepository.GetAll();
            return ParseToModel(simpleRenovations);
        }
        public async Task<bool> validateSimpleRenovation(CreateSimpleRenovationDTO renovation)
        {
            if (renovation.StartDate >= renovation.EndDate)
                throw new Exception("Start is equal or after end");
            Room room = await _roomRepository.GetRoomById(renovation.RoomId);
            if (room == null)
                throw new Exception("Non existant room");

            if (!_renovationService.IsAvaliable(room, renovation).Result)
                throw new Exception("Room is already renovating in that period");
            return true;
        }
        public async Task<SimpleRenovationDomainModel> Create(CreateSimpleRenovationDTO dto)
        {
            if (validateSimpleRenovation(dto).Result)
            {
                SimpleRenovation newSimpleRenovation = new SimpleRenovation
                {
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    RoomId = dto.RoomId
                };
                _simpleRenovationRepository.Post(newSimpleRenovation);
                _simpleRenovationRepository.Save();
                return ParseToModel(newSimpleRenovation);
            }
            return null;

        }
        public SimpleRenovationDomainModel ParseToModel(SimpleRenovation simpleRenovation)
        {
            return new SimpleRenovationDomainModel
            {
                Id = simpleRenovation.Id,
                EndDate = simpleRenovation.EndDate,
                StartDate = simpleRenovation.StartDate,
                RoomId = simpleRenovation.RoomId
            };
        }
        public IEnumerable<SimpleRenovationDomainModel> ParseToModel(IEnumerable<SimpleRenovation> renovations)
        {
            List<SimpleRenovationDomainModel> renovationModels = new List<SimpleRenovationDomainModel>();
            foreach (var renovation in renovations)
            {
                renovationModels.Add(ParseToModel(renovation));
            }
            return renovationModels;
        }
    }
}
