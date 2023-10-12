using AppInterface.ViewModel;
namespace AppInterface.Views.Desktop;

public partial class PrincipalPageDesktop : ContentPage
{
	public PrincipalPageDesktop()
	{
        InitializeComponent();
        BindingContext = new DesktopMainViewModel();
    }
    private void OnInputEntryCompleted(object sender, EventArgs e)
    {
        if (sender is Entry entry && entry.Text.Length > 0)
        {
            runButton.SendClicked();
            inputEntry.Focus();
        }
    }
}