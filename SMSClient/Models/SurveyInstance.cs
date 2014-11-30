﻿using System;
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
    public class SurveyInstance
    {
        private bool surveyStarted;
        public int userID;
        public string phone;
        PatientModel patient = null;
        PatientSurveyQuestionModel CurrentQuestionModel = null;

        public SurveyInstance(string _phone, int? _userID = null )
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