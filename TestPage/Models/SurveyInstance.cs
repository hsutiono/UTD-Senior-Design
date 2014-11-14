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
        List<PatientSurveyModel> surveyData;

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
            surveyData = client.Execute<List<PatientSurveyModel>>(request).Data;
            if (sequenceType == 1)
            {
                CurrentQuestion = 0;
            }
            //need for other sequence types when available
        }
        public PatientSurveyQuestionModel getQuestion(int questionNumber)
        {
            PatientSurveyQuestionModel retVal = null;
            if (sequenceType == 2)
            {
                foreach (PatientSurveyModel properties in surveyData)
                {
                    foreach (PatientSurveyQuestionModel question in properties.PatientSurveyQuestions)
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
                List<PatientSurveyQuestionModel> results = new List<PatientSurveyQuestionModel>();
                foreach (PatientSurveyModel properties in surveyData)
                {
                    foreach (PatientSurveyQuestionModel question in properties.PatientSurveyQuestions)
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

        public static string getFormattedQuestionText(PatientSurveyQuestionModel patientQuestion)
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
                            retVal = patientQuestion.PatientSurveyQuestionTexts.First<PatientSurveyQuestionTextModel>().Text + "\n";
                            int i = 1;
                            if (patientQuestion.PatientSurveyOptions != null)
                            {
                                foreach (PatientSurveyOptionModel option in patientQuestion.PatientSurveyOptions)
                                {
                                    retVal += i + ": ";
                                    foreach (PatientSurveyOptionTextModel temp in option.PatientSurveyOptionTexts)
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

        public PatientResponseApiPostModel MakePostModelForResponseToCurrentQuestion()//will populate with appropriate PatientResponseValueApiPostModel which must either be edited or removed!
        {
            PatientResponseApiPostModel output = new PatientResponseApiPostModel();
            output.PatientId = userID;
            PatientSurveyQuestionModel currentQuestion = getQuestion(CurrentQuestion);
            output.PatientSurveyQuestionId = currentQuestion.PatientSurveyQuestionId;
            output.SurveyQuestionTypeId = currentQuestion.SurveyQuestionTypeId;
            output.PatientResponseInputMethodId = 1;
            output.ObservationDateTime_UTC = System.DateTime.Now;
            foreach(PatientSurveyOptionModel item in currentQuestion.PatientSurveyOptions)
            {
                PatientResponseValueApiPostModel valuemodel = new PatientResponseValueApiPostModel();
                valuemodel.PatientSurveyOptionId = item.PatientSurveyOptionId;
                valuemodel.SurveyParameterTypeId = item.SurveyParameterTypeId.GetValueOrDefault();
                output.PatientResponseValues.Add(valuemodel);
            }
            return output;
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
                else return "Text yes when ready.";
            }
            else
            {
                //check validity , create PatientResponseApiPostModel, send message to vivify here!
                if (sequenceType == 2)
                {
                }
                if (sequenceType == 1)
                {
                    CurrentQuestion++;
                    PatientSurveyQuestionModel current = getQuestion(CurrentQuestion);
                    if (current == null) return null;
                    return getFormattedQuestionText(current);
                }
            }
            return "N/A";
        }
    }
}