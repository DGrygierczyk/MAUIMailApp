using MailApp.Services;
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

		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MailboxPageViewModel>();
		
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MailboxPage>();
		
		return builder.Build();
	}
}

