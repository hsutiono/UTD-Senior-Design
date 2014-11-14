using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestPage.Models;

namespace TestPage.Component
{
    public class SmsResponse
    {
        const string YES = "yes";
        const int NOT_STARTED = -1;

        static public string HandleSmsResponse( ResponseModel response, ActiveRegistry data )
        {
            string retVal = null;

            if ( response != null )
            {
                SurveyInstance patientSurvey = data.GetInstance(response.From);
                if (patientSurvey != null)
                {
                    //
                    retVal = ProcessSmsText(patientSurvey, response);
                }
            }
            return retVal;
        }

        static private string ProcessSmsText(SurveyInstance patientSurvey, ResponseModel response)
        {
            string retVal = null;
            if (patientSurvey != null )
            {
                if (patientSurvey.CurrentQuestion == NOT_STARTED )
                {
                    retVal = PlayFirstQuestion(patientSurvey, response);
                }
                else
                {
                    retVal = HandleQuestionResponse(patientSurvey, response);
                }
            }

            return retVal;
        }

        static private string PlayFirstQuestion(SurveyInstance patientSurvey, ResponseModel response)
        {
            string retVal = null;
            if (patientSurvey != null )
            {
                if ( string.Compare( response.ResponseText, YES, true ) == 0 )
                {
                    patientSurvey.fetchSurvey();
                    PatientSurveyQuestion nextQuestion = patientSurvey.getQuestion(patientSurvey.CurrentQuestion);

                    retVal = SurveyInstance.getFormattedQuestionText(nextQuestion);
                }
                else
                {
                    retVal = "Ok. We will try again later.";
                }
            }
            return retVal;
 
        }

        static private string HandleQuestionResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string retVal = null;
            bool success = false;
            if (patientSurvey != null)
            {
                PatientSurveyQuestion nextQuestion = patientSurvey.getQuestion(patientSurvey.CurrentQuestion);
                if (nextQuestion != null)
                {
                    switch ((SurveyQuestionType)nextQuestion.SurveyQuestionTypeId)
                    {
                        case SurveyQuestionType.BloodSugar:
                            {
                                success = HandleBloodSugarResponse(patientSurvey, response);
                                break;
                            }
                        case SurveyQuestionType.BloodPressure:
                            {
                                success = HandleBloodPressureResponse(patientSurvey, response);
                                break;
                            }
                        case SurveyQuestionType.PulseOx:
                            {
                                success = HandlePulseOxResponse(patientSurvey, response);
                                break;
                            }
                        case SurveyQuestionType.Weight:
                            {
                                success = HandleWeightResponse(patientSurvey, response);
                                break;
                            }
                        case SurveyQuestionType.Number:
                            {
                                success = HandleNumberResponse(patientSurvey, response);
                                break;
                            }
                        case SurveyQuestionType.SingleSelection:
                            {
                                success = HandleSingleSelectionResponse(patientSurvey, response);
                                break;
                            }
                        case SurveyQuestionType.MultiSelection:
                            {
                                success = HandleMultiSelectionResponse(patientSurvey, response);
                                break;
                            }
                    }
                    if(success)
                    {
                        retVal = PlayNextQuestion(patientSurvey, response);
                    }
                    else
                    {
                        //replay last question
                    }
                }
            }

            
            
            return retVal;
        }
        private static bool HandleMultiSelectionResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }

        private static bool HandleSingleSelectionResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }

        private static bool HandleNumberResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }

        static private bool HandlePulseOxResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            //HandlePusleOsResponse();
            // parse the oxygen and heart rate
            char[] delimitors = { ' ', ',' };
            string[] parts = response.ResponseText.Split(delimitors);
            if (parts.Length > 1)
            {
                string oxygen = parts[0];
                string heartRate = parts[1];
            }
            bool isValid = valid();
            //send back to vivify here
            return isValid;
        }

        private static bool HandleWeightResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }

        private static bool HandleBloodPressureResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }

        private static bool HandleBloodSugarResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }

        static private string PlayNextQuestion(SurveyInstance patientSurvey, ResponseModel response)
        {
            string retVal = null;
            if (patientSurvey != null)
            {
                patientSurvey.CurrentQuestion++;
                PatientSurveyQuestion nextQuestion = patientSurvey.getQuestion(patientSurvey.CurrentQuestion);
                if (nextQuestion != null)
                {
                    retVal = SurveyInstance.getFormattedQuestionText(nextQuestion);
                }
                else
                {
                    retVal = "Thank you for completing your survey.";
                }
             }
            return retVal;
        }
 
        static private bool valid()
        {
            return true;
        }
    }
}