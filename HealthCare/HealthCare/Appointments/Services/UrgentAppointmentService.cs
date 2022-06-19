using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services
{
    public class UrgentAppointmentService : IUrgentAppointmentService
    {

        private IExaminationRepository _examinationRepository;
        private IOperationRepository _operationRepository;
        private IDoctorRepository _doctorRepository;

        
        private IDoctorService _doctorService;
        private INotificationService _notificationService;
        private IRoomService _roomService;
        private IPatientService _patientService;
        public UrgentAppointmentService(IExaminationRepository examinationRepository,
                                  IDoctorRepository doctorRepository,
                                  IDoctorService doctorService,
                                  INotificationService notificationService,
                                  IRoomService roomService,
                                  IPatientService patientService,
                                  IOperationRepository operationRepository)
        {
            _examinationRepository = examinationRepository;
            _doctorRepository = doctorRepository;
            _doctorService = doctorService;
            _notificationService = notificationService;
            _roomService = roomService;
            _patientService = patientService;
            _operationRepository = operationRepository;
        }

        public async Task<DateTime?> FirstStartTime(List<KeyValuePair<DateTime, DateTime>> schedule, decimal duration)
        {
            DateTime now = DateTime.Now;
            DateTime limit = UtilityService.RemoveSeconds(now.AddHours(2));
            foreach (KeyValuePair<DateTime, DateTime> pair in schedule)
            {
                // Now: 20:00, Limit: 22:00, Schedule: 14:00 - 16:00 -> continue
                if (now > pair.Value) continue;
                // Now: 20:00, Limit: 22:00, Schedule: 15:00 - 21:00 -> 20:00
                if (now >= pair.Key && now <= pair.Value && (pair.Value - now).Minutes >= duration) return now;
                // Now: 20:00, Limit: 22:00, Schedule: 21:00 - 23:00 -> 21:00
                if (limit >= pair.Key && pair.Key > now && (pair.Value - pair.Key).Minutes >= duration) return pair.Key;
                // Now: 20:00, Limit: 22:00, Schedule: 23:00 - 23:30 -> break completely (every other
                // pair will be greater than this one, so return null)
                return null;
            }
            return null;
        }

        // DoctorService is needed for doctor's schedule
        public async Task<Boolean> CreateUrgent(CreateUrgentAppointmentDTO dto)
        {
            AppointmentDomainModel appointmentModel = new AppointmentDomainModel
            {
                PatientId = dto.PatientId,
                Duration = dto.Duration,
                Type = dto.IsExamination ? Appointment.Examination : Appointment.Operation,
                IsDeleted = false,
                IsEmergency = true
            };
            // Find examination in the first 2 hours for any doctor that matches the specialization criteria
            List<Doctor> doctors = (List<Doctor>)await _doctorRepository.GetBySpecialization(dto.SpecializationId);
            if (doctors == null || doctors.Count == 0) throw new NoAvailableSpecialistsException();
            // Find start times (to sort by earliest) 
            List<KeyValuePair<DateTime, decimal>> urgentStartTimes = await GetUrgentStartTimes(doctors);

            urgentStartTimes.Sort((x, y) => x.Key.CompareTo(y.Key));
            // Try to create examination
            Boolean isCreated = await ParsePairs(appointmentModel, urgentStartTimes);
            _ = await SendNotifications(appointmentModel.DoctorId, appointmentModel.PatientId);
            return isCreated;
        }

        public async Task<Boolean> TryCreateOperation(AppointmentDomainModel appointmentModel)
        {
            decimal roomId = await _roomService.GetAvailableRoomId(appointmentModel.StartTime, "operation");
            if (roomId == -1) return false;
            OperationDomainModel operationModel = appointmentModel.ToOperationModel();
            operationModel.RoomId = roomId;
            Operation operation = OperationService.ParseFromModel(operationModel);
            _ = _operationRepository.Post(operation);
            _operationRepository.Save();
            return true;
        }

        public async Task<Boolean> TryCreateExamination(AppointmentDomainModel appointmentModel)
        {
            decimal roomId = await _roomService.GetAvailableRoomId(appointmentModel.StartTime, "examination");
            if (roomId == -1) return false;
            ExaminationDomainModel examinationModel = appointmentModel.ToExaminationModel();
            examinationModel.RoomId = roomId;
            Examination examination = ExaminationService.ParseFromModel(examinationModel);
            _ = _examinationRepository.Post(examination);
            _examinationRepository.Save();
            return true;
        }

        public async Task<Boolean> TryCreateAppointment(AppointmentDomainModel appointmentModel)
        {
            if (appointmentModel.Type == Appointment.Examination) return await TryCreateExamination(appointmentModel);
            return await TryCreateOperation(appointmentModel);
        }

        public async Task<Boolean> ParsePairs(AppointmentDomainModel appointmentModel, 
            List<KeyValuePair<DateTime, decimal>> urgentStartTimes)
        {
            Boolean flag;
            foreach (KeyValuePair<DateTime, decimal> pair in urgentStartTimes)
            {
                appointmentModel.StartTime = UtilityService.RemoveSeconds(pair.Key);
                appointmentModel.DoctorId = pair.Value;
                flag = await TryCreateAppointment(appointmentModel);
                if (flag) return true;
            }
            return false;
        }

        public async Task<List<KeyValuePair<DateTime, decimal>>> GetUrgentStartTimes(List<Doctor> doctors)
        {
            List<KeyValuePair<DateTime, decimal>> result = new List<KeyValuePair<DateTime, decimal>>();
            foreach (Doctor doctor in doctors)
            {
                var schedule = (List<KeyValuePair<DateTime, DateTime>>)await _doctorService.GetAvailableSchedule(doctor.Id);
                DateTime? startTime = await FirstStartTime(schedule, 15);
                if (startTime.HasValue)
                    result.Add(new KeyValuePair<DateTime, decimal>(startTime.GetValueOrDefault(), doctor.Id));
            }
            return result;
        }

        public async Task<IEnumerable<IEnumerable<RescheduleDTO>>> FindFiveAppointments(CreateUrgentAppointmentDTO dto)
        {
            // For every doctor try to find a single reschedule 
            List<Doctor> doctors = (List<Doctor>)await _doctorRepository.GetAll();
            List<List<List<RescheduleDTO>>> reschedule = new List<List<List<RescheduleDTO>>>();
            foreach (Doctor doctor in doctors)
                reschedule.Add(await GetRescheduleForDoctor(dto, doctor.Id));
            List<KeyValuePair<DateTime, List<RescheduleDTO>>> rescheduleSorted = new List<KeyValuePair<DateTime, List<RescheduleDTO>>>();
            foreach (List<List<RescheduleDTO>> item in reschedule)
                rescheduleSorted.AddRange(await FindRescheduleTime(item, dto.PatientId));
            rescheduleSorted.Sort((x, y) => x.Key.CompareTo(y.Key));
            List<List<RescheduleDTO>> result = new List<List<RescheduleDTO>>();
            foreach (var item in rescheduleSorted)
            {
                result.Add(item.Value);
                if (result.Count > 5) break;
            }
            return result;
        }

        public async Task<List<KeyValuePair<DateTime, List<RescheduleDTO>>>> FindRescheduleTime(List<List<RescheduleDTO>> schedule, decimal patientId)
        {
            List<KeyValuePair<DateTime, DateTime>> freePatientSchedule =
                (List<KeyValuePair<DateTime, DateTime>>)await _patientService.GetSchedule(patientId);
            decimal doctorId = schedule[0][0].DoctorId;
            List<KeyValuePair<DateTime, DateTime>> freeDoctorSchedule =
                (List<KeyValuePair<DateTime, DateTime>>)await _doctorService.GetAvailableSchedule(doctorId);
            List<KeyValuePair<DateTime, List<RescheduleDTO>>> result = new List<KeyValuePair<DateTime, List<RescheduleDTO>>>();
            foreach (List<RescheduleDTO> sequence in schedule)
            {
                _ = await SetRescheduleForSequence(sequence, freePatientSchedule, freeDoctorSchedule);
                DateTime max = await FindMaxDateInSequence(sequence);
                result.Add(new KeyValuePair<DateTime, List<RescheduleDTO>>(max, sequence));
            }
            return result;
        }

        public async Task<DateTime> FindMaxDateInSequence(List<RescheduleDTO> sequence)
        {
            DateTime max = DateTime.Now;
            foreach (RescheduleDTO item in sequence)
                if (item.RescheduleTime > max) max = item.RescheduleTime;
            return max;
        }

        public async Task<Boolean> SetRescheduleForSequence(List<RescheduleDTO> sequence,
            List<KeyValuePair<DateTime, DateTime>> patientSchedule,
            List<KeyValuePair<DateTime, DateTime>> doctorSchedule)
        {
            foreach (RescheduleDTO item in sequence)
                _ = await SetRescheduleForDTO(item, patientSchedule, doctorSchedule);
            return true;
        }

        public int GetIndex(List<KeyValuePair<DateTime, DateTime>> schedule, DateTime reference)
        {
            for (int i = 0; i < schedule.Count; i++)
                if (schedule[i].Key > reference)
                    return i;
            return 0;
        }

        public async Task<Boolean> SetRescheduleForDTO(RescheduleDTO dto,
            List<KeyValuePair<DateTime, DateTime>> patientSchedule,
            List<KeyValuePair<DateTime, DateTime>> doctorSchedule)
        {
            int patientIndex = GetIndex(patientSchedule, dto.StartTime);
            int doctorIndex = GetIndex(patientSchedule, dto.StartTime);

            Boolean found = false;
            while (!found)
            {
                KeyValuePair<DateTime, DateTime> doctorPair = doctorSchedule[doctorIndex];
                KeyValuePair<DateTime, DateTime> patientPair = patientSchedule[patientIndex];
                if (!UtilityService.IsDateTimeOverlap(doctorPair, patientPair))
                {
                    // Update smaller
                    if (doctorPair.Key < patientPair.Key && doctorPair.Value < patientPair.Value)
                        doctorIndex++;
                    else
                        patientIndex++;
                    continue;
                }

                DateTime rescheduleTime = CalculateRescheduleTime(doctorPair, patientPair, dto.Duration);
                if (rescheduleTime == DateTime.MaxValue) continue;
                dto.RescheduleTime = rescheduleTime;
                found = true;
            }
            return true;
        }

        public DateTime CalculateRescheduleTime(KeyValuePair<DateTime, DateTime> first, KeyValuePair<DateTime, DateTime> second, decimal duration)
        {
            decimal window = 0;
            if (first.Key < second.Key)
            {
                if (first.Value < second.Value)
                    window = (first.Value - second.Key).Minutes;
                else
                    window = (second.Value - second.Key).Minutes;

                if (window >= duration)
                    return second.Key;
            }
            if (first.Value > second.Value)
                window = (second.Value - first.Key).Minutes;
            else
                window = (first.Value - first.Key).Minutes;

            if (window >= duration)
                return first.Key;

            return DateTime.MaxValue;
        }

        public async Task<List<List<RescheduleDTO>>> GetRescheduleForDoctor(CreateUrgentAppointmentDTO dto, 
            decimal doctorId)
        {
            List<KeyValuePair<DateTime, DateTime>> freeSchedule =
                (List<KeyValuePair<DateTime, DateTime>>)await _doctorService.GetAvailableSchedule(doctorId);
            List<KeyValuePair<DateTime, DateTime>> busySchedule =
                (List<KeyValuePair<DateTime, DateTime>>)await _doctorService.GetBusySchedule(doctorId);
            // Loop variables
            DateTime now = UtilityService.RemoveSeconds(DateTime.Now);
            DateTime new_now = now;
            DateTime limit = UtilityService.RemoveSeconds(DateTime.Now.AddHours(2));
            DateTime first, second;
            int index = GetFirstIndex(freeSchedule, false);
            int size = 0;

            List<List<RescheduleDTO>> result = new List<List<RescheduleDTO>>();
            List<RescheduleDTO> tempList = new List<RescheduleDTO>();
            if (index == -1)
                // If doctor has no free room in his schedule
                return CalculateWithNoFreeTime(busySchedule, dto.PatientId, doctorId, dto.Duration);

            // If doctor has free time in his schedule
            int busyIndex = GetFirstIndex(busySchedule, true);
            while (index != -1 && busyIndex != -1)
            {
                bool flagFree = false;
                KeyValuePair<DateTime, DateTime> freePair = freeSchedule[index];
                KeyValuePair<DateTime, DateTime> busyPair = busySchedule[busyIndex];
                if (freePair.Value == busyPair.Key)
                {
                    flagFree = true;
                    new_now = busyPair.Value;
                }
                else if (freePair.Key == busyPair.Value)
                {
                    flagFree = true;
                    new_now = freePair.Value;
                }
                else
                    new_now = busyPair.Key;

                int old_free = index;
                int old_busy = busyIndex;
                result.Add(FindSequence(freeSchedule, busySchedule, index, busyIndex, dto.Duration, now, dto.PatientId, doctorId));

                if (flagFree)
                    if (UpdateIndex(freeSchedule, old_free) != -1) index = UpdateIndex(freeSchedule, old_free);
                if (UpdateIndex(busySchedule, old_busy) != -1) busyIndex = UpdateIndex(busySchedule, old_busy);
                if (new_now > limit) break;
                now = new_now;
            }
            return result;
        }

        public List<RescheduleDTO> FindSequence(List<KeyValuePair<DateTime, DateTime>> freeSchedule, List<KeyValuePair<DateTime, DateTime>> busySchedule,
            int index, int busyIndex, decimal duration, DateTime now, decimal patientId, decimal doctorId)
        {
            int size = 0;
            DateTime rescheduleTime = now;
            List<RescheduleDTO> sequence = new List<RescheduleDTO>();
            while (size < duration || (index == -1 && busyIndex == -1))
            {
                Boolean flagFree = false;
                KeyValuePair<DateTime, DateTime> freePair = freeSchedule[index];
                KeyValuePair<DateTime, DateTime> busyPair = busySchedule[busyIndex];
                DateTime first, second;
                // Max possible range (if rescheduled)
                if (freePair.Value == busyPair.Key)
                {
                    first = freePair.Key;
                    second = busyPair.Value;
                    flagFree = true;
                }
                else if (freePair.Key == busyPair.Value)
                {
                    first = busyPair.Key;
                    second = freePair.Value;
                    flagFree = true;
                }
                else
                {
                    first = busyPair.Key;
                    second = busyPair.Value;
                }
                size += (second - now).Minutes;
                sequence.Add(new RescheduleDTO { PatientId = patientId, DoctorId = doctorId, StartTime = second, EndTime = first, UrgentStartTime = rescheduleTime });
                now = first;
                // Update
                if (flagFree)
                    if (UpdateIndex(freeSchedule, index) != -1) index = UpdateIndex(freeSchedule, index);
                if (UpdateIndex(busySchedule, busyIndex) != -1) busyIndex = UpdateIndex(busySchedule, busyIndex);
            }

            return sequence;
        }

        public List<List<RescheduleDTO>> CalculateWithNoFreeTime(List<KeyValuePair<DateTime, DateTime>> busySchedule,
            decimal patientId, decimal doctorId, decimal duration)
        {
            List<RescheduleDTO> tempList = new List<RescheduleDTO>();
            List<List<RescheduleDTO>> result = new List<List<RescheduleDTO>>();
            DateTime first, second, now = DateTime.Now;
            decimal size = 0;
            for (var i = GetFirstIndex(busySchedule, true); i < busySchedule.Count - 1; i++)
            {
                first = busySchedule[i].Value;
                second = busySchedule[i + 1].Key;
                size = 0;
                DateTime rescheduleTime = now;
                while (size < duration)
                {
                    size += (second - now).Minutes;
                    tempList.Add(new RescheduleDTO { PatientId = patientId, DoctorId = doctorId, StartTime = second, EndTime = first, UrgentStartTime = rescheduleTime });
                }
                now = first;
                result.Add(tempList);
            }

            return result;
        }

        public int UpdateIndex(List<KeyValuePair<DateTime, DateTime>> schedule, int lastIndex)
        {
            if (lastIndex + 1 == schedule.Count) return -1;
            KeyValuePair<DateTime, DateTime> pair = schedule[lastIndex + 1];
            if (pair.Key > UtilityService.RemoveSeconds(DateTime.Now).AddHours(2)) return -1;
            return lastIndex + 1;
        }

        public int GetFirstIndex(List<KeyValuePair<DateTime, DateTime>> schedule, bool isBusy)
        {
            DateTime now = UtilityService.RemoveSeconds(DateTime.Now);
            DateTime limit = now.AddHours(2);
            for (var i = 0; i < schedule.Count; i++)
            {
                KeyValuePair<DateTime, DateTime> pair = schedule[i];
                if (pair.Key > limit) break;
                if (isBusy && pair.Value > now) return i;
                if (!isBusy && pair.Key >= now) return i;
            }

            return -1;
        }

        public async Task<Boolean> AppointUrgent(List<RescheduleDTO> dto)
        {
            foreach (RescheduleDTO item in dto)
                _ = await RescheduleOne(item);
            // Any dto will do
            return await MakeUrgent(dto[0]);
        }

        public async Task<Boolean> RescheduleOneExamination(RescheduleDTO dto)
        {
            Examination examination = await _examinationRepository.GetByParams(dto.DoctorId, dto.PatientId, dto.StartTime);
            examination.StartTime = dto.RescheduleTime;
            _ = _examinationRepository.Update(examination);
            _examinationRepository.Save();
            _ = await SendNotifications(dto.DoctorId, dto.PatientId);
            return true;
        }

        public async Task<Boolean> RescheduleOneOperation(RescheduleDTO dto)
        {
            Operation operation = await _operationRepository.GetByParams(dto.DoctorId, dto.PatientId, dto.StartTime);
            operation.StartTime = dto.RescheduleTime;
            _ = _operationRepository.Update(operation);
            _operationRepository.Save();
            _ = await SendNotifications(dto.DoctorId, dto.PatientId);
            return true;
        }

        public async Task<Boolean> RescheduleOne(RescheduleDTO dto)
        {
            if (dto.Duration == 15)
                return await RescheduleOneExamination(dto);
            return await RescheduleOneOperation(dto);
        }

        public async Task<Boolean> SendNotifications(decimal doctorId = 0, decimal patientId = 0)
        {
            KeyValuePair<string, string> content = new KeyValuePair<string, string>("Rescheduling",
                "Your appointment has been rescheduled. Please check your schedule");
            if (doctorId != 0)
                _ = await _notificationService.Send(new SendNotificationDTO { IsPatient = false, Content = content, PersonId = doctorId });
            if (patientId != 0)
                _ = await _notificationService.Send(new SendNotificationDTO { IsPatient = true, Content = content, PersonId = patientId });
            return true;
        }


        public async Task<Boolean> MakeUrgentExamination(RescheduleDTO dto)
        {
            ExaminationDomainModel examinationModel = new ExaminationDomainModel
            {
                DoctorId = dto.DoctorId,
                IsDeleted = false,
                IsEmergency = true,
                StartTime = dto.UrgentStartTime,
                PatientId = dto.PatientId,
                RoomId = await _roomService.GetAvailableRoomId(dto.UrgentStartTime, "examination")
            };
            _ = _examinationRepository.Post(ExaminationService.ParseFromModel(examinationModel));
            _examinationRepository.Save();
            return true;
        }

        public async Task<Boolean> MakeUrgentOperation(RescheduleDTO dto)
        {
            OperationDomainModel operationModel = new OperationDomainModel
            {
                DoctorId = dto.DoctorId,
                IsDeleted = false,
                IsEmergency = true,
                StartTime = dto.UrgentStartTime,
                PatientId = dto.PatientId,
                RoomId = await _roomService.GetAvailableRoomId(dto.UrgentStartTime, "operation", dto.Duration)
            };
            _ = _operationRepository.Post(OperationService.ParseFromModel(operationModel));
            _operationRepository.Save();
            return true;
        }
        public async Task<Boolean> MakeUrgent(RescheduleDTO dto)
        {
            if (dto.Duration == 15)
                return await MakeUrgentExamination(dto);
            return await MakeUrgentOperation(dto);
        }

        public Task<IEnumerable<AppointmentDomainModel>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}

