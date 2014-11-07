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

        public int CurrentQuestion { get; set; }
        
        // prviate below this
        public static int sequenceType = 1;//1 sorts by sent order. 2 by question number order.

        public static string AccountSid = "AC7a6db27538ba8ed863c14e825beb35f4";
        public static string AuthToken = "56cb022777274d5e98fdeed9523987a7";
        public static string server = "+17743077070";
        public int userID;
        public string phone;
        //string emergencyContact;
        
        public DateTime previousContact;
        List<PatientSurvey> surveyData;

        public SurveyInstance(int userID, string phone)
        {
            this.userID = userID;
            this.phone = phone;
            CurrentQuestion = -1;//-1 means prep stage.
            surveyData = null;
            previousContact = DateTime.Now;

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
            if (sequenceType == 1)
            {
                CurrentQuestion = 0;
            }
            //need for other sequence types when available
        }
        public PatientSurveyQuestion getQuestion(int questionNumber)
        {
            PatientSurveyQuestion retVal = null;
            if (sequenceType == 2)
            {
                foreach (PatientSurvey properties in surveyData)
                {
                    foreach (PatientSurveyQuestion question in properties.PatientSurveyQuestions)
                    {
                        if (question.PatientSurveyQuestionId == questionNumber)
                        {
                            retVal = question;
                            break;
                        }
                    }
                    if ( retVal != null )
                    {
                        break;
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
                } 
                retVal = results[questionNumber];
            }
            return retVal;
        }
        /*public PatientSurveyQuestion nextQuestion(PatientSurveyQuestion currentQuestion, string response)
        { }*/
        public static string getFormattedQuestionText(PatientSurveyQuestion patientQuestion)
        {
            string retVal = "";
            if ( patientQuestion != null )
            {
                switch (  patientQuestion.SurveyQuestionTypeId )
                {
                    case 3: // SurveyQuestionTypeEnum.PulseOx
                        {
                            retVal = "Please enter your oxygen level and heart rate.";
                            break;
                        }
                    case 2: // SurveyQuestionTypeEnum.BloodPressure
                        {
                            retVal = "Please enter your blood pressure. Systolic/Diastolic.";
                            break;
                        }
                    default:
                        {
                            retVal = patientQuestion.PatientSurveyQuestionTexts.First<PatientSurveyQuestionText>().Text + "\n";
                            int i = 1;
                            if (patientQuestion.PatientSurveyOptions != null)
                            {
                                foreach (PatientSurveyOption option in patientQuestion.PatientSurveyOptions)
                                {
                                    retVal += i + ": ";
                                    foreach (PatientSurveyOptionText temp in option.PatientSurveyOptionTexts)
                                    {
                                        retVal += temp.Text + " ";
                                    }
                                    retVal += "\n";
                                    i++;
                                }
                            }
                            break;
                        }
                }
            }
            return retVal;
        }
        public string response(string message)
        {
            DateTime prevprevContact = previousContact;
            previousContact = DateTime.Now;
            message = message.ToLower();//
            if (message.Equals("show"))//debugging
            {
                return "User ID:" + userID + " Phone:" + phone + " Current Question:" + CurrentQuestion;
            }
            if (CurrentQuestion < 0)
            {
                if (message.Equals("yes"))
                {
                    fetchSurvey();
                    return getFormattedQuestionText(getQuestion(CurrentQuestion));
                }
                else return "Say yes when ready.";
            }
            else
            {
                //check validity , create PatientResponse, send message to vivify here!
                if (sequenceType == 2)
                {
                }
                if (sequenceType == 1)
                {
                    CurrentQuestion++;
                    PatientSurveyQuestion current = getQuestion(CurrentQuestion);
                    if (current == null) return null;
                    return getFormattedQuestionText(current);
                }
            }
            return "N/A";
        }
    }
}