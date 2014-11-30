using SMSClient.Integration;
using SMSClient.Models;
using System;
using System.Collections.Generic;

namespace SMSClient.Components
{
    public class HandleResponse // if the return is null, then input invalid
    {
        public static PatientResponseApiPostModel HandleQuestionResponse(SurveyInstance patientSurvey, ResponseModel response) //returns response from the server on success and null on failure.
        {
            PatientResponseApiPostModel serverresponse = null;
            if (patientSurvey != null)
            {
                PatientSurveyQuestionModel nextQuestion = patientSurvey.GetCurrentQuestion();
                if (nextQuestion != null)
                {
                    try
                    {
                        switch ((SurveyQuestionType)nextQuestion.SurveyQuestionTypeId)
                        {
                            case SurveyQuestionType.BloodSugar:
                                {
                                    serverresponse = HandleBloodSugarResponse(patientSurvey, response);
                                    break;
                                }
                            case SurveyQuestionType.BloodPressure:
                                {
                                    serverresponse = HandleBloodPressureResponse(patientSurvey, response);
                                    break;
                                }
                            case SurveyQuestionType.PulseOx:
                                {
                                    serverresponse = HandlePulseOxResponse(patientSurvey, response);
                                    break;
                                }
                            case SurveyQuestionType.Weight:
                                {
                                    serverresponse = HandleWeightResponse(patientSurvey, response);
                                    break;
                                }
                            case SurveyQuestionType.Number:
                                {
                                    serverresponse = HandleNumberResponse(patientSurvey, response);
                                    break;
                                }
                            case SurveyQuestionType.SingleSelection:
                                {
                                    serverresponse = HandleSingleSelectionResponse(patientSurvey, response);
                                    break;
                                }
                            case SurveyQuestionType.MultiSelection:
                                {
                                    serverresponse = HandleMultiSelectionResponse(patientSurvey, response);
                                    break;
                                }
                            default:
                                {
                                    serverresponse = new PatientResponseApiPostModel(); // do nothing
                                    break;
                                }
                        }
                    }
                    catch (Integration.HttpResponseException e)
                    {
                        Console.WriteLine(e.StackTrace);
                        serverresponse = new PatientResponseApiPostModel();//ignore case
                    };
                }
            }
            return serverresponse;
        }
        private static PatientResponseApiPostModel HandleMultiSelectionResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            char[] delimitors = { ' ', ',' };
            string[] parts = response.ResponseText.Split(delimitors);

            bool isValid = SmsValidation.validMultipleSelection(patientSurvey, patientSurvey.GetCurrentQuestion().PatientSurveyQuestionId, response.ResponseText);
            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = MakePostModelForResponseToQuestion(patientSurvey.GetCurrentQuestion(), patientSurvey.GetPatient());
                foreach(string item in parts)
                {
                    PatientResponseValueApiPostModel msResponse = new PatientResponseValueApiPostModel();
                    msResponse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Survey;
                    int value = FetchOptionCodeForResponseValue(patientSurvey.GetCurrentQuestion(),item);
                    if (value != -1)
                    {
                        msResponse.PatientSurveyOptionId = value;
                        patientResponse.PatientResponseValues.Add(msResponse);
                    }
                }

                IvrService vivifyService = new IvrService();
                patientSurvey.NextQuestion(MakePatientSelectionDescriptorFromPostModel(patientResponse));
                return vivifyService.PostPatientResponse(patientResponse);
            }
            return null;
        }
        private static PatientResponseApiPostModel HandleSingleSelectionResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string singleResponse = response.ResponseText;

            bool isValid = SmsValidation.validSingleSelection(patientSurvey, patientSurvey.GetCurrentQuestion().PatientSurveyQuestionId, singleResponse);
            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = MakePostModelForResponseToQuestion(patientSurvey.GetCurrentQuestion(), patientSurvey.GetPatient());
                PatientResponseValueApiPostModel singleResponseModel = new PatientResponseValueApiPostModel();
                singleResponseModel.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Survey;
                singleResponseModel.PatientSurveyOptionId =FetchOptionCodeForResponseValue(patientSurvey.GetCurrentQuestion(),singleResponse);
                patientResponse.PatientResponseValues.Add(singleResponseModel);

                IvrService vivifyService = new IvrService();
                patientSurvey.NextQuestion(MakePatientSelectionDescriptorFromPostModel(patientResponse));//Advance Question to the next question now, based on the patient response
                return vivifyService.PostPatientResponse(patientResponse);
            }

            return null;
        }
        private static PatientResponseApiPostModel HandleNumberResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string number = response.ResponseText;

            bool isValid = number!=null;

            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = MakePostModelForResponseToQuestion(patientSurvey.GetCurrentQuestion(), patientSurvey.GetPatient());

                PatientResponseValueApiPostModel NumberResponse = new PatientResponseValueApiPostModel();
                NumberResponse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.ReadingType;//not sure about this
                NumberResponse.Value = number;
                patientResponse.PatientResponseValues.Add(NumberResponse);

                IvrService vivifyService = new IvrService();
                patientSurvey.NextQuestion(MakePatientSelectionDescriptorFromPostModel(patientResponse));//Advance Question to the next question now, based on the patient response
                return vivifyService.PostPatientResponse(patientResponse);
            }

            return null;
        }
        private static PatientResponseApiPostModel HandlePulseOxResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            char[] delimitors = { ' ', ',' };
            string[] parts = response.ResponseText.Split(delimitors);

            string oxygen = null;
            string heartRate = null; 

            if (parts.Length > 1)
            {
                oxygen = parts[0];
                heartRate = parts[1];
            }
            bool isValid = SmsValidation.validPulseOxHeartRate(oxygen, heartRate);

            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = MakePostModelForResponseToQuestion(patientSurvey.GetCurrentQuestion(), patientSurvey.GetPatient());

                // Add Heart Rate Value
                PatientResponseValueApiPostModel hearRateResonse = new PatientResponseValueApiPostModel();
                hearRateResonse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Pulse; // heart rate
                hearRateResonse.Value = heartRate;
                patientResponse.PatientResponseValues.Add(hearRateResonse);

                // Add Oxygen Value
                PatientResponseValueApiPostModel OxygenResonse = new PatientResponseValueApiPostModel();
                OxygenResonse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Oxygen; // Oxygen
                OxygenResonse.Value = oxygen;
                patientResponse.PatientResponseValues.Add(OxygenResonse);

                IvrService vivifyService = new IvrService();
                patientSurvey.NextQuestion(MakePatientSelectionDescriptorFromPostModel(patientResponse));//Advance Question to the next question now, based on the patient response
                return vivifyService.PostPatientResponse(patientResponse);
            }

            return null;
        }
        private static PatientResponseApiPostModel HandleWeightResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string weight = response.ResponseText;

            bool isValid = SmsValidation.validWeightResponse(weight);

            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = MakePostModelForResponseToQuestion(patientSurvey.GetCurrentQuestion(), patientSurvey.GetPatient());

                PatientResponseValueApiPostModel WeightResponse = new PatientResponseValueApiPostModel();
                WeightResponse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Weight;
                WeightResponse.Value = weight;
                patientResponse.PatientResponseValues.Add(WeightResponse);

                IvrService vivifyService = new IvrService();
                patientSurvey.NextQuestion(MakePatientSelectionDescriptorFromPostModel(patientResponse));//Advance Question to the next question now, based on the patient response
                return vivifyService.PostPatientResponse(patientResponse);
            }

            return null;
        }
        private static PatientResponseApiPostModel HandleBloodPressureResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            char[] delimitors = { ' ', ',' };
            string[] parts = response.ResponseText.Split(delimitors);

            string systole = null;
            string diastole = null;

            if (parts.Length > 1)
            {
                systole = parts[0];
                diastole = parts[1];
            }
            bool isValid = SmsValidation.validBloodPressure(systole, diastole);

            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = MakePostModelForResponseToQuestion(patientSurvey.GetCurrentQuestion(), patientSurvey.GetPatient());

                // Add systolic
                PatientResponseValueApiPostModel systolicResponse = new PatientResponseValueApiPostModel();
                systolicResponse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Systolic; // 
                systolicResponse.Value = diastole;
                patientResponse.PatientResponseValues.Add(systolicResponse);

                // Add diastolic
                PatientResponseValueApiPostModel diastolicResponse = new PatientResponseValueApiPostModel();
                diastolicResponse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Diastolic; // 
                diastolicResponse.Value = systole;
                patientResponse.PatientResponseValues.Add(diastolicResponse);

                IvrService vivifyService = new IvrService();
                patientSurvey.NextQuestion(MakePatientSelectionDescriptorFromPostModel(patientResponse));//Advance Question to the next question now, based on the patient response
                return vivifyService.PostPatientResponse(patientResponse);
            }

            return null;
        }
        private static PatientResponseApiPostModel HandleBloodSugarResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string bloodsugarResponse = response.ResponseText;

            bool isValid = SmsValidation.validBloodSugarResponse(bloodsugarResponse);

            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = MakePostModelForResponseToQuestion(patientSurvey.GetCurrentQuestion(), patientSurvey.GetPatient());

                PatientResponseValueApiPostModel BloodSugarResponse = new PatientResponseValueApiPostModel();
                BloodSugarResponse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.BloodSugar;
                BloodSugarResponse.Value = bloodsugarResponse;
                patientResponse.PatientResponseValues.Add(BloodSugarResponse);

                IvrService vivifyService = new IvrService();
                patientSurvey.NextQuestion(MakePatientSelectionDescriptorFromPostModel(patientResponse));//Advance Question to the next question now, based on the patient response
                return vivifyService.PostPatientResponse(patientResponse);
            }

            return null;
        }
        private static int FetchOptionCodeForResponseValue(PatientSurveyQuestionModel patientQuestion, string value)//returns option code for a response string.
        {
            int retval = -1;
            value = value.Trim();
            List<PatientSurveyOptionModel> currentq = patientQuestion.PatientSurveyOptions;
            int opcount = 1;
            foreach (PatientSurveyOptionModel item in currentq)
            {
                bool matchesthisoption = false;
                foreach (PatientSurveyOptionTextModel text in item.PatientSurveyOptionTexts)
                {
                    if (string.Compare(value, text.Text, true) == 0)
                    {
                        matchesthisoption = true;
                        break;
                    }
                }
                if (opcount.ToString().Equals(value))
                {
                    matchesthisoption = true;
                }
                if (matchesthisoption)
                {
                    retval = item.PatientSurveyOptionId;
                    break;
                }
                opcount++;
            }
            return retval;
        }
        private static PatientResponseApiPostModel MakePostModelForResponseToQuestion(PatientSurveyQuestionModel currentQuestion, PatientModel patient)//will populate with appropriate PatientResponseValueApiPostModel with empty PatientResponseValues
        {
            PatientResponseApiPostModel output = new PatientResponseApiPostModel();
            output.PatientId = patient.Id;
            output.PatientSurveyQuestionId = currentQuestion.PatientSurveyQuestionId;
            output.SurveyQuestionTypeId = currentQuestion.SurveyQuestionTypeId;
            output.PatientResponseInputMethodId = 1;
            output.ObservationDateTime_UTC = System.DateTime.Now;
            output.PatientResponseValues = new List<PatientResponseValueApiPostModel>();

            return output;
        }
        private static List<int?> MakePatientSelectionDescriptorFromPostModel(PatientResponseApiPostModel input)
        {// PatientComponent.GetNextQuestion requires patient response values in this specific format.
            List<int?> retval = new List<int?>();
            foreach (PatientResponseValueApiPostModel entry in input.PatientResponseValues)
            {
                retval.Add(entry.PatientSurveyOptionId);
            }
            return retval;
        }

    }
}