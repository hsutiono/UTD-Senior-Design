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
        public List<PatientSurveySchedule> PatientSurveyScheduleModel { get; set; }
        public override string ToString()
        {
            PatientSurveyQuestion[] k = PatientSurveyQuestions.ToArray<PatientSurveyQuestion>();
            Array.Sort(k, delegate(PatientSurveyQuestion user1, PatientSurveyQuestion user2)
            {
                return user1.PatientSurveyQuestionId.CompareTo(user2.PatientSurveyQuestionId);
            });
            
            
            string s = "("+this.Name+") ";
            foreach(PatientSurveyQuestion i in k)
            {
                s += i.PatientSurveyQuestionId+": "+i.ToString() + " ";
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
        public string GetQuestions()
        {
            string s = "";//this.SurveyQuestionTypeName;
                
            foreach (PatientSurveyQuestionText i in PatientSurveyQuestionTexts)
            {
                s += i.ToString() + " ";
            }
                
            s+=" {";
            foreach (PatientSurveyOption i in PatientSurveyOptions)
            {
                s+=i.ToString()+" ";
            }
            s += "} ";
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
        public override string ToString()
        {
            string s = this.OptionName + " <";
            foreach (PatientSurveyOptionText i in PatientSurveyOptionTexts)
            {
                s += i.ToString() + " ";
            }
            s += "> ";


            //foreach (PatientSurveyQuestion i in PatientSurveyQuestions)
            //{
            //    s += i.ToString() + " ";
            //}


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
        public override string ToString()
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
        public override string ToString()
        {
            return Text;
        }
    }
    public class PatientSurveySchedule
    {
        public int PatientSurveyScheduleId { get; set; }
        public int PatientSurveyId { get; set; }
        public bool IsOneTime { get; set; }
        public string StartDate { get; set; }
        public int DaysAfterProgramStartDate { get; set; }
        public int DaysBeforeProgramEndDate { get; set; }
        public ActiveDaySet ActiveDays{ get; set; }
        public string SchedulePromptTimeOfDay { get; set; }
        public int StartMinutesBeforePrompt { get; set; }
        public int LateMinutesAfterPrompt { get; set; }
        public int EndMinutesAfterPrompt { get; set; }
        public string CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public string DeletedDateTime_UTC { get; set; }
        public int DeletedBy_Id { get; set; }
    }
    public class ActiveDaySet
    {
        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }

    }
}