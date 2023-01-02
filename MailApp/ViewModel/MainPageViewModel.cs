using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using MailApp.Services;
using MailApp.View;

namespace MailApp.ViewModel;

public partial class MainPageViewModel : BaseViewModel
{
    private EmailService emailService;

    public MainPageViewModel(EmailService emailService)
    {
        this.emailService = emailService;
    }

    [ICommand]
    async Task LoginUserAsync()
    {
    //TODO: DO WYWALENIA
    Username = "inzynierka2022grygierczyk@wp.pl";
    Password = "zxczxczxc1";
    var RequiredOauth = emailService.IsOauthSupported(Username);
        try
        {
            if (RequiredOauth)
            {
                // await emailService.VerifyOAuthCredentialsAsync(Username, Password);
                Debug.WriteLine("OAuth is supported");
            }
            else
            {
                var veryfied = await emailService.VerifyCredentialsAsync(Username, Password);
                if (veryfied)
                {
                    //navigate to page and pass username and password
                    await Shell.Current.GoToAsync(
                        $"{nameof(MailboxPage)}?username={Username}&password={Password}");
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
    }
}