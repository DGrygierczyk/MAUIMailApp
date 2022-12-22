using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using MailApp.Services;

namespace MailApp.ViewModel;

public partial class MainPageViewModel : BaseViewModel
{
   private EmailService emailService;
   public MainPageViewModel(EmailService emailService)
   {
      this.emailService = emailService;
   }
   
   [RelayCommand]
   async Task LoginUserAsync()
   {
      try
      {
         var result = await emailService.VerifyCredentialsAsync("damiangrygierczyktest@gmail.com", "damiangrygierczyktest123");
         Debug.WriteLine(result);
      }
      catch (Exception e)
      {
         Debug.WriteLine(e);
         throw;
      }
   }
   // private async void Login()
   // {
   //    if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
   //    {
   //       // Display an error message
   //       return;
   //    }
   //
   //    var emailService = new EmailService();
   //    bool isValid = await emailService.VerifyCredentialsAsync(Username, Password);
   //
   //    if (isValid)
   //    {
   //       // Navigate to the main application window
   //    }
   //    else
   //    {
   //       // Display an error message
   //    }
   // }
}