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
        _credentialService.SetCredentials(string.Empty, string.Empty);
    }

    [ICommand]
    async Task LoginUserAsync()
    {
        //TODO: DO WYWALENIA
        // Username = "inzynierka2022grygierczyk@wp.pl";
        // Password = "zxczxczxc1";

        var veryfied = await emailService.VerifyCredentialsAsync(Username, Password);
        if (veryfied)
        {
            _credentialService.SetCredentials(Username, Password);
            await Shell.Current.GoToAsync(nameof(MailboxPage));
            Username = string.Empty;
            Password = string.Empty;
        }
    }

    public async Task ClearCredentialsAsync()
    {
        _credentialService.SetCredentials("", "");
    }
}