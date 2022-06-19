using HealthCare.Data.Entities;
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
    public class QuestionService : IQuestionService
    {
        private IQuestionRepository _questionRepository;
        private IAnswerRepository _answerRepository;
        public QuestionService(IQuestionRepository questionRepository, IAnswerRepository answerRepository)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }
        public async Task<IEnumerable<QuestionDomainModel>> GetAll()
        {
            IEnumerable<Question> questions = await _questionRepository.GetAll();
            IEnumerable<QuestionDomainModel> questionModels = parseToModel(questions);
            
            return questionModels;
        }

        private IEnumerable<QuestionDomainModel> parseToModel(IEnumerable<Question> questions)
        {
            List<QuestionDomainModel> questionModels = new List<QuestionDomainModel>();
            foreach(Question question in questions)
            {
                questionModels.Add(parseToModel(question));
            }
            return questionModels;
        }

        private QuestionDomainModel parseToModel(Question question)
        {
            return new QuestionDomainModel
            {
                Id = question.Id,
                Text = question.Text,
                IsForDoctor = question.IsForDoctor,
            };
        }
    }
}
