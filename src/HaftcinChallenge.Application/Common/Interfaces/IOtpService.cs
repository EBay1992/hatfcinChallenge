namespace HaftcinChallenge.Application.Common.Interfaces;

public interface IOtpService
{
    (string Otp, string HashedOtp) GenerateOtp();
    public bool VerifyOtp(string hashedOtp, string receivedOtp);
}