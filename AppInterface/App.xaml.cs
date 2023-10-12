using AppInterface.Views.Desktop;
using AppInterface.Views.Mobile;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace AppInterface;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
#if ANDROID || IOS
        MainPage = new NavigationPage(new PrincipalPageMobile());
#else   
        MainPage = new NavigationPage(new PrincipalPageDesktop());
#endif
    }
}
