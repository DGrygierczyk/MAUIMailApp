using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailApp.ViewModel;

namespace MailApp.View;

public partial class EmailDetailsPage : ContentPage
{
    public EmailDetailsPage(EmailDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}