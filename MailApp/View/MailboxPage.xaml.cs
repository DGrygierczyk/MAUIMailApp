using MailApp.ViewModel;

namespace MailApp.View;

public partial class MailboxPage : ContentPage
{
    public MailboxPage(MailboxPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((MailboxPageViewModel)BindingContext).FetchEmailsAsync();
    }

}