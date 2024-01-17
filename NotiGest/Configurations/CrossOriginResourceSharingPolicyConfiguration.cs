namespace NotiGest.Configurations
{
    public class CrossOriginResourceSharingPolicyConfiguration
    {
        public required IEnumerable<string> AllowedOrigins { get; set; }
    }
}
