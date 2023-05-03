namespace TriviaAPP.Views;

public partial class Inicio : ContentPage
{
	public Inicio()
	{
		InitializeComponent();
        BindingContext = App.ViewModel;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var b1= btnIniciar.FadeTo(0, 1000, Easing.Linear);
        var b2= btnIniciar.ScaleTo(2.0, 1000, Easing.BounceIn);

        await Task.WhenAll(b1, b2);

        var b3 = btnIniciar.FadeTo(1, 1000, Easing.Linear);
        var b4 = btnIniciar.ScaleTo(1.0, 1000, Easing.BounceOut);

        await Task.WhenAll(b3, b4);
    }
}