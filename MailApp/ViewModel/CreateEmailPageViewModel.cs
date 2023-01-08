using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MailApp.Services;
using MailApp.Services.Interfaces;
using MailApp.View;
using MimeKit;


namespace MailApp.ViewModel;

public partial class CreateEmailPageViewModel : BaseViewModel
{
    [ObservableProperty]  string to;
    [ObservableProperty]  string subject;
    [ObservableProperty]  string body;
    [ObservableProperty]  List<MimeEntity> attachments;
    [ObservableProperty]  ContentDisposition contentDisposition;

    
    private CreateEmailService createEmailService;
    private readonly ICredentialService _credentialService;


    public CreateEmailPageViewModel(CreateEmailService createEmailService, ICredentialService credentialService)
    {
        _credentialService = credentialService;
        this.createEmailService = createEmailService;
        (Username, Password, SmtpServer, SmtpPort, ImapServer, ImapPort) = _credentialService.GetCredentials();
    }
    
    [ICommand]
    public async Task SendEmail()
    {
        await createEmailService.SendEmailAsync(to, subject, body, Username, Password, attachments, ImapServer, ImapPort, SmtpServer, SmtpPort);
        await Shell.Current.GoToAsync($"..");
    }
    
    [ICommand]
    public async Task AddAttachments()
    {
        Attachments = await createEmailService.AddAttachmentsAsync();
    }
}