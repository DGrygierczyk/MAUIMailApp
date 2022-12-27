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
      var RequiredOauth =  emailService.IsOauthSupported(Username);
      try
      {
         if (RequiredOauth)
         {
            // await emailService.VerifyOAuthCredentialsAsync(Username, Password);
            Debug.WriteLine("OAuth is supported");
         }
         else
         {
            await emailService.VerifyCredentialsAsync(Username, Password);
            await Shell.Current.GoToAsync($"{nameof(MailboxPage)}");
         }
      }
      catch (Exception e)
      {
         Debug.WriteLine(e);
         throw;
      }
   }
}