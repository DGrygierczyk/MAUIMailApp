using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
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
        _credentialService.SetCredentials(
            string.Empty, string.Empty, string.Empty,
            0, string.Empty, 0);
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
        _credentialService.SetCredentials(Username, Password, SmtpServer, SmtpPort, ImapServer, ImapPort);

        var veryfied = await emailService.VerifyCredentialsAsync(Username, Password, ImapServer, ImapPort);
        if (veryfied)
        {
            await Shell.Current.GoToAsync(nameof(MailboxPage));
            Username = string.Empty;
            Password = string.Empty;
        }
    }

    public async Task ClearCredentialsAsync()
    {
        _credentialService.SetCredentials("", "", "", 0, "", 0);
    }
}