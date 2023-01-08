using MailApp.ViewModel;

namespace MailApp.View;

public partial class CreateEmailPage : ContentPage
{
    public CreateEmailPage(CreateEmailPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing(); 
        if (((CreateEmailPageViewModel)BindingContext).EmailDetails != null)
        {
            ((CreateEmailPageViewModel)BindingContext).FillReplayEmail(((CreateEmailPageViewModel)BindingContext).EmailDetails);
        }
    }

    
}