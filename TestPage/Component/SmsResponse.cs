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
                SurveyInstance patientPhone = data.GetInstance(response.From);
                if (patientPhone != null)
                {
                    //
                    retVal = ProcessSmsText(patientPhone, response);
                }
            }
            return retVal;
        }

        static private string ProcessSmsText(SurveyInstance patientPhone, ResponseModel response)
        {
            string retVal = null;
            if (patientPhone != null )
            {
                if (patientPhone.CurrentQuestion == NOT_STARTED )
                {
                    retVal = PlayFirstQuestion(patientPhone, response);
                }
                else
                {
                    retVal = HandleQuestionResponse(patientPhone, response);
                }
            }

            return retVal;
        }

        static private string PlayFirstQuestion(SurveyInstance patientPhone, ResponseModel response)
        {
            string retVal = null;
            if (patientPhone != null )
            {
                if ( string.Compare( response.ResponseText, YES, true ) == 0 )
                {
                    patientPhone.fetchSurvey();
                    PatientSurveyQuestion nextQuestion = patientPhone.getQuestion(patientPhone.CurrentQuestion);

                    retVal = SurveyInstance.getFormattedQuestionText(nextQuestion);
                }
                else
                {
                    retVal = "Ok. We will try again later.";
                }
            }
            return retVal;
 
        }

        static private string HandleQuestionResponse(SurveyInstance patientPhone, ResponseModel response)
        {
            string retVal = null;
            if (patientPhone != null)
            {
                PatientSurveyQuestion nextQuestion = patientPhone.getQuestion(patientPhone.CurrentQuestion);
                if (nextQuestion != null)
                {
                    switch (nextQuestion.SurveyQuestionTypeId)
                    {
                        case 3: // SurveyQuestionTypeEnum.PulseOx
                            {
                                //HandlePusleOsResponse();
                                // parse the oxygen and heart rate
                                char[] delimitors = {' ', ','};
                                string[] parts = response.ResponseText.Split(delimitors);
                                if (parts.Length > 1 )
                                {
                                    string oxygen = parts[0];
                                    string heartRate = parts[1];
                                }
                                if ( valid() )
                                {
                                    // Save response to Vivify Portal
                                    retVal = PlayNextQuestion(patientPhone, response);
                                }
                                else
                                {
                                    // retVal = ReplaySameQuestion();
                                }
                                break;
                            }
                    }
                }
            }
            
            return retVal;
        }

        static private string PlayNextQuestion(SurveyInstance patientPhone, ResponseModel response)
        {
            string retVal = null;
            if (patientPhone != null)
            {
         //       patientPhone.CurrentQuestion++;
                PatientSurveyQuestion nextQuestion = patientPhone.getQuestion(patientPhone.CurrentQuestion);
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