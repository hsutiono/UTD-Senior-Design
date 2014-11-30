using NLog;
using System.Net;
using Twilio;

namespace SMSClient.Integration
{
    public class TwilioService:TwilioRestClient //interface with Twilio API
    {
        private const string DefaultServerPhone = "+17743077070";
        private const string DefaultAccountId = "AC7a6db27538ba8ed863c14e825beb35f4";
        private const string DefaultAuthtoken = "56cb022777274d5e98fdeed9523987a7";

        private string ServerPhone;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public TwilioService(string _phone = DefaultServerPhone,string _AccountSid = DefaultAccountId, string _Authtoken = DefaultAuthtoken)
            : base(_AccountSid, _Authtoken)
        {
            ServerPhone = _phone;
            if(ServerPhone == null)
            {
                ServerPhone = DefaultServerPhone;
            }
            // ignore SSL certificate errors.
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;
        }
        public string GetServerPhone()
        {
            return ServerPhone;
        }
        public Message SendMessage(string to, string body)
        {
            Message response = base.SendMessage(ServerPhone,to,body);
            if(response.Status.Equals("failed")||response.Status.Equals("undelivered"))
            {
                Log.Error("SMS message send error. Error Code: {0}, ; Message:{1} ",response.ErrorCode,response.ErrorMessage);
            }
            return response;
        }
    }
}