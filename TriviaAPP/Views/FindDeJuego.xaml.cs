namespace TriviaAPP.Views;

public partial class FindDeJuego : ContentPage
{
	public FindDeJuego()
	{
		InitializeComponent();
		BindingContext= App.ViewModel;
	}

    private async void btnAnim_Clicked(object sender, EventArgs e)
    {
        var b1 = btnAnim.FadeTo(0, 1000, Easing.Linear);
        var b2 = btnAnim.ScaleTo(2.0, 1000, Easing.BounceIn);

        await Task.WhenAll(b1, b2);

        var b3 = btnAnim.FadeTo(1, 1000, Easing.Linear);
        var b4 = btnAnim.ScaleTo(1.0, 1000, Easing.BounceOut);

        await Task.WhenAll(b3, b4);
    }
}