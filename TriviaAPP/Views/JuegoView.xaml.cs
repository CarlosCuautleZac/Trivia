namespace TriviaAPP.Views;

public partial class JuegoView : ContentPage
{
	public JuegoView()
	{
		InitializeComponent();
		BindingContext = App.ViewModel;
	}
}