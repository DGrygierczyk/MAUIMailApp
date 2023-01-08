using MailApp.ViewModel;

namespace MailApp.View;

public partial class EmailDetailsPage : ContentPage
{
    public EmailDetailsPage(EmailDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        var x = ((EmailDetailsPageViewModel)BindingContext).EmailDetails;
    }
}