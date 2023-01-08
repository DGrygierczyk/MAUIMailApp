using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MailApp.Model;
using MailApp.Services;
using MailApp.Services.Interfaces;
using MailApp.View;
using MailKit;
using MailKit.Search;
using MimeKit;

namespace MailApp.ViewModel;

public partial class MailboxPageViewModel : BaseViewModel
{
    // [ObservableProperty] string username;
    // [ObservableProperty] string password;
    public ObservableCollection<EmailEnvelope> EmailEnvelopes { get; } = new();

    [ObservableProperty] IList<IMailFolder> emailFolders;
    [ObservableProperty] string searchEmailQuery;
    [ObservableProperty] ObservableCollection<string> name = new();

    private EmailService emailService;
    private readonly ICredentialService _credentialService;

    public MailboxPageViewModel(EmailService emailService, ICredentialService credentialService)
    {
        _credentialService = credentialService;
        this.emailService = emailService;
    }

    [ICommand]
    public async Task FetchEmailsAsync(string selectedFolder = "Inbox")
    {
        EmailEnvelopes.Clear();
        var credentials = _credentialService.GetCredentials();
        var envelopes = await emailService.FetchAllEmailSummariesAsync(credentials, selectedFolder);

        foreach (var envelope in envelopes)
        {
            EmailEnvelopes.Insert(0, envelope);
        }
        
        Name.Clear();
        var folders = await emailService.GetFoldersAsync(credentials);
        foreach (var folder in folders)
        {
            Name.Add(folder.Name);
        } 
    }

    [ICommand]
    public async Task GoToEmailAsync(EmailEnvelope envelope)
    {
        var credentials = _credentialService.GetCredentials();
        List<MimeEntity> attachments = new();
        var fetchedEmail = await emailService.FetchEmailAsync(credentials, envelope.Id);

        foreach (var attachment in fetchedEmail.Attachments)
        {
            attachments.Add(attachment);
        }
        var emailDetails = new EmailBody
        {
            Body = fetchedEmail,
            Attachments = attachments,
            HtmlBody = fetchedEmail.HtmlBody+
            "<script>document.body.style.fontSize='3em'</script>" +
            "<style>body{color: white; background-color: black;}</style>"
        };

        await Shell.Current.GoToAsync($"{nameof(EmailDetailsPage)}", true, new Dictionary<string, object>
        {
            { "EmailDetails", emailDetails }
        });
    }

    [ICommand]
    public async Task CreateEmailAsync()
    {
        await Shell.Current.GoToAsync(nameof(CreateEmailPage), true);
    }

    [ICommand]
    public async Task SearchEmailsAsync()
    {
        var credentials = _credentialService.GetCredentials();
        EmailEnvelopes.Clear();
        if (string.IsNullOrEmpty(searchEmailQuery))
        {
            await FetchEmailsAsync();
            return;
        }

        var envelopes = await emailService.SearchEmailsAsync(credentials, searchEmailQuery);
        foreach (var envelope in envelopes)
        {
            EmailEnvelopes.Insert(0, envelope);
        }
    }

    [ICommand]
    public async Task DeleteEmailAsync(EmailEnvelope envelope)
    {
        var credentials = _credentialService.GetCredentials();
        var result = await Application.Current.MainPage.DisplayAlert("Confirm",
            "Are you sure you want to delete this email?", "Yes", "No");
        if (result)
        {
            await emailService.DeleteEmailAsync(credentials, envelope.Id);
            EmailEnvelopes.Remove(envelope);
        }
    }
}