using Hangfire.Dashboard;

namespace NotiGest.Configurations
{
    public class HangfireAuthorization : IDashboardAuthorizationFilter
    {
        public bool? Administrador { get; set; }

        public HangfireAuthorization(bool? Administrador_Hangfire)
        {
            Administrador = Administrador_Hangfire;
        }
        public bool Authorize(DashboardContext context)
        {
            return Administrador ?? false;
        }
    }
}
