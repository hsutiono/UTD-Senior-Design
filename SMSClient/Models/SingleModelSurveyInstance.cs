using SMSClient.Components;
using SMSClient.Integration;
using System.Collections.Generic;
using System.Linq;

namespace SMSClient.Models
{
    public class SingleModelSurveyInstance
    {//Keep this in case PatientComponent.GetNextSurveyQuestion is preferable.
        private bool surveyStarted;
        private int userID;
        private string phone;
        private PatientModel patient = null;
        private PatientSurveyQuestionModel CurrentQuestionModel = null;

        public SingleModelSurveyInstance(string _phone, int? _userID = null )
        {
            this.phone = _phone;
            TwilioService firstSend = new TwilioService();
            firstSend.SendMessage(phone, "Are you ready to take your survey?");
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
            return CurrentQuestionModel;
        }
        public void StartSurvey()
        {
            if(CurrentQuestionModel==null)
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
                //get patient data
                patient = PatientComponent.GetPatient(userID);
            }
            if(CurrentQuestionModel == null)
            {
                //get question model.
                CurrentQuestionModel = PatientComponent.GetNextSurveyQuestion(GetPatient().Id, -1, null);// what is a good null value here?
            }
        }
        public bool NextQuestion(List<int?> currentPatientSurveyOptionIds)
        {
            PatientSurveyQuestionModel nextbasequestion = PatientComponent.GetNextSurveyQuestion(GetPatient().Id, GetCurrentQuestion().PatientSurveyQuestionId, currentPatientSurveyOptionIds);
            List<PatientSurveyOptionModel> optionsselected = new List<PatientSurveyOptionModel>();
            foreach(PatientSurveyOptionModel item in CurrentQuestionModel.PatientSurveyOptions)
            {
                if(currentPatientSurveyOptionIds.Contains(item.PatientSurveyOptionId))
                {
                    optionsselected.Add(item);
                }
            }
            if(optionsselected.Count!=0)
            {
                CurrentQuestionModel = optionsselected.FirstOrDefault().PatientSurveyQuestions.FirstOrDefault();
            }
            else
            {
                CurrentQuestionModel = nextbasequestion;
            }
            if(CurrentQuestionModel==null)
            {
                CurrentQuestionModel = nextbasequestion;
            }
            return CurrentQuestionModel!=null;
        }
    }
}