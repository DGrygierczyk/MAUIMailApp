namespace MailApp.Model;

public class ServerCredentials
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ImapServer { get; set; }
    public int ImapPort { get; set; }
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    
    public ServerCredentials(string username, string password, string imapServer, int imapPort, string smtpServer, int smtpPort)
    {
        Username = username;
        Password = password;
        ImapServer = imapServer;
        ImapPort = imapPort;
        SmtpServer = smtpServer;
        SmtpPort = smtpPort;
    }

    public ServerCredentials()
    {
        Username = null;
        Password = null;
        ImapServer = null;
        ImapPort = 0;
        SmtpServer = null;
        SmtpPort = 0;
    }
}
