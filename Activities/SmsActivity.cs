using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mod.Twilio.Services;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;

namespace Mod.Twilio.Activities {
    [OrchardFeature("Mod.Twilio.Workflows")]
    public class SmsActivity : Task {
        private readonly ITwilioService _twilioService;

        public SmsActivity(ITwilioService twilioService) {
            T = NullLocalizer.Instance;
            _twilioService = twilioService;
        }

        public Localizer T { get; set; }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            return new[] { T("Done") };
        }

        public override string Form {
            get {
                return "SmsActivity";
            }
        }

        public override LocalizedString Category {
            get { return T("Messaging"); }
        }

        public override string Name {
            get { return "SendSms"; }
        }

        public override LocalizedString Description {
            get { return T("Sends an sms to a specific user."); }
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            var body = activityContext.GetState<string>("Body");
            var recipients = activityContext.GetState<string>("Recipients");

            foreach(var recipient in recipients.Split(',')) {
                _twilioService.SendSms(recipient, body);
            }

            yield return T("Done");
        }
    }
}