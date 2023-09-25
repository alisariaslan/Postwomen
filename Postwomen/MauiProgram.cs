using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Postwomen.Handlers;
using Postwomen.Interfaces;
using Postwomen.Others;
using Postwomen.ViewModels;
using Postwomen.Views;
using Refit;

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

		builder.Services.AddRefitClient<IApi>().ConfigurePrimaryHttpMessageHandler(() => new HttpLoggingHandler());

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<PostwomenDatabase>();
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddTransient<EditCardPage>();
        builder.Services.AddTransient<LogsPage>();

        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<SettingsViewModel>();


        builder.Services.AddSingleton<IMessenger, WeakReferenceMessenger>();
        builder.Services.AddTransient<ServiceTestPage>();

        return builder.Build();
	}
}
