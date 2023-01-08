using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MailApp.Model;
using MailApp.Services;
using MailApp.Services.Interfaces;
using MailApp.View;
using MimeKit;


namespace MailApp.ViewModel;

[QueryProperty(nameof(EmailDetails), "EmailDetails")]
public partial class CreateEmailPageViewModel : BaseViewModel
{
    [ObservableProperty]  string to;
    [ObservableProperty]  string subject;
    [ObservableProperty]  string body;
    [ObservableProperty]  List<MimeEntity> attachments;
    [ObservableProperty]  ContentDisposition contentDisposition;
    [ObservableProperty]  EmailBody emailDetails;

    
    private CreateEmailService createEmailService;
    private readonly ICredentialService _credentialService;


    public CreateEmailPageViewModel(CreateEmailService createEmailService, ICredentialService credentialService)
    {
        _credentialService = credentialService;
        this.createEmailService = createEmailService;
    }
    
    [ICommand]
    public async Task SendEmail()
    {
        var credential =  _credentialService.GetCredentials();
        await createEmailService.SendEmailAsync(to, subject, body, attachments, credential);
        await Shell.Current.GoToAsync($"..");
    }
    
    [ICommand]
    public async Task AddAttachments()
    {
        Attachments = await createEmailService.AddAttachmentsAsync();
    }
    
    public void FillReplayEmail(EmailBody emailBody)
    {
        To = emailBody.Body.From.First().Name;
        Subject = "Reply: "+emailBody.Body.Subject;
        Body = emailBody.Body.TextBody;
    }
}