namespace PasswordSystem;

public class PasswordResetService(IEmailProvider emailProvider, ITokenGenerator tokenGenerator)
{
    private readonly IEmailProvider _emailProvider = emailProvider;
    private readonly ITokenGenerator _tokenGenerator = tokenGenerator;

    public List<String> emailsInSystem = new List<String>();

    public int Resetcount { get ; set ; } = 0;
    public void SendResetEmail(string email)
    {
        Resetcount ++;
        if (emailsInSystem.Contains(email))
        {
            if (Resetcount <= 3)
            {
            string subject = "Password Reset Requested";
            string token = _tokenGenerator.Generate();
            string body = CreateEmailBody(token);
            _emailProvider.SendEmail(email, subject, body);
            }
            else if (Resetcount > 3) {throw new InvalidOperationException("du har försökt för mycket");}
        }
        else throw new InvalidOperationException("du angav fel email adress");
    }
        
        

    private string CreateEmailBody(string token)
    {
        string link = $"https://example.com/reset?token={token}";
        return $"Click the following link to reset your password: {link}";
    }
}