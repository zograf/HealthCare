using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Interfaces 
{
    public interface IService<T> where T : class 
    {
        Task<IEnumerable<T>> GetAll();
    }
    
    public class DataIsNullException : Exception
    {
        public DataIsNullException() : base("Data doesn't exist")
        {
        }
    }
    public class AlreadyHandledException : Exception
    {
        public AlreadyHandledException() : base("Cannot approve/reject the approval because it has already been handled")
        {
        }
    }
    public class AntiTrollException : Exception
    {
        public AntiTrollException() : base("Patient is blocked via AntiTroll mechanism and cannot make requests")
        {
        }
    }
    public class DoctorNotAvailableException : Exception
    {
        public DoctorNotAvailableException() : base("Doctor is not available for given start time")
        {
        }
    }
    public class PatientNotAvailableException : Exception
    {
        public PatientNotAvailableException() : base("Patient is not available for given start time")
        {
        }
    }
    public class NoFreeRoomsException : Exception
    {
        public NoFreeRoomsException() : base("There are no available rooms for current procedure")
        {
        }
    }

    public class RoomTypeNotFoundException : Exception
    {
        public RoomTypeNotFoundException() : base("There is no room type with such id")
        {

        }
    } 
    
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User was not found")
        {
        }
    }
    
    public class UserIsBlockedException : Exception
    {
        public UserIsBlockedException() : base("User is blocked")
        {
        }
    }
    public class DateInPastExeption : Exception
    {
        public DateInPastExeption() : base("Entered date is in past")
        {
        }
    }
    
    public class PatientIsBlockedException : Exception
    {
        public PatientIsBlockedException() : base("Patient is blocked")
        {
        }
    }

    public class AnamnesisAlreadyExistsException : Exception
    {
        public AnamnesisAlreadyExistsException() : base("Examination already has anamnesis")
        {
        }
    }

    public class NotEnoughResourcesForTransfer : Exception
    {
        public NotEnoughResourcesForTransfer() : base("Not enough resources for equipment transfer")
        {
        }
    }
    public class ReferralCannotBeUsedException : Exception
    {
        public ReferralCannotBeUsedException() : base("This referral letter has already been used")
        {
        }
    }

    public class NoAvailableSpecialistsException : Exception
    {
        public NoAvailableSpecialistsException() : base("There are no available specialists for the given date")
        {
        }
    }

    public class PatientIsAllergicException : Exception
    {
        public PatientIsAllergicException(string allergen) : base("You can't prescribe the chosen drug to the patient. Allergic to " + allergen + ".")
        {
        }
    }
    public class ReferredYourselfException : Exception
    {
        public ReferredYourselfException() : base("You can't reffer yourself")
        {
        }
    }

    public class DrugSuggestionAlreadyApprovedException : Exception
    {
        public DrugSuggestionAlreadyApprovedException() : base("Drug suggestion is already approved")
        {
        }
    }

    public class DrugSuggestionAlreadyRejectedException : Exception
    {
        public DrugSuggestionAlreadyRejectedException() : base("Drug suggestion is already rejected")
        {
        }
    }

    public class LateForDaysOffRequestException : Exception
    {
        public LateForDaysOffRequestException() : base("Days off must be requested at least 2 days in advance (5 days for urgent requests))")
        {
        }
    }
    
    public class DoctorIsNotFreeOnDayException : Exception
    {
        public DoctorIsNotFreeOnDayException() : base("You are not free on requested days. Check your schedule and make sure you have no appointmets in the selected date range.")
        {
        }
    }

    public class NumberOfUrgentDaysOfNotAllowedException : Exception
    {
        public NumberOfUrgentDaysOfNotAllowedException() : base("You can request a maximum of 5 days off with urgent requests.")
        {
        }
    }
    
    public class DaysOffRequestAlreadyHandledException : Exception
    {
        public DaysOffRequestAlreadyHandledException() : base("This request was already handled.")
        {
        }
    }

}
