using SMSClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMSClient.Components
{
    public class SmsValidation
    {
        private const int MINbloodSugar = 0;
        private const int MAXbloodSugar = 2000;
        private const int MINoxygen = 0;
        private const int MAXoxygen = 100;
        private const int MINheartRate = 0;
        private const int MAXheartRage = 400;
        private const int MINweight = 0;
        private const int MAXweight = int.MaxValue;
        private const int MINsystolic = 0;
        private const int MAXsystolic = int.MaxValue;
        private const int MINdiastolic = 0;
        private const int MAXdiastolic = int.MaxValue;

        internal static bool validBloodSugarResponse(string patientResponse)
        {
            bool valid = false;
            int success = 0;
            int bloodSugar = 0;

            if (patientResponse != string.Empty)
            {
                if (Int32.TryParse(patientResponse, out success))
                {
                    bloodSugar = int.Parse(patientResponse);

                    if (bloodSugar >= MINbloodSugar && bloodSugar <= MAXbloodSugar)
                    {
                        valid = true;
                    }
                }
            }
            return valid;
        }

        internal static bool validPulseOxHeartRate(string oxygen, string heartRate)
        {
            bool valid = false;
            int oxSuccess = 0;
            int rateSuccess = 0;
            int ox = 0;
            int rate = 0;

            if (Int32.TryParse(oxygen, out oxSuccess) && Int32.TryParse(heartRate, out rateSuccess))
            {
                ox = int.Parse(oxygen);
                rate = int.Parse(heartRate);

                if ((ox >= MINoxygen && ox <= MAXoxygen) && (rate >= MINheartRate && rate <= MAXheartRage))
                {
                    valid = true;
                }
            }

            return valid;
        }

        internal static bool validWeightResponse(string weight)
        {
            bool valid = false;
            int weightSuccess = 0;
            int weightResponse = 0;

            if(weight != string.Empty)
            {
                if (Int32.TryParse(weight, out weightSuccess))
                {
                    weightResponse = int.Parse(weight);

                    if (weightResponse >= MINweight && weightResponse <= MAXweight)
                    {
                        valid = true;
                    }
                }
            }

            return valid;
        }

        internal static bool validBloodPressure(string systole, string diastole)
        {
            bool valid = false;
            int systoleSuccess = 0;
            int diastoleSuccess = 0;
            int systoleResponse = 0;
            int diastoleResponse = 0;

            if (Int32.TryParse(systole, out systoleSuccess) && Int32.TryParse(diastole, out diastoleSuccess))
            {
                systoleResponse = int.Parse(systole);
                diastoleResponse = int.Parse(diastole);

                if ((systoleResponse >= MINsystolic) && (systoleResponse <= MAXsystolic) && (diastoleResponse >= MINdiastolic) && (diastoleResponse <= MAXdiastolic))
                {
                    valid = true;
                }
            }

            return valid;
        }

        internal static bool validSingleSelection(SurveyInstance patientSurvey, int currentQuestion, string singleResponse)
        {
            bool valid = false;
            int isOptionNumber = 0;
            int chosenOption = 0;
            int numberOfOptions = patientSurvey.GetCurrentQuestion().PatientSurveyOptions.Count();

            if (singleResponse != string.Empty)
            {
                if (Int32.TryParse(singleResponse, out isOptionNumber))
                {
                    chosenOption = int.Parse(singleResponse);

                    if (chosenOption > 0 && chosenOption <= numberOfOptions)
                    {
                        valid = true;
                    }
                }
            }

            return valid;
        }

        internal static bool validMultipleSelection(SurveyInstance patientSurvey, int currentQuestion, string multipleResponse)
        {
            bool valid = false;
            int chosenOpt = 0;
            int chosenValid = 0;
            List<int> chosenOptions = new List<int>();
            char[] delimitors = { ' ', ',' };
            string[] parts = multipleResponse.Split(delimitors);
            int numberOfOptions = patientSurvey.GetCurrentQuestion().PatientSurveyOptions.Count();

            if (multipleResponse != string.Empty)
            {
                foreach (string part in parts)
                {
                    if (Int32.TryParse(part, out chosenValid))
                    {
                        chosenOpt = int.Parse(part);
                        chosenOptions.Add(chosenOpt);
                    }
                }

                foreach (int opt in chosenOptions)
                {
                    if (opt > 0 && opt <= numberOfOptions)
                    {
                        valid = true;
                    }
                    else
                    {
                        valid = false;
                        break;
                    }
                }
            }

            return valid;
        }
    }
}