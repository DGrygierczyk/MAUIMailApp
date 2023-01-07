using MailApp.Services;
using MailApp.ViewModel;

namespace MailApp;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		 // viewModel.ClearCredentialsAsync();
	}
	
	protected override async void OnAppearing()
	{
		base.OnAppearing(); 
		await ((MainPageViewModel)BindingContext).ClearCredentialsAsync();
	}
}


