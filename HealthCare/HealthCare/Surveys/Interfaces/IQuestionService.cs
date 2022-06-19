using HealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionDomainModel>> GetAll();
    }
}
