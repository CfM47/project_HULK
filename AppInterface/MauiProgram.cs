using AppInterface.ViewModel;
using AppInterface.Views.Mobile;
using Microsoft.Extensions.Logging;

namespace AppInterface;
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
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<DesktopMainViewModel>();
        builder.Services.AddSingleton<MobileMainViewModel>();

        builder.Services.AddTransient<FunctionsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
