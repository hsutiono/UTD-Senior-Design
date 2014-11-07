using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using TestPage.Models;
using System.Web.Mvc.Html;
using RestSharp;
using System.Net;
using Twilio.Mvc;
using Twilio.TwiML;

namespace TestPage.Models
{
    public class SurveyInstance
    {
        public static int sequenceType = 1;//1 sorts by sent order. 2 by question number order.
        public static string AccountSid = "AC7a6db27538ba8ed863c14e825beb35f4";
        public static string AuthToken = "56cb022777274d5e98fdeed9523987a7";
        public static string server = "+17743077070";
        public int userID;
        public string phone;
        //string emergencyContact;
        public int currentQuestion;
        List<PatientSurvey> surveyData;
        public SurveyInstance(int userID,string phone)
        {
            this.userID = userID;
            this.phone = phone;
            currentQuestion = -1;//-1 means prep stage.
            surveyData = null;

            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            var send = twilio.SendMessage(server, phone, "Are you ready to take your survey?");
        }
        public bool surveyFetched()
        {
            return surveyData == null;
        }
        public void fetchSurvey()//gets survey AND sets first currentQuestion value
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var client = new RestClient("https://demoutdesign.dev.vivifyhealth.com/api/PatientSurvey");
            var request = new RestRequest(Method.GET);
            request.AddParameter("authtoken", "26881576-3F9B-4F97-B7F7-91532DE1586A", ParameterType.QueryString);
            request.AddParameter("PatientId", userID.ToString(), ParameterType.QueryString);
            surveyData = client.Execute<List<PatientSurvey>>(request).Data;
            if(sequenceType == 1)currentQuestion = 0;
            //need for other sequence types when available
        }
        public PatientSurveyQuestion getQuestion(int questionNumber)
        {
            if (sequenceType == 2)
            {
                foreach (PatientSurvey properties in surveyData)
                {
                    foreach (PatientSurveyQuestion question in properties.PatientSurveyQuestions)
                    {
                        if (question.PatientSurveyQuestionId == questionNumber) return question;
                    }
                }
            }
            if (sequenceType == 1)
            {
                List<PatientSurveyQuestion> results = new List<PatientSurveyQuestion>();
                foreach (PatientSurvey properties in surveyData)
                {
                    foreach (PatientSurveyQuestion question in properties.PatientSurveyQuestions)
                    {
                        results.Add(question);
                    }
                } return results[questionNumber];
            }
            return null;
        }
        /*public PatientSurveyQuestion nextQuestion(PatientSurveyQuestion currentQuestion, string response)
        { }*/
        public string response(string message)
        {
            if(currentQuestion<0)
            {
                if(message.Equals("yes"))
                {
                    fetchSurvey();
                    return getQuestion(currentQuestion).PatientSurveyQuestionTexts.First<PatientSurveyQuestionText>().Text;
                }
                else return "";
            }
            else
            {
                //check validity , create PatientResponse, send message to vivify here!
                if (sequenceType == 2)
                {
                }
                if (sequenceType == 1)
                {
                    currentQuestion++;
                    PatientSurveyQuestion current = getQuestion(currentQuestion);
                    if(current == null)return null;
                    return current.PatientSurveyQuestionTexts.First<PatientSurveyQuestionText>().Text;
                }
            }
            return "";
        }
    }
}