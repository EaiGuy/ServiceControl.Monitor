namespace EaiGuy.ServiceControl.Monitor.Emailer
{
    using EaiGuy.ServiceControl.Monitor.Messages.Events;
    using Email;
    using Email.Messages.Commands;
    using log4net;
    using NServiceBus;
    using RazorEngine.Templating;
    using System;
    using System.IO;

    public class HealthStatusChangedHandler : IHandleMessages<IHealthStatusChanged>
    {
        public TemplateService TemplateService { get; set; }
        public RazorEngineService RazorEngineService { get; set; }
        private static readonly ILog log = LogManager.GetLogger(typeof(HealthStatusChangedHandler));

        public void Handle(IHealthStatusChanged message)
        {
            // parse the email body
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\EmailTemplate.cshtml");
            string body = this.TemplateService.Parse(File.ReadAllText(templatePath), message, null, "AlertEmails");

            // get rid of new lines since the EmailSenderService replaces them with paragraph tags
            body = body.Replace("\r\n", "");

            log.Info("Sending email: " + message.ChangeSummary);

            var settings = new ServiceControlMonitorEmailerSettings();

            new EmailProvider(new EmailSettings())
                .SendEmail(new EmailCommand(settings.ErrorEmailAddress, message.ChangeSummary, body));
        }
    }
}
