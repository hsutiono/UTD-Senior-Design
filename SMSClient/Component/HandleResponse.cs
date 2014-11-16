using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMSClient.Integration;
using SMSClient.Models;

namespace SMSClient.Component
{
    public class HandleResponse
    {
        public static bool HandleMultiSelectionResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string multipleResponse = response.ResponseText;

            bool isValid = SmsValidation.validMultilpeSelection(patientSurvey, patientSurvey.CurrentQuestion, multipleResponse);

            return isValid;
        }

        public static bool HandleSingleSelectionResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string singleResponse = response.ResponseText;

            bool isValid = SmsValidation.validSingleSelection(patientSurvey, patientSurvey.CurrentQuestion, singleResponse);

            return isValid;
        }

        #region questionable handling function
        public static bool HandleNumberResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }
        #endregion

        public static bool HandlePulseOxResponse(SurveyInstance patientSurvey, ResponseModel response)
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
                PatientResponseApiPostModel patientResponse = patientSurvey.MakePostModelForResponseToCurrentQuestion();

                // Add Heart Rate Value
                PatientResponseValueApiPostModel hearRateResonse = new PatientResponseValueApiPostModel();
                hearRateResonse.SurveyParameterTypeId = 5; // heart rate
                hearRateResonse.Value = heartRate;
                patientResponse.PatientResponseValues.Add(hearRateResonse);

                // Add Oxygen Value
                PatientResponseValueApiPostModel OxygenResonse = new PatientResponseValueApiPostModel();
                OxygenResonse.SurveyParameterTypeId = 6; // Oxygen
                OxygenResonse.Value = oxygen;
                patientResponse.PatientResponseValues.Add(OxygenResonse);

                IvrService vivifyService = new IvrService();
                vivifyService.PostPatientResponse(patientResponse);
            }


            return isValid;
        }

        public static bool HandleWeightResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string weight = response.ResponseText;

            bool isValid = SmsValidation.validWeightResponse(weight);

            return isValid;
        }

        public static bool HandleBloodPressureResponse(SurveyInstance patientSurvey, ResponseModel response)
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

            return isValid;
        }

        public static bool HandleBloodSugarResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string patientResponse = response.ResponseText;
            
            bool isValid = SmsValidation.validBloodSugarResponse(patientResponse);

            return isValid;
        }
    }
}