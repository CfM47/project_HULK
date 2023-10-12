using AppInterface.ViewModel;
namespace AppInterface.Views.Mobile;

public partial class FunctionsPage : ContentPage
{
	public FunctionsPage(MobileMainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}