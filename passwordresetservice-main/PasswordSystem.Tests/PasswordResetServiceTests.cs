namespace PasswordSystem.Tests;

public class PasswordResetServiceTests
{
    [Fact]
    public void SendResetEmail_SendsTokenToCorrectEmail()
    {
        // Arrange
        var fakeEmailProvider = new FakeEmailProvider();
        var fakeTokenGenerator = new FakeTokenGenerator();
        var sut = new PasswordResetService(fakeEmailProvider, fakeTokenGenerator);
        var testEmail = "test@test.com";
        sut.emailsInSystem.Add(testEmail);

        // Act
        sut.SendResetEmail(testEmail);

        // Assert
        var sentEmail = Assert.Single(fakeEmailProvider.SentEmails);
        Assert.Equal(testEmail, sentEmail.To);
        Assert.Contains(fakeTokenGenerator.Token, sentEmail.Body);
        Assert.Contains(testEmail, sut.emailsInSystem);
    }
       
       [Fact]
        public void SendResetPassword_Max_Three_Times()
    {
        // Arrange
        var fakeEmailProvider = new FakeEmailProvider();
        var fakeTokenGenerator = new FakeTokenGenerator();
        var sut = new PasswordResetService(fakeEmailProvider, fakeTokenGenerator);
        var testEmail = "test@test.com";
        sut.emailsInSystem.Add(testEmail);
        // Act
        sut.SendResetEmail(testEmail);
        sut.SendResetEmail(testEmail);
        sut.SendResetEmail(testEmail);

        // Assert
        Assert.Throws<InvalidOperationException>(() => sut.SendResetEmail(testEmail));
        Assert.Equal(3, fakeEmailProvider.SentEmails.Count);
    }   
}
