using FluentAssertions;
using HaftcinChallenge.Domain.Common.Interfaces;
using HaftcinChallenge.Domain.Users;
using NSubstitute;

namespace HaftcinChallenge.Domain.Tests
{
    public class UserTests
    {
        [Fact]
        public void SetOtp_WhenSetOtpCalled_ShouldSetOtpHashAndOtpSetTimeAndThrowIfEmpty()
        {
            // Arrange
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var now = DateTime.UtcNow;
            dateTimeProvider.UtcNow.Returns(now);

            var user = new User("091209121213");

            // Act
            user.SetOtp("123456", dateTimeProvider);
            var actionWithEmptyOtp = () => user.SetOtp("", dateTimeProvider);

            // Assert
            user.OtpHash.Should().Be("123456");
            user.OtpSetTime.Should().Be(now);

            actionWithEmptyOtp.Should().Throw<ArgumentException>()
                .WithMessage("*string should not be empty.*")
                .And.ParamName.Should().Be("otp");
        }

        [Theory]
        [InlineData(59, false)] // OTP should not be expired after 59 seconds
        [InlineData(61, true)]  // OTP should be expired after 61 seconds
        public void IsOtpExpired_WhenCalled_ShouldReturnCorrectExpirationStatus(int secondsToAdd, bool expectedResult)
        {
            // Arrange
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var now = DateTime.UtcNow;
            dateTimeProvider.UtcNow.Returns(now);

            var user = new User("091209121213");
            user.SetOtp("123456", dateTimeProvider);

            // Act
            dateTimeProvider.UtcNow.Returns(now.AddSeconds(secondsToAdd));
            var result = user.IsOtpExpired(dateTimeProvider);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void RemoveExpiredOtp_WhenCalled_ShouldRemoveOtp()
        {
            // Arrange
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var now = DateTime.UtcNow;
            dateTimeProvider.UtcNow.Returns(now);

            var user = new User("091209121213");
            user.SetOtp("123456", dateTimeProvider);

            // Act
            user.RemoveExpiredOtp();

            // Assert
            user.OtpHash.Should().BeNull();
            user.OtpSetTime.Should().BeNull();
        }

        [Fact]
        public void VerifyUser_WhenCalled_ShouldSetIsVerifiedToTrueAndClearOtp()
        {
            // Arrange
            var user = new User("091209121213");
            user.SetOtp("123456", Substitute.For<IDateTimeProvider>());

            // Act
            user.VerifyUser();

            // Assert
            user.IsVerified.Should().BeTrue();
            user.OtpHash.Should().BeNull();
            user.OtpSetTime.Should().BeNull();
        }

        [Fact]
        public void UpdateProfile_WhenCalled_ShouldUpdateProfileInformation()
        {
            // Arrange
            var user = new User("091209121213");

            // Act
            user.UpdateProfile(
                "John", 
                "Doe",
                "john.doe@gmail.com",
                new DateTime(1990, 1, 1)
                );

            // Assert
            user.FirstName.Should().Be("John");
            user.LastName.Should().Be("Doe");
            user.Email.Should().Be("john.doe@gmail.com");
            user.DateOfBirth.Should().Be(new DateTime(1990, 1, 1));
        }
        
        [Theory]
        [InlineData("", "Doe", "john.doe@gmail.com", "firstName")]
        [InlineData("John", "", "john.doe@gmail.com", "lastName")]
        [InlineData("John", "Doe", "", "email")]
        public void UpdateProfile_WhenCalledWithEmptyParameter_ShouldThrowArgumentException(
            string firstName, string lastName, string email, string expectedParamName)
        {
            // Arrange
            var user = new User("091209121213");
            var dateOfBirth = new DateTime(1990, 1, 1);

            // Act
            Action act = () => user.UpdateProfile(firstName, lastName, email, dateOfBirth);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*string should not be empty.*")
                .And.ParamName.Should().Be(expectedParamName);
        }
    }
}