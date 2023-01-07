using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MailApp.Model;
using MailApp.Services;
using MailApp.View;
using MailKit;
using MailKit.Search;
using MimeKit;

namespace MailApp.ViewModel;

[QueryProperty(nameof(Username), "username")]
[QueryProperty(nameof(Password), "password")]
public partial class MailboxPageViewModel : BaseViewModel
{
    private string username;
    private string password;

    public ObservableCollection<EmailEnvelope> EmailEnvelopes { get; } = new();

    [ObservableProperty] IList<IMailFolder> emailFolders;

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
    [ObservableProperty] ObservableCollection<string> name = new();

    private EmailService emailService;

    public MailboxPageViewModel(EmailService emailService)
    {
        this.emailService = emailService;
    }

    [ICommand]
    public async Task FetchEmailsAsync(string selectedFolder = "Inbox")
    {
        EmailEnvelopes.Clear();

        var envelopes = await emailService.FetchAllEmailSummariesAsync(username, password, selectedFolder);

        foreach (var envelope in envelopes)
        {
            EmailEnvelopes.Insert(0, envelope);
        }
        
        Name.Clear();
        var folders = await emailService.GetFoldersAsync(username, password);
        foreach (var folder in folders)
        {
            Name.Add(folder.Name);
        } 
    }

    [ICommand]
    public async Task GoToEmailAsync(EmailEnvelope envelope)
    {
        List<MimeEntity> attachments = new();
        var fetchedEmail = await emailService.FetchEmailAsync(username, password, envelope.Id);

        foreach (var attachment in fetchedEmail.Attachments)
        {
            attachments.Add(attachment);
        }

        var emailDetails = new EmailBody
        {
            Body = fetchedEmail,
            Attachments = attachments
        };

        await Shell.Current.GoToAsync($"{nameof(EmailDetailsPage)}", true, new Dictionary<string, object>
        {
            { "EmailDetails", emailDetails }
        });
    }

    [ICommand]
    public async Task CreateEmailAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(CreateEmailPage)}?username={Username}&password={Password}", true);
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

        var envelopes = await emailService.SearchEmailsAsync(username, password, searchEmailQuery);
        foreach (var envelope in envelopes)
        {
            EmailEnvelopes.Insert(0, envelope);
        }
    }

    [ICommand]
    public async Task DeleteEmailAsync(EmailEnvelope envelope)
    {
        var result = await Application.Current.MainPage.DisplayAlert("Confirm",
            "Are you sure you want to delete this email?", "Yes", "No");
        if (result)
        {
            await emailService.DeleteEmailAsync(username, password, envelope.Id);
            EmailEnvelopes.Remove(envelope);
        }
    }
}