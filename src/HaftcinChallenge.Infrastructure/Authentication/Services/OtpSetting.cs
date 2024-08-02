namespace HaftcinChallenge.Infrastructure.Authentication.Services;

public class OtpSettings
{ 
        public const string Section = "OtpSettings";
        public string SecretKey { get; set; } = string.Empty;
}