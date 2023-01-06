using MailApp.ViewModel;

namespace MailApp.View;

public partial class CreateEmailPage : ContentPage
{
    public CreateEmailPage(CreateEmailPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    
}