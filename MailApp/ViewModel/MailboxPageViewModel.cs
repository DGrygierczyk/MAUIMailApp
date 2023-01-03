using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MailApp.Model;
using MailApp.Services;
using MailApp.View;
using MailKit;
using MailKit.Search;

namespace MailApp.ViewModel;

[QueryProperty(nameof(Username), "username")]
[QueryProperty(nameof(Password), "password")]
public partial class MailboxPageViewModel : BaseViewModel
{
    private string username;
    private string password;

    public ObservableCollection<EmailEnvelope> EmailEnvelopes { get; } = new();
    
    [ObservableProperty]  
    IList<IMailFolder> emailFolders;
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

    [ObservableProperty] string searchEmailQuery;    

    private EmailService emailService;

    public MailboxPageViewModel(EmailService emailService)
    {
        this.emailService = emailService;
    }

    [ICommand]
    public async Task FetchEmailsAsync()
    {
        EmailEnvelopes.Clear();
        var envelopes = await emailService.FetchAllEmailSummariesAsync(username, password);
        foreach (var envelope in envelopes)
        {
            EmailEnvelopes.Insert(0, envelope);
        }
        EmailFolders = await emailService.GetFoldersAsync(username, password);
    }

    [ICommand]
    public async Task GoToEmailAsync(EmailEnvelope envelope)
    {
        var fetchedEmail = await emailService.FetchEmailAsync(username, password, envelope.Id);
        var emailDetails = new EmailBody { Body = fetchedEmail };
        await Shell.Current.GoToAsync($"{nameof(EmailDetailsPage)}", true, new Dictionary<string, object>
        {
            { "EmailDetails", emailDetails }
        });
    }
    
    [ICommand]
    public async Task CreateEmailAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(CreateEmailPage)}", true);
    }
    
    [ICommand]
    public async Task SearchEmailsAsync()
    {
        EmailEnvelopes.Clear();
        if (string.IsNullOrEmpty(searchEmailQuery))
        {
            await FetchEmailsAsync();
            return;
        }
        // SearchQuery searchQuery = SearchQuery.SubjectContains(searchEmailQuery);
        var envelopes = await emailService.SearchEmailsAsync(username, password, searchEmailQuery);
        foreach (var envelope in envelopes)
        {
            EmailEnvelopes.Insert(0, envelope);
        }
    }
    
    [ICommand]
    public async Task DeleteEmailAsync(EmailEnvelope envelope)
    {
        var result = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this email?", "Yes", "No");
        if (result)
        {
            await emailService.DeleteEmailAsync(username, password, envelope.Id);
            EmailEnvelopes.Remove(envelope);
        }
    }
}