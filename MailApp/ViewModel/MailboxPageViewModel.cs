using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MailApp.Model;
using MailApp.Services;
using MailKit;

namespace MailApp.ViewModel;

[QueryProperty(nameof(Username), "username")]
[QueryProperty(nameof(Password), "password")]
public partial class MailboxPageViewModel : BaseViewModel
{
    private string username;
    private string password;

    public ObservableCollection<EmailEnvelope> EmailEnvelopes { get; }= new();
    public string Username
    {
        get => username;
        set => SetProperty(ref username, value);
    }
    public string Password
    {
        get => password;
        set => SetProperty(ref password, value);
    }
    
    private EmailService emailService;

    public MailboxPageViewModel(EmailService emailService)
    {
        this.emailService = emailService;
    }

    [RelayCommand]
   public async Task FetchEmailsAsync()
    {
        var envelopes = await emailService.FetchAllEmailSummariesAsync(username, password);
        foreach (var envelope in envelopes)
        {
            EmailEnvelopes.Add(envelope);
        }
    }
}