using CommunityToolkit.Maui;
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
	public static ServiceProvider MyServiceProvider { get; private set; }
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
		builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddTransient<EditCard>();

		MyServiceProvider = builder.Services.BuildServiceProvider();

		return builder.Build();
	}
}
