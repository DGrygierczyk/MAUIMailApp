using CommunityToolkit.Mvvm.ComponentModel;
using MailApp.Model;

namespace MailApp.ViewModel;

[QueryProperty(nameof(EmailDetails), "EmailDetails")]
public partial class EmailDetailsPageViewModel : BaseViewModel
{
     [ObservableProperty]
     EmailBody emailDetails;
    // private EmailBody emailDetails;
    // public EmailBody EmailDetails
    // {
    //     get => emailDetails;
    //     set => SetProperty(ref emailDetails, value);
    // }
    public EmailDetailsPageViewModel()
    {
    }
}