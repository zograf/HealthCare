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
    public class RenovationService : IRenovationService
    {
        private readonly IJoinRenovationRepository _joinRenovationRepository;
        private readonly ISplitRenovationRepository _splitRenovationRepository;
        private readonly ISimpleRenovationRepository _simpleRenovationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IExaminationRepository _examinationRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IEquipmentRepository _equipmentRepository;


        public RenovationService(IJoinRenovationRepository joinRenovationRepository,
            ISplitRenovationRepository splitRenovationRepository,
            ISimpleRenovationRepository simpleRenovationRepository,
            IRoomRepository roomRepository,
            IExaminationRepository examinationRepository,
            IOperationRepository operationRepository,
            IInventoryRepository inventoryRepository,
            IEquipmentRepository equipmentRepository)
        {
            _joinRenovationRepository = joinRenovationRepository;
            _splitRenovationRepository = splitRenovationRepository;
            _simpleRenovationRepository = simpleRenovationRepository;
            _roomRepository = roomRepository;
            _examinationRepository = examinationRepository;
            _operationRepository = operationRepository;
            _inventoryRepository = inventoryRepository;
            _equipmentRepository = equipmentRepository;
        }


        public async Task<IEnumerable<RenovationDomainModel>> GetAll()
        {
            IEnumerable<Renovation> renovations = await GetAllRenovations();
            return parseToModel(renovations);
        }

        public async Task<IEnumerable<Renovation>> GetRenovation(Room room)
        {
            IEnumerable<Renovation> renovations = await GetAllRenovations();
            return renovations.Where(r => room.Id == room.Id);
        }

        private async Task<IEnumerable<Examination>> GetExaminations(Room room)
        {
            IEnumerable<Examination> examinations = await _examinationRepository.GetAll();
            return examinations.Where(e => e.RoomId == room.Id);
        }

        private async Task<IEnumerable<Operation>> GetOperations(Room room)
        {
            IEnumerable<Operation> operations = await _operationRepository.GetAll();
            return operations.Where(o => o.RoomId == room.Id);
        }
        public async Task<bool> IsAvaliable(Room room, CreateRenovationDTO renovationToCheck)
        {
            IEnumerable<Renovation> roomRenovations = await GetRenovation(room);
            foreach(Renovation renovation in roomRenovations)
            {
                if (UtilityService.IsDateTimeOverlap(renovation.StartDate, renovation.EndDate,
                    renovationToCheck.StartDate, renovationToCheck.EndDate))
                    return false;
            }

            IEnumerable<Examination> roomExaminations = await GetExaminations(room);
            foreach (Examination examination in roomExaminations)
            {
                if (UtilityService.IsDateTimeOverlap(examination.StartTime, examination.StartTime.AddMinutes(15),
                    renovationToCheck.StartDate, renovationToCheck.EndDate))
                    return false;
            }

            IEnumerable<Operation> roomOperations = await GetOperations(room);
            foreach (Operation operation in roomOperations)
            {
                if (UtilityService.IsDateTimeOverlap(operation.StartTime, operation.StartTime.AddMinutes(Decimal.ToDouble(operation.Duration)),
                    renovationToCheck.StartDate, renovationToCheck.EndDate))
                    return false;
            }

            return true;
        }
        public async Task ExecuteComplexRenovations()
        {
            await ExecuteJoinRenovations();
            await ExecuteSplitRenovations();
        }

        public async Task ExecuteSplitRenovations()
        {
            IEnumerable<SplitRenovation> renovations = await _splitRenovationRepository.GetAll();
            foreach(SplitRenovation renovation in renovations)
            {
                Room splitRoom = await _roomRepository.GetRoomById(renovation.SplitRoomId);
                Room resultRoom1 = await _roomRepository.GetRoomById(renovation.ResultRoomId1);
                Room resultRoom2 = await _roomRepository.GetRoomById(renovation.ResultRoomId2);
                if(renovation.EndDate < DateTime.Now && !resultRoom1.IsFormed && !resultRoom2.IsFormed)
                {
                    splitRoom.IsDeleted = true;
                    await TransferEquipmentToStorage(splitRoom);
                    resultRoom1.IsFormed = true;
                    resultRoom2.IsFormed = true;

                    _roomRepository.Update(splitRoom);
                    _roomRepository.Update(resultRoom1);
                    _roomRepository.Update(resultRoom2);
                    _roomRepository.Save();
                }
            }
        }

        public async Task ExecuteJoinRenovations()
        {
            IEnumerable<JoinRenovation> renovations = await _joinRenovationRepository.GetAll();
            foreach (JoinRenovation renovation in renovations)
            {
                Room joinRoom1 = await _roomRepository.GetRoomById(renovation.JoinRoomId1);
                Room joinRoom2 = await _roomRepository.GetRoomById(renovation.JoinRoomId2);
                Room resultRoom = await _roomRepository.GetRoomById(renovation.ResultRoomId);
                if (renovation.EndDate < DateTime.Now && !resultRoom.IsFormed)
                {
                    joinRoom1.IsDeleted = true;
                    joinRoom2.IsDeleted = true;
                    await TransferEquipmentToStorage(joinRoom1);
                    await TransferEquipmentToStorage(joinRoom2);
                    resultRoom.IsFormed = true;
                    
                    _roomRepository.Update(joinRoom1);
                    _roomRepository.Update(joinRoom2);
                    _roomRepository.Update(resultRoom);
                    _roomRepository.Save();
                }
            }
        }

        private async Task TransferEquipmentToStorage(Room room)
        {
            // get all inventories that posses room equipment
            IEnumerable<Inventory> roomInventories = await _inventoryRepository.Get(room);

            // get storage room
            Room storageRoom = await _roomRepository.GetRoomByName("storage");
            if (storageRoom == null)
                throw new Exception("No storage in system");
            
            // move all equipment to storage room
            foreach(Inventory roomInventory in roomInventories)
            {
                Equipment equipment = await _equipmentRepository.GetById(roomInventory.EquipmentId);
                Inventory storageRoomInventory = await _inventoryRepository.Get(storageRoom, equipment);

                // make new inventory if it doesn't exist
                if(storageRoomInventory == null)
                {
                    storageRoomInventory = new Inventory
                    {
                        RoomId = storageRoom.Id,
                        EquipmentId = equipment.Id,
                        Amount = roomInventory.Amount,
                        IsDeleted = false,
                    };
                    _inventoryRepository.Post(storageRoomInventory);
                }
                else
                {
                    storageRoomInventory.Amount += roomInventory.Amount;
                    _inventoryRepository.Update(storageRoomInventory);
                }
                roomInventory.Amount = 0;
                _inventoryRepository.Update(roomInventory);   
                _inventoryRepository.Save();
                

            }
        }
        public async Task<IEnumerable<Renovation>> GetAllRenovations()
        {
            IEnumerable<Renovation> simpleRenovations = await _simpleRenovationRepository.GetAll();
            IEnumerable<Renovation> joinRenovations = await _joinRenovationRepository.GetAll();
            IEnumerable<Renovation> splitRenovations = await _splitRenovationRepository.GetAll();

            IEnumerable<Renovation> result = new List<Renovation>();

            if (simpleRenovations != null)
                result = result.Concat<Renovation>(simpleRenovations);
            if (joinRenovations != null)
                result = result.Concat<Renovation>(joinRenovations);
            if (splitRenovations != null)
                result = result.Concat<Renovation>(splitRenovations);

            return result;
        }
        public RenovationDomainModel parseToModel(Renovation renovation)
        {
            return new RenovationDomainModel
            {
                Id = renovation.Id,
                EndDate = renovation.EndDate,
                StartDate = renovation.StartDate,
            };
        }

        private IEnumerable<RenovationDomainModel> parseToModel(IEnumerable<Renovation> renovations)
        {
            List<RenovationDomainModel> renovationModels = new List<RenovationDomainModel>();
            foreach (Renovation renovation in renovations)
            {
                renovationModels.Add(parseToModel(renovation));
            }
            return renovationModels;
        }
    }
}
