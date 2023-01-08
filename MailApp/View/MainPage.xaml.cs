using MailApp.ViewModel;

namespace MailApp;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((MainPageViewModel)BindingContext).ClearCredentialsAsync();
    }

    private void OnShowAdditionalFieldsSwitchToggled(object sender, ToggledEventArgs e)
    {
        additionalFieldsContainer.IsVisible = showAdditionalFieldsSwitch.IsToggled;
    }
}


