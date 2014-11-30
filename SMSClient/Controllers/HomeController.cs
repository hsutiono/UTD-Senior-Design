using SMSClient.Components;
using SMSClient.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMSClient.Controllers
{

    public class HomeController : Controller
    {
        public static Dictionary<string, SurveyInstance> reg = new Dictionary<string, SurveyInstance>();

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

            string responseText = SmsResponse.HandleSmsResponse(patientResponse, reg);
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
    }
}
