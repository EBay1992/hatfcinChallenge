using HaftcinChallenge.Domain.Common.Interfaces;
using Throw;

namespace HaftcinChallenge.Domain.Users
{
    public class User
    {
        public Guid Id { get; private set; }
        public bool IsVerified { get; private set; } = false;
        public string MobileNumber { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Email { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string? OtpHash { get; private set; }
        public DateTime? OtpSetTime { get; private set; }
        private static readonly TimeSpan OtpExpirationTime = TimeSpan.FromSeconds(60);

        public User(
            string mobileNumber,
            string? firstName = null,
            string? lastName = null,
            string? email = null,
            DateTime? dateOfBirth = null,
            Guid? id = null
        )
        {
            MobileNumber = mobileNumber.Throw().IfEmpty().Value;
            Id = id ?? Guid.NewGuid();
            
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateOfBirth = dateOfBirth ?? new DateTime(1900, 1, 1);
        }

        public void SetOtp(string otp, IDateTimeProvider dateTimeProvider)
        {
            OtpHash = otp.Throw().IfEmpty().Value;
            OtpSetTime = dateTimeProvider.UtcNow;
        }

        public void VerifyUser()
        {
            IsVerified = true;
            OtpHash = null;
            OtpSetTime = null;
        }

        public void UpdateProfile(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            FirstName = firstName.Throw().IfEmpty().Value;
            LastName = lastName.Throw().IfEmpty().Value;
            Email = email.Throw().IfEmpty().Value;
            DateOfBirth = dateOfBirth.ThrowIfNull().Value;
        }

        public bool IsOtpExpired(IDateTimeProvider dateTimeProvider)
        {
            return OtpSetTime == null || dateTimeProvider.UtcNow - OtpSetTime > OtpExpirationTime;
        }

        public void RemoveExpiredOtp()
        {
            OtpHash = null;
            OtpSetTime = null;
        }

        private User() { }
    }
}