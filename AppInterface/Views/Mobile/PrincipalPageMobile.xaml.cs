using AppInterface.ViewModel;

namespace AppInterface.Views.Mobile;

public partial class PrincipalPageMobile : ContentPage
{
	public PrincipalPageMobile()
	{
        InitializeComponent();
        BindingContext = new MobileMainViewModel();
    }
    private void OnInputEntryCompleted(object sender, EventArgs e)
    {
        if (sender is Entry entry && entry.Text.Length > 0)
        {
            runButton.SendClicked();
            inputEntry.Focus();
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        MobileMainViewModel vm = BindingContext as MobileMainViewModel;
        await Navigation.PushAsync(new FunctionsPage(vm)); 
    }
}