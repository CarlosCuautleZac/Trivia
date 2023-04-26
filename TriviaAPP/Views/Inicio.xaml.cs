namespace TriviaAPP.Views;

public partial class Inicio : ContentPage
{
	public Inicio()
	{
		InitializeComponent();
        BindingContext = App.ViewModel;
    }
}