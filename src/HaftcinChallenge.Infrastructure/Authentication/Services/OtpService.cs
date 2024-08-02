using System.Security.Cryptography;
using HaftcinChallenge.Application.Common.Interfaces;
using OtpNet;

namespace HaftcinChallenge.Infrastructure.Authentication.Services;

public class OtpService : IOtpService
{
    private readonly byte[] _secretKey;

    public OtpService(string secretKey)
    {
        _secretKey = Base32Encoding.ToBytes(secretKey);
    }

    public (string Otp, string HashedOtp) GenerateOtp()
    {
        var totp = new Totp(_secretKey);
        var otp = totp.ComputeTotp(DateTime.UtcNow);
        var hashedOtp = HashOtp(otp);
        return (otp, hashedOtp);
    }

    public bool VerifyOtp(string hashedOtp, string receivedOtp)
    {
        var hashedReceivedOtp = HashOtp(receivedOtp);
        return hashedOtp == hashedReceivedOtp;
    }

    private string HashOtp(string otp)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(otp));
            return Convert.ToBase64String(hashBytes);
        }
    }
}