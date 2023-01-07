using MailApp.Services;
using MailApp.Services.Interfaces;
using MailApp.View;
using MailApp.ViewModel;
using Microsoft.Extensions.Logging;

namespace MailApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<EmailService>();
		builder.Services.AddSingleton<CreateEmailService>();
		// builder.Services.AddSingleton<CredentialService>();
		builder.Services.AddSingleton<ICredentialService,CredentialService>();

		builder.Services.AddScoped<MainPageViewModel>();
		builder.Services.AddSingleton<MailboxPageViewModel>();
		builder.Services.AddTransient<EmailDetailsPageViewModel>();
		builder.Services.AddTransient<CreateEmailPageViewModel>();
		
		builder.Services.AddScoped<MainPage>();
		builder.Services.AddSingleton<MailboxPage>();
		builder.Services.AddTransient<EmailDetailsPage>();
		builder.Services.AddTransient<CreateEmailPage>();
		
		return builder.Build();
	}
}

