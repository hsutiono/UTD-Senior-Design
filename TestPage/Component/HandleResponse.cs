﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestPage.Models;

namespace TestPage.Component
{
    public class HandleResponse
    {
        public static bool HandleMultiSelectionResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }

        public static bool HandleSingleSelectionResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }

        public static bool HandleNumberResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }

        public static bool HandlePulseOxResponse(SurveyInstance patientSurvey, ResponseModel response)
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

        public static bool HandleWeightResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }

        public static bool HandleBloodPressureResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }

        public static bool HandleBloodSugarResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            throw new NotImplementedException();
        }
    }
}