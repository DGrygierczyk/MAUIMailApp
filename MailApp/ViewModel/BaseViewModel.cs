using CommunityToolkit.Mvvm.ComponentModel;

namespace MailApp.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty] 
    string username;
    [ObservableProperty]
    string password;
    [ObservableProperty]
    string smtpServer;
    [ObservableProperty]
    string imapServer;
    [ObservableProperty]
    int smtpPort;
    [ObservableProperty]
    int imapPort;
}