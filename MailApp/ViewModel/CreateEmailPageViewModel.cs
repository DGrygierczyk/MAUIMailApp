using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MailApp.Services;
using MailApp.View;
using MimeKit;


namespace MailApp.ViewModel;

[QueryProperty(nameof(Username), "username")]
[QueryProperty(nameof(Password), "password")]
public partial class CreateEmailPageViewModel : BaseViewModel
{
    [ObservableProperty]  string to;
    [ObservableProperty]  string subject;
    [ObservableProperty]  string body;
    [ObservableProperty]  List<MimeEntity> attachments;
    [ObservableProperty]  ContentDisposition contentDisposition;
    private string username;
    private string password;
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
    
    private CreateEmailService createEmailService;
    
    public CreateEmailPageViewModel(CreateEmailService createEmailService)
    {
        this.createEmailService = createEmailService;
    }
    
    [ICommand]
    public async Task SendEmail()
    {
        await createEmailService.SendEmailAsync(to, subject, body, username, password, attachments);
        await Shell.Current.GoToAsync($"..");
    }
    
    [ICommand]
    public async Task AddAttachments()
    {
        Attachments = await createEmailService.AddAttachmentsAsync();
    }
}