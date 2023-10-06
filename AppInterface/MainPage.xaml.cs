using AppInterface.ViewModel;

namespace AppInterface;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
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

