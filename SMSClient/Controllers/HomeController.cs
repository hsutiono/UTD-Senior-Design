using SMSClient.Components;
using SMSClient.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Collections.Concurrent;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SMSClient.Controllers
{

    public class HomeController : Controller
    {
        //public static Dictionary<string, SurveyInstance> reg = new Dictionary<string, SurveyInstance>();
        private static string FileName = HostingEnvironment.MapPath("~/App_Data/SavedRegistry.bin");
        
        private static Dictionary<string, int> ScheduledPatients = new Dictionary<string, int>()
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
            lock (responselock)
            {
                ActiveUserRegistry reg = getRegistry();
                ViewBag.reg = reg;
                ViewBag.current = reg.GetKeys();
            }
            return View();
        }

        public static ActiveUserRegistry getRegistry()//must be called within a lock
        {
            ActiveUserRegistry retval= null;
            if (System.IO.File.Exists(FileName))
            {
                Stream TestFileStream = System.IO.File.OpenRead(FileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                retval = (ActiveUserRegistry)deserializer.Deserialize(TestFileStream);
                TestFileStream.Close();
            }
            else
            {
                retval = new ActiveUserRegistry();
            }
            return retval;
        }

        public static void saveRegistry(ActiveUserRegistry reg)//must be called within a lock
        {
            Stream TestFileStream = System.IO.File.Create(FileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(TestFileStream, reg);
            TestFileStream.Close();
        }

        public static void AddUser(string number, string user)
        {
            lock (responselock)
            {
                ActiveUserRegistry reg = getRegistry();
                if (number.Length == 10)
                {
                    if (number.Substring(0, 2) != "+1")
                    {
                        number = "+1" + number;
                    }
                }
                if (number.Length == 12)
                {
                    int val = 5;
                    int.TryParse(user, out val);
                    reg.AddUser(number, val);
                }
                saveRegistry(reg);
            }
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
                ActiveUserRegistry reg = getRegistry();
                responseText = SmsResponse.HandleSmsResponse(patientResponse, reg);
                saveRegistry(reg);
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
            lock (responselock)
            {
                ActiveUserRegistry reg = getRegistry();
                foreach (string item in ScheduledPatients.Keys)
                {
                    reg.AddUser(item, ScheduledPatients[item]);
                }
                saveRegistry(reg);
            }
            return View();
        }
    }
}
