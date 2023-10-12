using AppInterface.Views.Mobile;

namespace AppInterface;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(FunctionsPage), typeof(FunctionsPage));
    }
}
