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
        webView.Navigated += (sender, args) =>
        {
            webView.Eval("document.body.style.fontSize='100px'");
        };
    }
    

    private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
    {
    }
    
}