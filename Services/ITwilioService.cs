using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mod.Twilio.Models;
using Orchard;
using Orchard.ContentManagement;
using Twilio;

namespace Mod.Twilio.Services {
    public interface ITwilioService : IDependency {
        void SendSms(string number, string message);
    }

    public class TwilioService : Component, ITwilioService {
        private readonly TwilioSettingsPart _twilioSettings;

        public TwilioService(IOrchardServices orchardServices) {
            _twilioSettings = orchardServices.WorkContext.CurrentSite.As<TwilioSettingsPart>();
        }

        public void SendSms(string number, string message) {

            if(String.IsNullOrWhiteSpace(_twilioSettings.AccountSID))
                return;

            TwilioRestClient smsClient = new TwilioRestClient(_twilioSettings.AccountSID, _twilioSettings.AuthToken);

            try {
                var msg = smsClient.SendMessage(_twilioSettings.FromNumber, number, message);

            }
            catch (Exception e) {
            }
        }
    }
}