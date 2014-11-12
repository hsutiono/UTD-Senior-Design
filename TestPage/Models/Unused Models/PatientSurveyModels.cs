using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Vivify.Platform.Models
{
    //patient survey models:
    public class PatientSurveyOptionTextModel
    {
        public int PatientSurveyOptionTextId { get; set; }
        public int PatientSurveyOptionId { get; set; }
        public string Text { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string LocaleCode { get; set; }
        public System.DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public Nullable<System.DateTime> DeletedDateTime_UTC { get; set; }
        public Nullable<int> DeletedBy_Id { get; set; }
    }

    public class PatientSurveyOptionModel
    {
        public PatientSurveyOptionModel()
        {
            //AlertSeverityLevelId = (int)AlertSeverityLevel.None;
            AlertSeverityLevelId = 0;
        }

        public int PatientSurveyOptionId { get; set; }
        public int PatientSurveyQuestionId { get; set; }
        public string OptionName { get; set; }
        public int AlertSeverityLevelId { get; set; }
        public Nullable<int> SurveyParameterTypeId { get; set; }
        public string SurveyParameterTypeName { get; set; }
        public int SortOrder { get; set; }
        public Nullable<int> SurveyVideoId { get; set; }
        public System.DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public Nullable<System.DateTime> DeletedDateTime_UTC { get; set; }
        public Nullable<int> DeletedBy_Id { get; set; }
        public ICollection<PatientSurveyOptionTextModel> PatientSurveyOptionTexts { get; set; }
        public PatientSurveyQuestionModel PatientSurveyQuestion { get; set; }
        public ICollection<PatientSurveyQuestionModel> PatientSurveyQuestions { get; set; }  //child questions
        //public PatientSurveyVideoModel SurveyVideo { get; set; }
    }

    public class PatientSurveyQuestionTextModel
    {
        public int PatientSurveyQuestionTextId { get; set; }
        public int PatientSurveyQuestionId { get; set; }
        public string Text { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string LocaleCode { get; set; }
        public System.DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public Nullable<System.DateTime> DeletedDateTime_UTC { get; set; }
        public Nullable<int> DeletedBy_Id { get; set; }
    }

    public class PatientSurveyQuestionModel
    {
        public int PatientSurveyQuestionId { get; set; }
        public int SurveyQuestionTypeId { get; set; }
        public string SurveyQuestionTypeName { get; set; }
        public Nullable<int> PatientSurveyId { get; set; }
        public int SortOrder { get; set; }
        public int SurveyQuestionCategoryId { get; set; }
        public string DisplayCondition { get; set; }
        public int ParentPatientSurveyOptionId { get; set; }
        public System.DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public Nullable<System.DateTime> DeletedDateTime_UTC { get; set; }
        public Nullable<int> DeletedBy_Id { get; set; }
        /*public AlertSeverityLevel MaxAlertSeverityLevel
        {
            get
            {
                AlertSeverityLevel retval = AlertSeverityLevel.None;

                foreach (PatientSurveyOptionModel PatientSurveyOption in PatientSurveyOptions)
                {
                    if (PatientSurveyOption.AlertSeverityLevelId < (int)retval)
                    {
                        retval = (AlertSeverityLevel)PatientSurveyOption.AlertSeverityLevelId;
                    }
                }

                return retval;
            }
        }*/

        public PatientSurveyModel PatientSurvey { get; set; }
        public ICollection<PatientSurveyOptionModel> PatientSurveyOptions { get; set; }
        public ICollection<PatientSurveyQuestionTextModel> PatientSurveyQuestionTexts { get; set; }
        public PatientSurveyOptionModel ParentSurveyOption { get; set; }
        /*
        public bool IsBiometricQuestion()
        {
            switch ((SurveyQuestionType)SurveyQuestionTypeId)
            {
                case SurveyQuestionType.BloodPressure:
                case SurveyQuestionType.BloodSugar:
                case SurveyQuestionType.PulseOx:
                case SurveyQuestionType.Weight:
                    {
                        return true;
                    }
            }
            return false;
        }*/
    }

    public class PatientSurveyModel
    {
        public int PatientSurveyId { get; set; }
        public int SurveyTypeId { get; set; }
        public string Name { get; set; }
        public int PatientId { get; set; }
        public System.DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public Nullable<System.DateTime> DeletedDateTime_UTC { get; set; }
        public Nullable<int> DeletedBy_Id { get; set; }

        public ICollection<PatientSurveyQuestionModel> PatientSurveyQuestions { get; set; }
        public PatientSurveyScheduleModel PatientSurveyScheduleModel { get; set; }
    }

    public class PatientSurveyCreateModel
    {
        public int PatientId { get; set; }
        public int SurveyTemplateId { get; set; }
    }

    public class SurveyTreeNodeModel
    {
        public int Id { get; set; }
        public int OriginalId { get; set; }
        public string ItemType { get; set; }
        public string PatientSurveyId { get; set; }
        public int? Parent_Id { get; set; }
        public int SortOrder { get; set; }
        public string Text { get; set; }
        //public AlertSeverityLevel AlertSeverityLevel { get; set; }
        /*public string AlertSeverityLevelName
        {
            get
            {
                return (AlertSeverityLevel == Models.AlertSeverityLevel.None) ? "" : AlertSeverityLevel.ToString();
            }
        }*/
        //public SurveyQuestionType? SurveyQuestionType { get; set; }
        public List<int> AnsweredOptionIds { get; set; }    //for selections
        public List<int> ParameterTypeIds { get; set; }     //for biometrics 
        public bool IsBiometric { get; set; }
        public bool IsVisible { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsOneTime { get; set; }
        public int UnAnsweredQuestionsCount { get; set; }
    }

    //Survey Scheduling
    public class SurveyTemplateSchedule
    {
        public int SurveyTemplateScheduleId { get; set; }
        public int SurveyTemplateId { get; set; }
        public bool IsOneTime { get; set; }

        //calculation props
        public int? DaysAfterProgramStartDate { get; set; }
        public int? DaysBeforeProgramEndDate { get; set; }

        //days of week
        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }

        //prompting 
        public DateTime SchedulePromptTimeOfDay { get; set; }
        public int StartMinutesBeforePrompt { get; set; }
        public int LateMinutesAfterPrompt { get; set; }
        public int EndMinutesAfterPrompt { get; set; }

        public DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public Nullable<System.DateTime> DeletedDateTime_UTC { get; set; }
        public Nullable<int> DeletedBy_Id { get; set; }
    }

    public class PatientSurveyScheduleModel
    {
        public PatientSurveyScheduleModel()
        {
            ActiveDays = new ActiveDaysOfWeek();
        }
        public int PatientSurveyScheduleId { get; set; }
        public int PatientSurveyId { get; set; }
        public bool IsOneTime { get; set; }	    //or weekly 
        public DateTime StartDate { get; set; }     //calculated or absolute, nullable. time is ignored.     

        //calculation props
        public int? DaysAfterProgramStartDate { get; set; }
        public int? DaysBeforeProgramEndDate { get; set; }

        public ActiveDaysOfWeek ActiveDays { get; set; }

        //prompting 
        public TimeSpan SchedulePromptTimeOfDay { get; set; }
        public int StartMinutesBeforePrompt { get; set; }
        public int LateMinutesAfterPrompt { get; set; }
        public int EndMinutesAfterPrompt { get; set; }

        public DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public Nullable<System.DateTime> DeletedDateTime_UTC { get; set; }
        public Nullable<int> DeletedBy_Id { get; set; }

        public PatientSurveyModel PatientSurvey { get; set; }
    }

    public class ActiveDaysOfWeek
    {
        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
    }
    public class PatientSurveyCompletedModel
    {
        public int PatientSurveyCompletedId { get; set; }
        public int PatientId { get; set; }
        public int PatientSurveyId { get; set; }
        public DateTime CompletedOn_UTC { get; set; }
        public DateTime CompletedDate { get; set; }

        public PatientSurveyModel PatientSurvey { get; set; }
    }
}