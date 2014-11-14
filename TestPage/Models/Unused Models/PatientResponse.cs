using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestPage.Models
{
    public class PatientResponse
    {
        public int PatientId { get; set; }
        public int PatientSurveyQuestionId { get; set; } //only used in survey
        public int SurveyQuestionTypeId { get; set; }
        public int PatientResponseInputMethodId { get; set; }
        public string ObservationDateTime_UTC { get; set; }
        public List<PatientResponseValue> PatientResponseValues { get; set; }
    }
    public class PatientResponseValue
    {
        public int SurveyParameterTypeId { get; set; }
        public int PatientSurveyOptionId { get; set; }
        public int Value { get; set; }
    }
}