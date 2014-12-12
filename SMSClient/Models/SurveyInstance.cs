using SMSClient.Components;
using SMSClient.Integration;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SMSClient.Models
{
    [Serializable()]
    public class SurveyInstance
    {
        private const string Greeting = "Are you ready to take your survey?";
        private bool surveyStarted;
        private int userID;
        private string phone;
        private PatientModel patient = null;
        private Stack<PatientSurveyQuestionModel> ActiveQuestion = new Stack<PatientSurveyQuestionModel>();

        public SurveyInstance(string _phone, int? _userID = null )
        {
            this.phone = _phone;
            TwilioService firstSend = new TwilioService();
            firstSend.SendMessage(phone, Greeting);
            if (_userID != null)
            {
                this.userID = _userID.GetValueOrDefault();
            }
            else
            {
                patient = PatientComponent.GetPatientByPhoneNumber(phone);
                this.userID = patient.Id;
            }
            surveyStarted = false;
            fetchData();
        }
        public PatientModel GetPatient()
        {
            return patient;
        }
        public PatientSurveyQuestionModel GetCurrentQuestion()
        {
            if (ActiveQuestion.Count == 0)
            {
                return null;
            }
            else
            {
                return ActiveQuestion.Peek();
            }
        }
        public void StartSurvey()
        {
            if(ActiveQuestion.Count==0 && !surveyStarted)
            {
                fetchData();
            }
            surveyStarted = true;
        }
        public bool SurveyStarted()
        {
            return surveyStarted;
        }
        protected void fetchData()//gets survey
        {
            if(patient == null)
            {
                patient = PatientComponent.GetPatient(userID);//get patient data
            }
            if (ActiveQuestion.Count == 0 && !surveyStarted)
            {
                List<PatientSurveyQuestionModel> questions = PatientComponent.GetPatientSurveyQuestionsForToday(GetPatient().Id).ToList();
                for(int i = questions.Count-1;i>=0;i--)
                {
                    ActiveQuestion.Push(questions[i]);//push them to stack in reverse order.
                }
            }
        }
        public bool NextQuestion(List<int?> currentPatientSurveyOptionIds)
        {
            if(ActiveQuestion.Count==0)
            {
                return false;
            }
            List<PatientSurveyOptionModel> optionsselected = new List<PatientSurveyOptionModel>();
            if (currentPatientSurveyOptionIds != null)
            {
                foreach (PatientSurveyOptionModel item in GetCurrentQuestion().PatientSurveyOptions)
                {
                    if (currentPatientSurveyOptionIds.Contains(item.PatientSurveyOptionId))
                    {
                        optionsselected.Add(item);
                    }
                }
            }
            if(optionsselected.Count==0)
            {
                ActiveQuestion.Pop();
            }
            else
            {
                PatientSurveyQuestionModel prevquestion = ActiveQuestion.Pop();
                foreach(PatientSurveyOptionModel item in optionsselected)
                {
                    foreach(PatientSurveyQuestionModel jtem in item.PatientSurveyQuestions)
                    {
                        ActiveQuestion.Push(jtem);
                    }
                }
            }
            return ActiveQuestion.Count!=0;
        }
    }
}