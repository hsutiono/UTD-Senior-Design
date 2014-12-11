using SMSClient.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMSClient.Components
{
    public class SmsResponse
    {
        private const string YES = "yes";
        private const string REJECT_MSG = "Ok. We will try again later. Send yes to continue";
        private const string RETRY_PREFIX = "Retry: ";
        private const string EXIT_MSG = "Thank you for completing your survey.";
        private const string CONTINUE_MSG = " Respond with anything to continue.";

        private const string PULSEOX_MSG = "Please enter your oxygen level and heart rate.";
        private const string BLOODPRESSURE_MSG = "Please enter your Systolic and Diastolic blood pressure.";
        private const string BLOODSUGAR_MSG = "Please enter your Blood Sugar Level.";
        private const string WEIGHT_MSG = "Please enter your weight.";

        public static string HandleSmsResponse(ResponseModel response, Dictionary<string, SurveyInstance> data)
        {
            string retVal = "";

            if ( response != null && response.From!=null && data.Keys.Contains(response.From))
            {
                SurveyInstance patientSurvey = data[response.From];
                if (patientSurvey != null)
                {
                    if (!patientSurvey.SurveyStarted())
                    {
                        retVal = PlayQuestion(patientSurvey, response);
                    }
                    else
                    {
                        bool success = HandleResponse.HandleQuestionResponse(patientSurvey, response) != null;
                        if (success)
                        {
                            retVal = PlayQuestion(patientSurvey, response);
                            if(retVal.Equals(EXIT_MSG))
                            {
                                data.Remove(response.From);
                            }
                            else
                            {
                                if (QuestionIsOptionless(patientSurvey.GetCurrentQuestion()))
                                {
                                    retVal += CONTINUE_MSG;
                                    patientSurvey.NextQuestion(null);
                                }
                            }
                        }
                        else
                        {
                            retVal = PlayQuestion(patientSurvey, response);
                            if (retVal.Equals(EXIT_MSG))
                            {
                                data.Remove(response.From);
                            }
                            else
                            {
                                retVal = RETRY_PREFIX + retVal;
                            }

                        }
                    }
                }
            }
            return retVal;
        }
        private static string PlayQuestion(SurveyInstance patientSurvey, ResponseModel response)
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
            if(currentQuestion!=null && currentQuestion.PatientSurveyOptions!=null)
            {
                return currentQuestion.PatientSurveyOptions.Count == 0;
            }
            return false;
        }
        private static string getFormattedQuestionText(PatientSurveyQuestionModel patientQuestion)
        {
            string retVal = null;
            if (patientQuestion != null)
            {
                switch (patientQuestion.SurveyQuestionTypeId)
                {
                    case (int)SurveyQuestionType.PulseOx: // SurveyQuestionTypeEnum.PulseOx
                        {
                            retVal = PULSEOX_MSG;
                            break;
                        }
                    case (int)SurveyQuestionType.BloodPressure: // SurveyQuestionTypeEnum.BloodPressure
                        {
                            retVal = BLOODPRESSURE_MSG;
                            break;
                        }
                    case (int)SurveyQuestionType.BloodSugar:
                        {
                            retVal = BLOODSUGAR_MSG;
                            break;
                        }
                    case (int)SurveyQuestionType.Weight:
                        {
                            retVal = WEIGHT_MSG;
                            break;
                        }
                    /*case (int)SurveyQuestionType.SingleSelection:
                        {

                        }
                    case (int)SurveyQuestionType.MultiSelection:
                        {

                        }*/
                    default: // This generates text for Selection type survey questions.
                        {
                            StringBuilder b = new StringBuilder();
                            foreach(PatientSurveyQuestionTextModel item in patientQuestion.PatientSurveyQuestionTexts)
                            {
                                b.Append(item.Text);
                                b.Append(' ');
                            }
                            b.Append('\n');
                            int i = 1;
                            if (patientQuestion.PatientSurveyOptions != null)
                            {
                                foreach (PatientSurveyOptionModel option in patientQuestion.PatientSurveyOptions)
                                {
                                    b.Append(i);
                                    b.Append(':');
                                    b.Append(' ');
                                    foreach (PatientSurveyOptionTextModel temp in option.PatientSurveyOptionTexts)
                                    {
                                        b.Append(temp.Text);
                                        b.Append(' ');
                                    }
                                    b.Append('\n');
                                    i++;
                                }
                            }
                            retVal = b.ToString();
                            break;
                        }
                }
            }
            return retVal;
        }

    }
}