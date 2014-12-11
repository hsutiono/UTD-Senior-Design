using SMSClient.Components;
using SMSClient.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections.Concurrent;
using System;

namespace SMSClient.Controllers
{

    public class HomeController : Controller
    {
        public static Dictionary<string, SurveyInstance> reg = new Dictionary<string, SurveyInstance>();

        public static Dictionary<string, int> ScheduledPatients = new Dictionary<string, int>()
            {// populate this list with all valid phone numbers and corresponding user numbers
                {"+12817813990",7},
                {"+14093546970",6},
                {"+12148548186",3},
                {"+12062889006",1},
                {"+19729630930",5}
            };
        private static readonly Object responselock = new Object();
        public ActionResult Index()
        {
            ViewBag.reg = reg;
            ViewBag.current = reg.Keys;
            return View();
        }

        public ActionResult TwilioResponse()//twilio's response hits this.
        {
            ResponseModel patientResponse = new ResponseModel();
            patientResponse.From = Request["From"];
            patientResponse.To = Request["To"];
            patientResponse.ResponseText = Request["Body"];
            string responseText = "";
            lock (responselock)
            {
                responseText = SmsResponse.HandleSmsResponse(patientResponse, reg);
            }
            if (responseText != null)
            {
                ViewBag.responseText = responseText;
            }
            else
            {
                ViewBag.responseText = "Something has gone wrong. ";
            }

            return View();
        }
        public ActionResult StartSurvey()
        {
            foreach(string item in ScheduledPatients.Keys)
            {
                reg[item] = new SurveyInstance(item, ScheduledPatients[item]);
            }
            return View();
        }
    }
}
