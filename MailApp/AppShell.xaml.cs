using MailApp.View;

namespace MailApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(MailboxPage), typeof(MailboxPage));
		Routing.RegisterRoute(nameof(EmailDetailsPage), typeof(EmailDetailsPage));
	}
}

