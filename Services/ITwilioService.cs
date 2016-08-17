using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Twilio;

namespace Mod.Twilio.Services {
    public interface ITwilioService : IDependency {
        void SendSms();
    }

    public class TwilioService : ITwilioService {


        public void SendSms() {

            TwilioRestClient smsClient = new TwilioRestClient(twilioSettings.AccountSID, twilioSettings.AuthToken);

            try {
                var msg = smsClient.SendSmsMessage(twilioSettings.FromNumber, number, context.MailMessage.Body);
                Logger.Debug("Message sent to {0}: {1}", number, context.Type);
            }
            catch (Exception e) {
                Logger.Error(e, "An unexpected error while sending a message to {0}: {1}", number, context.Type);
            }
        }
    }
}