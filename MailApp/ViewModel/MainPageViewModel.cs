using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using MailApp.Model;
using MailApp.Services;
using MailApp.Services.Interfaces;
using MailApp.View;

namespace MailApp.ViewModel;

public partial class MainPageViewModel : BaseViewModel
{
    private EmailService emailService;
    private ICredentialService _credentialService;

    public MainPageViewModel(EmailService emailService, ICredentialService credentialService)
    {
        this.emailService = emailService;
        _credentialService = credentialService;
        _credentialService.SetCredentials(new ServerCredentials());
    }

    [ICommand]
    async Task LoginUserAsync()
    {
        //TODO: DO WYWALENIA
        Username = "inzynierka2022grygierczyk@wp.pl";
        Password = "zxczxczxc1";
        SmtpServer = "smtp.wp.pl";
        SmtpPort = 465;
        ImapServer = "imap.wp.pl";
        ImapPort = 993;
        ServerCredentials credentials = new ServerCredentials(Username, Password, ImapServer, ImapPort, SmtpServer, SmtpPort);
        _credentialService.SetCredentials(credentials);

        var veryfied = await emailService.VerifyCredentialsAsync(credentials);
        if (veryfied)
        {
            await Shell.Current.GoToAsync(nameof(MailboxPage));
            Username = string.Empty;
            Password = string.Empty;
        }
    }

    public async Task ClearCredentialsAsync()
    {
        _credentialService.SetCredentials(new ServerCredentials());
    }
}