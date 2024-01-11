using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Postwomen.Handlers;
using Postwomen.Interfaces;
using Postwomen.Views;
using Refit;
using DesenMobileDatabase;
using Postwomen.Services;
using Postwomen.Extensions;

namespace Postwomen;

public static class MauiProgram
{

	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddRefitClient<IMainApi>().ConfigureHttpClient(c =>
        {
            c.Timeout = TimeSpan.FromSeconds(30);
#if DEBUG
            c.BaseAddress = new Uri("http://10.0.2.2:5000");
#else
			c.BaseAddress = new Uri("https://api.alisariaslan.com.tr");
#endif
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpLoggingHandler());

        builder.Services.AddSingleton<MobileDB>();
        builder.Services.AddTransient<IDbService,DbService>();

		builder.Services.AddTransient<EditCardPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<LogsPage>();
        builder.Services.AddTransient<VersionPopup>();

		builder.Services.AddSingleton<Translator>();

		builder.Services.AddSingleton<MainPage>();

        return builder.Build();
	}
}
