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

namespace SMSClient.Models
{
    public class SequentialSurveyInstance
    {

        public int CurrentQuestion { get; set; }
        private static string VivifySurveyAuthtoken = "26881576-3F9B-4F97-B7F7-91532DE1586A";
        private static string AccountSid = "AC7a6db27538ba8ed863c14e825beb35f4";
        private static string AuthToken = "56cb022777274d5e98fdeed9523987a7";
        private static TwilioRestClient twilio = new TwilioRestClient(AccountSid, AuthToken);
        private static string server = "+17743077070";
        public int userID;
        public string phone;
        //string emergencyContact;
        
        //public DateTime previousContact;
        List<PatientSurveyModel> surveyData;
        PatientSurveyQuestionModel CurrentQuestionModel;

        public SequentialSurveyInstance(int userID, string phone)//:base(userID,phone)
        {
            this.userID = userID;
            this.phone = phone;
            CurrentQuestion = -1;//-1 means prep stage.
            surveyData = null;
            //previousContact = DateTime.Now;
            var send = twilio.SendMessage(server, phone, "Are you ready to take your survey?");
        }

        protected void fetchData()//gets survey AND sets first currentQuestion value
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var client = new RestClient("https://demoutdesign.dev.vivifyhealth.com/api/PatientSurvey");
            var request = new RestRequest(Method.GET);
            request.AddParameter("authtoken", VivifySurveyAuthtoken, ParameterType.QueryString);
            request.AddParameter("PatientId", userID.ToString(), ParameterType.QueryString);
            surveyData = client.Execute<List<PatientSurveyModel>>(request).Data;
            CurrentQuestion = 0;
        }
        public bool NextQuestion()//increments current question value
        {
            CurrentQuestion++;
            return CurrentQuestion < surveyData.Count;
        }
        public PatientSurveyQuestionModel GetCurrentQuestion()
        {
            return GetQuestion(CurrentQuestion);
        }
        private PatientSurveyQuestionModel GetQuestion(int questionNumber)
        {
            PatientSurveyQuestionModel retVal = null;
            List<PatientSurveyQuestionModel> results = new List<PatientSurveyQuestionModel>();
            foreach (PatientSurveyModel properties in surveyData)
            {
                foreach (PatientSurveyQuestionModel question in properties.PatientSurveyQuestions)
                {
                    results.Add(question);
                }
            }
            if (questionNumber < results.Count)
            {
                retVal = results[questionNumber];
            }
            return retVal;
        }
    }
}