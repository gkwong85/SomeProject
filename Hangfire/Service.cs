namespace Sabio.Services.Interfaces
{
    public class AnalyticsInfoService
    {
        private IEmailService emailService;
        private IDataProvider dataProvider;

        public AnalyticsInfoService(IEmailService emailService, IDataProvider dataProvider)
        {
            this.emailService = emailService;
            this.dataProvider = dataProvider;
        }

        public void SendAnalytics()
        {
            List<LogEvents_GetAnalyticsInfoService> data = GetAnalyticsData();

            MailAddress[] emails = new MailAddress[]
            {
                //new MailAddress("wmaldonado@mailinator.com", "Wendy Maldonado"),
                new MailAddress("address@address.com", "name")
            };

            MailAddress from = new MailAddress("address@support.com", "name");
            string subject = "Analytic Data";
            string htmlBody = null;
            string htmlContent =
            "email info"
            emailService.Send(emails, from, subject, htmlBody, htmlContent);
        }

        private List<LogEvents_GetAnalyticsInfoService> GetAnalyticsData()
        {
            List<LogEvents_GetAnalyticsInfoService> results = new List<LogEvents_GetAnalyticsInfoService>();
            LogEvents_GetAnalyticsInfoService data = null;
            
            dataProvider.ExecuteCmd("some_procedure",
                parameters => {},
                (reader, set) =>
                {
                    data = new LogEvents_GetAnalyticsInfoService();
                    int startingIndex = 0;

                    data.NewUsers = reader.GetSafeInt32(startingIndex++);
                    data.WatchedVideos = reader.GetSafeInt32(startingIndex++);

                    results.Add(data);
                });
            return results;
        }
    }
}
