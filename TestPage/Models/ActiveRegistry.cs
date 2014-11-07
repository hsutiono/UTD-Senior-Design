using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using TestPage.Models;
using System.Web.Mvc.Html;
using RestSharp;
using System.Net;
using Twilio.Mvc;
using Twilio.TwiML;

namespace TestPage.Models
{
    public class ActiveRegistry
    {
        public Dictionary<string, SurveyInstance> active;
        public ActiveRegistry()
        {
            active = new Dictionary<string, SurveyInstance>();
        }
        public void addInstance(SurveyInstance s)
        {
            active.Add(s.phone, s);
        }
        public void addInstance(int UserID,string Phone)
        {
            active.Add(Phone, new SurveyInstance(UserID, Phone));
        }
        public string response(string from, string to, string message)
        {
            if(SurveyInstance.server.Equals(to))
            {
                SurveyInstance t = active[from];
                string temp = t.response(message);
                if (t != null && temp != null) return temp;
                if(temp == null)
                {
                    active.Remove(from);
                    return "Survey Over.";
                }
            }
            return "";
        }
    }
}
