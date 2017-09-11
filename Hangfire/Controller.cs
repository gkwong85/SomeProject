[assembly: OwinStartupAttribute(typeof(startup))]
namespace Sabio.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("connectionName");
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire");

            // Cron.Daily(int) => is UTC @ 12pm
            RecurringJob.AddOrUpdate(() => SendAnalytics() , Cron.Daily(12));
        }
        
        public void SendAnalytics()
        {
            var sendEmail = (AnalyticsInfoService)
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(AnalyticsInfoService));

            sendEmail.SendAnalytics();
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}