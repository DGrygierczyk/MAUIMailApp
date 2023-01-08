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
        var myCollectionView = EmailsView;
        myCollectionView.ScrollTo(0);
    }

    private async void MenuItem_OnClicked(object sender, EventArgs e)
    {
        if (MenuGrid.IsVisible)
        {
            await MenuGrid.FadeTo(0);
            MenuGrid.IsVisible = false;
            MainContentGrid.IsVisible = true;
        }
        else
        {
            MainContentGrid.IsVisible = false;
            MenuGrid.IsVisible = true;
            await MenuGrid.FadeTo(1, 400, Easing.SinIn);
        }
    }
}