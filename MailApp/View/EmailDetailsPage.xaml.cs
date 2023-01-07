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
        // webView.Navigated += (sender, args) =>
        // {
            // webView.EvaluateJavaScriptAsync("document.body.style.fontSize='100px'");
        // };
    }
    
    protected override  void OnAppearing()
    {
        base.OnAppearing();
        webView.EvaluateJavaScriptAsync("document.body.style.fontSize='100px'");
        // await webView.EvaluateJavaScriptAsync("document.body.style.fontSize='100px'");

    }
}