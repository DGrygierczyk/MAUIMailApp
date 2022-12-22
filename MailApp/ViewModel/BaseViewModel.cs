using CommunityToolkit.Mvvm.ComponentModel;

namespace MailApp.ViewModel;

public partial class BaseViewModel: ObservableObject
{
    [ObservableProperty] string username;
    [ObservableProperty] string password;
}