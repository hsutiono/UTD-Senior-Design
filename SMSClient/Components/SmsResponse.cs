using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMSClient.Models;
using SMSClient.Components;

namespace SMSClient.Components
{
    public class SmsResponse
    {
        private const string YES = "yes";
        private const string REJECT_MSG = "Ok. We will try again later. Send yes to continue";
        private const string RETRY_PREFIX = "Retry: ";
        private const string EXIT_MSG = "Thank you for completing your survey.";

        static public string HandleSmsResponse(ResponseModel response, Dictionary<string, SurveyInstance> data)
        {
            string retVal = "";

            if ( response != null && response.From!=null && data.Keys.Contains(response.From))
            {
                SurveyInstance patientSurvey = data[response.From];
                if (patientSurvey != null)
                {
                    if (!patientSurvey.SurveyStarted() || QuestionIsOptionless(patientSurvey.GetCurrentQuestion()))
                    {
                        retVal = PlayQuestion(patientSurvey, response);
                    }
                    else
                    {
                        bool success = HandleResponse.HandleQuestionResponse(patientSurvey, response) != null;
                        if (success)
                        {
                            retVal = PlayQuestion(patientSurvey, response);
                        }
                        else
                        {
                            retVal = RETRY_PREFIX + PlayQuestion(patientSurvey, response);
                        }
                    }
                    if (retVal.Equals(EXIT_MSG))
                    {
                        data.Remove(response.From);
                    }
                }
            }
            return retVal;
        }

        static private string PlayQuestion(SurveyInstance patientSurvey, ResponseModel response)
        {
            string retVal = null;
            if (patientSurvey != null)
            {
                PatientSurveyQuestionModel question = patientSurvey.GetCurrentQuestion();
                if(!patientSurvey.SurveyStarted())
                {
                    if (string.Compare(response.ResponseText, YES, true) == 0)
                    {
                        patientSurvey.StartSurvey();
                        retVal = getFormattedQuestionText(question);
                    }
                    else
                    {
                        retVal = REJECT_MSG;
                    }
                }
                else
                {
                        
                    if (question != null)
                    {
                        retVal = getFormattedQuestionText(question);
                    }
                    else
                    {
                        retVal = EXIT_MSG;
                    }

                }
            }
            return retVal;
        }

        private static bool QuestionIsOptionless(PatientSurveyQuestionModel currentQuestion)
        {
            return currentQuestion.PatientSurveyOptions.Count == 0;
        }
        private static string getFormattedQuestionText(PatientSurveyQuestionModel patientQuestion)
        {
            string retVal = "";
            if (patientQuestion != null)
            {
                switch (patientQuestion.SurveyQuestionTypeId)
                {
                    case (int)SurveyQuestionType.PulseOx: // SurveyQuestionTypeEnum.PulseOx
                        {
                            retVal = "Please enter your oxygen level and heart rate.";
                            break;
                        }
                    case (int)SurveyQuestionType.BloodPressure: // SurveyQuestionTypeEnum.BloodPressure
                        {
                            retVal = "Please enter your blood pressure. Systolic,Diastolic.";
                            break;
                        }
                    case (int)SurveyQuestionType.BloodSugar:
                        {
                            retVal = "Please enter your Blood Sugar Level.";
                            break;
                        }
                    case (int)SurveyQuestionType.Weight:
                        {
                            retVal = "Please enter your weight.";
                            break;
                        }
                    /*case (int)SurveyQuestionType.SingleSelection:
                        {

                        }
                    case (int)SurveyQuestionType.MultiSelection:
                        {

                        }*/
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

    }
}