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
    public class AnswerService : IAnswerService
    {
        private IAnswerRepository _answerRepository;
        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<IEnumerable<AnswerDomainModel>> GetAll()
        {
            var answers = await _answerRepository.GetAll();
            return parseToModel(answers);
        }

        public async Task<IEnumerable<AnswerDomainModel>> GetForDoctor(decimal id)
        {
            var answers = await _answerRepository.GetForDoctor(id);
            return parseToModel(answers);
        }

        public async Task<IEnumerable<AnswerDomainModel>> GetForHospital()
        {
            var answers = await _answerRepository.GetForHospital();
            return parseToModel(answers);
        }
        private IEnumerable<AnswerDomainModel> parseToModel(IEnumerable<Answer> answers)
        {
            List<AnswerDomainModel> answerModels = new List<AnswerDomainModel>();
            foreach (var answer in answers)
            {
                answerModels.Add(parseToModel(answer));
            }
            return answerModels;
        }

        private AnswerDomainModel parseToModel(Answer answer)
        {
            return new AnswerDomainModel
            {
                Id = answer.Id,
                AnswerText  = answer.AnswerText,
                Evaluation = answer.Evaluation,
                DoctorId = answer.DoctorId,
                PatientId  = answer.PatientId,
                QuestionId = answer.QuestionId
            };
        }

        private Answer parseFromModel(AnswerDomainModel answerModel)
        {
            return new Answer
            {
                Id = answerModel.Id,
                AnswerText = answerModel.AnswerText,
                Evaluation = answerModel.Evaluation,
                DoctorId = answerModel.DoctorId,
                PatientId = answerModel.PatientId,
                QuestionId = answerModel.QuestionId
            };
        }
        public async Task<decimal> GetAverageRating(decimal id)
        {
            List<AnswerDomainModel> answers = (List<AnswerDomainModel>) await GetForDoctor(id);
            if (answers.Count == 0) return -1;
            return answers.Select(x => x.Evaluation).Average();
        }

        public HospitalQuestionDTO RateHospital(HospitalQuestionDTO dto)
        {
            _ = _answerRepository.Post(parseFromModel(dto.answer1));
            _answerRepository.Save();
            _ = _answerRepository.Post(parseFromModel(dto.answer2));
            _answerRepository.Save();
            _ = _answerRepository.Post(parseFromModel(dto.answer3));
            _answerRepository.Save();
            _ = _answerRepository.Post(parseFromModel(dto.answer4));
            _answerRepository.Save();
            return dto;
        }

        public DoctorQuestionDTO RateDoctor(DoctorQuestionDTO dto)
        {
            _ = _answerRepository.Post(parseFromModel(dto.answer1));
            _answerRepository.Save();
            _ = _answerRepository.Post(parseFromModel(dto.answer2));
            _answerRepository.Save();
            return dto;
        }
    }
}
