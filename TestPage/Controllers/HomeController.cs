using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using TestPage.Models;
using System.Web.Mvc.Html;

namespace TestPage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult SendButton()
        {
            Team3[] team3Members = new Team3[1];
            //team3Members[0] = new Team3 { Name = "Zheng Li", Number = "+12817813990" };
            //team3Members[1] = new Team3 { Name = "Hansel Sutiono", Number = "+19729630930" };
            //team3Members[2] = new Team3 { Name = "Elena Zhang", Number = "+12062889006" };
            //team3Members[3] = new Team3 { Name = "Dylan Boor", Number = "+14093546970" };
            team3Members[0] = new Team3 { Name = "Gerardus Hardjo", Number = "+12148548186" };



            string AccountSid = "AC7a6db27538ba8ed863c14e825beb35f4";
            string AuthToken = "56cb022777274d5e98fdeed9523987a7";
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            foreach (var member in team3Members)
            {
                var message = twilio.SendMessage("+17743077070", member.Number, "Hello");
            }

            return View("Success");
        }

        #region We Don't Need This at All, Ignore This
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        #endregion
    }
}