using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestPage.Models;
using TestPage.Component;

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
                    PatientSurveyQuestionModel nextQuestion = patientSurvey.getQuestion(patientSurvey.CurrentQuestion);

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
                PatientSurveyQuestionModel nextQuestion = patientSurvey.getQuestion(patientSurvey.CurrentQuestion);
                if (nextQuestion != null)
                {
                    switch ((SurveyQuestionType)nextQuestion.SurveyQuestionTypeId)
                    {
                        case SurveyQuestionType.BloodSugar:
                            {
                                success = HandleResponse.HandleBloodSugarResponse(patientSurvey, response);
                                break;
                            }
                        case SurveyQuestionType.BloodPressure:
                            {
                                success = HandleResponse.HandleBloodPressureResponse(patientSurvey, response);
                                break;
                            }
                        case SurveyQuestionType.PulseOx:
                            {
                                success = HandleResponse.HandlePulseOxResponse(patientSurvey, response);
                                break;
                            }
                        case SurveyQuestionType.Weight:
                            {
                                success = HandleResponse.HandleWeightResponse(patientSurvey, response);
                                break;
                            }
                        case SurveyQuestionType.Number:
                            {
                                success = HandleResponse.HandleNumberResponse(patientSurvey, response);
                                break;
                            }
                        case SurveyQuestionType.SingleSelection:
                            {
                                success = HandleResponse.HandleSingleSelectionResponse(patientSurvey, response);
                                break;
                            }
                        case SurveyQuestionType.MultiSelection:
                            {
                                success = HandleResponse.HandleMultiSelectionResponse(patientSurvey, response);
                                break;
                            }
                    }
                    if(success)
                    {
                        retVal = PlayNextQuestion(patientSurvey, response);
                    }
                    else
                    {
                        retVal = ReplayLastQuestion(patientSurvey, response);
                    }
                }
            }
            return retVal;
        }

        static private string PlayNextQuestion(SurveyInstance patientSurvey, ResponseModel response)
        {
            string retVal = null;
            if (patientSurvey != null)
            {
                patientSurvey.CurrentQuestion++;//eventually this will be replaced with actual data structure traversal!
                PatientSurveyQuestionModel nextQuestion = patientSurvey.getQuestion(patientSurvey.CurrentQuestion);
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
        static private string ReplayLastQuestion(SurveyInstance patientSurvey, ResponseModel response)
        {
            string retVal = null;
            if (patientSurvey != null)
            {
                //patientSurvey.CurrentQuestion++; //don't change value if replay
                PatientSurveyQuestionModel nextQuestion = patientSurvey.getQuestion(patientSurvey.CurrentQuestion);
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
    }
}