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

namespace TestPage.Controllers
{
    public class SurveyController : Controller
    {
        public static ActiveRegistry reg = new ActiveRegistry();
        public ActionResult Index()
        {
            ViewBag.reg = reg;
            ViewBag.current = reg.active.Keys;
            return View();
        }
        public ActionResult TwilioResponse()//twilio's response hits this.
        {
            ViewBag.reg = reg;
            return View();
        }
    }
}
