using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using SMSClient.Models;
using System.Web.Mvc.Html;
using RestSharp;
using System.Net;
using Twilio.Mvc;
using Twilio.TwiML;
using SMSClient.Components;
using SMSClient.Integration;

namespace SMSClient.Models
{
    public class StackSurveyInstance
    {
        private bool surveyStarted;
        public int userID;
        public string phone;
        PatientModel patient = null;
        Stack<PatientSurveyQuestionModel> ActiveQuestion = new Stack<PatientSurveyQuestionModel>();

        public StackSurveyInstance(string _phone, int? _userID = null )
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
            return ActiveQuestion.First();
        }

        public void StartSurvey()
        {
            if(ActiveQuestion.Count==0)
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
            if (ActiveQuestion.Count == 0)
            {
                //get question model.
                ActiveQuestion.Push(PatientComponent.GetNextSurveyQuestion(GetPatient().Id, -1, null));// what is a good null value here?
            }
        }
        public bool NextQuestion(List<int?> currentPatientSurveyOptionIds)
        {
            List<PatientSurveyOptionModel> optionsselected = new List<PatientSurveyOptionModel>();
            foreach(PatientSurveyOptionModel item in GetCurrentQuestion().PatientSurveyOptions)
            {
                if(currentPatientSurveyOptionIds.Contains(item.PatientSurveyOptionId))
                {
                    optionsselected.Add(item);
                }
            }
            if(optionsselected.Count==0)
            {
                ActiveQuestion.Pop();
                if(ActiveQuestion.Count<=0)
                    ActiveQuestion.Push(PatientComponent.GetNextSurveyQuestion(GetPatient().Id, GetCurrentQuestion().PatientSurveyQuestionId, currentPatientSurveyOptionIds));
            }
            else
            {
                ActiveQuestion.Pop();
                if (ActiveQuestion.Count <= 0)
                    ActiveQuestion.Push(PatientComponent.GetNextSurveyQuestion(GetPatient().Id, GetCurrentQuestion().PatientSurveyQuestionId, currentPatientSurveyOptionIds));
            }
            return ActiveQuestion.Count!=0;
        }
    }
}