using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestPage.Models
{
    public class PatientSurvey
    {
        public int PatientSurveyId { get; set; }
        public int SurveyTypeId { get; set; }
        public string Name { get; set; }
        public string CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public string DeletedDateTime_UTC { get; set; }
        public int DeletedBy_Id { get; set; }
        public List<PatientSurveyQuestion> PatientSurveyQuestions { get; set; }
        public string ToString()
        {
            string s = this.Name+" ";
            foreach(PatientSurveyQuestion i in PatientSurveyQuestions)
            {
                s += i.ToString() + " ";
            }
            return s;
        }
    }
    public class PatientSurveyQuestion
    {
        public int PatientSurveyQuestionId { get; set; }
        public int SurveyQuestionTypeId { get; set; }
        public string SurveyQuestionTypeName { get; set; }
        public int PatientSurveyId { get; set; }
        public int SortOrder { get; set; }
        public int SurveyQuestionCategoryId { get; set; }
        public int DisplayCondition { get; set; }//note: not sure what type
        public int ParentPatientSurveyOptionId { get; set; }
        public string CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public string DeletedDateTime_UTC { get; set; }
        public int DeletedBy_Id { get; set; }
        public int MaxAlertSeverityLevel { get; set; }
        public List<PatientSurveyOption> PatientSurveyOptions { get; set; }
        public List<PatientSurveyQuestionText> PatientSurveyQuestionTexts { get; set; }
        public string ParentSurveyOption { get; set; }//note:not sure what type
        public string ToString()
        {
            string s = this.SurveyQuestionTypeName+" {";
            foreach (PatientSurveyOption i in PatientSurveyOptions)
            {
                s+=i.ToString()+" ";
            }
            s += "} ";
            foreach (PatientSurveyQuestionText i in PatientSurveyQuestionTexts)
            {
                s += i.ToString() + " ";
            }
            return s;
        }
    }
    public class PatientSurveyOption
    {
        public int PatientSurveyOptionId { get; set; }
        public int patientSurveyQuestionId { get; set; }
        public string OptionName { get; set; }
        public int AlertSeveritylevelId { get; set; }
        public int SurveyParameterTypeId { get; set; }
        public string SurveyparameterTypeName { get; set; }
        public int SortOrder { get; set; }
        public int SurveyVideoId { get; set; } //not sure type
        public string CreatedDateTime_UTC { get; set; }
        public int CreatedBy_ID { get; set; }
        public string DeletedDateTime_UTC { get; set; }
        public int DeletedBy_Id { get; set; }
        public List<PatientSurveyOptionText> PatientSurveyOptionTexts { get; set; }
        public List<PatientSurveyQuestion> PatientSurveyQuestions { get; set; }
        public string SurveyVideo { get; set; }//not sure type
        public string ToString()
        {
            string s = this.OptionName + " <";
            foreach (PatientSurveyOptionText i in PatientSurveyOptionTexts)
            {
                s += count + ":" + i.ToString() + " ";
            }
            s += "> ";
            foreach (PatientSurveyQuestion i in PatientSurveyQuestions)
            {
                s += i.ToString() + " ";
            }
            return s;
        }
    }
    public class PatientSurveyQuestionText
    {
        public int PatientSurveyQuestionTextId { get; set; }
        public int PatientSurveyQuestionId { get; set; }
        public string Text { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string LocaleCode { get; set; }
        public string CreatedDateTime_UTC { get; set; }
        public int CreatedBy_ID { get; set; }
        public string DeletedDateTime_UTC { get; set; }
        public int DeletedBy_Id { get; set; }
        public string ToString()
        {
            return Text;
        }
    }
    public class PatientSurveyOptionText
    {
        public int PatientSurveyOptionTextId { get; set; }
        public int PatientSurveyOptionId { get; set; }
        public string Text { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string LocaleCode { get; set; }
        public string CreatedDateTime_UTC { get; set; }
        public int CreatedBy_ID { get; set; }
        public string DeletedDateTime_UTC { get; set; }
        public int DeletedBy_Id { get; set; }
        public string ToString()
        {
            return Text;
        }
    }
}