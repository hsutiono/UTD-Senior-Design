using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSClient.Models
{
    [Serializable()]
    public class ActiveUserRegistry
    {
        private Dictionary<string, SurveyInstance> reg = new Dictionary<string, SurveyInstance>();
        public Dictionary<string, SurveyInstance>.KeyCollection GetKeys()
        {
            return reg.Keys;
        }
        public SurveyInstance Get(string key)
        {
            return reg[key];
        }
        public SurveyInstance Set(string key,SurveyInstance val)
        {
            SurveyInstance temp = reg[key];
            reg[key] = val;
            return temp;
        }
        public SurveyInstance Remove(string key)
        {
            SurveyInstance temp = reg[key];
            reg.Remove(key);
            return temp;
        }
        public bool AddUser(string phone,int userId)
        {
            if(reg.Keys.Contains(phone))
            {
                return false;
            }
            else
            {
                reg[phone] = new SurveyInstance(phone, userId);
                return true;
            }
        }
    }
}