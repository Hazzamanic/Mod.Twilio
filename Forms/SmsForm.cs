using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Forms.Services;
using Orchard.Localization;

namespace Mod.Twilio.Forms {
    [OrchardFeature("Mod.Twilio.Workflows")]
    public class SmsForm : Component, IFormProvider {
        protected dynamic New { get; set; }

        public SmsForm(IShapeFactory shapeFactory) {
            New = shapeFactory;
        }

        public void Describe(DescribeContext context) {
            Func<IShapeFactory, dynamic> formFactory =
                shape => {
                    var form = New.Form(
                        Id: "SmsActivity",
                        _Type: New.FieldSet(
                            Title: T("Send to"),
                            _Recipients: New.Textbox(
                                Id: "recipients",
                                Name: "Recipients",
                                Title: T("Phone Numbers"),
                                Description: T("Specify a comma-separated list of recipient phone numbers"),
                                Classes: new[] { "large", "text", "tokenized" }),
                            _Message: New.Textarea(
                                Id: "Body", Name: "Body",
                                Title: T("Message"),
                                Description: T("The body of the sms message."),
                                Classes: new[] { "tokenized" })
                            ));

                    return form;
                };

            context.Form("SmsActivity", formFactory);
        }

    }

    public class SmsFormValidator : IFormEventHandler {
        public Localizer T { get; set; }
        public void Building(BuildingContext context) { }
        public void Built(BuildingContext context) { }
        public void Validated(ValidatingContext context) { }

        public void Validating(ValidatingContext context) {
            if (context.FormName != "SmsActivity") return;

            var recipients = context.ValueProvider.GetValue("Recipients").AttemptedValue;
            var body = context.ValueProvider.GetValue("Body").AttemptedValue;


            if (String.IsNullOrWhiteSpace(recipients)) {
                context.ModelState.AddModelError("Recipients", T("You must specify at least one recipient.").Text);
            }

            if (String.IsNullOrWhiteSpace(body)) {
                context.ModelState.AddModelError("Body", T("You must provide a Body.").Text);
            }
        }
    }
}