using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Button = Microsoft.Maui.Controls.Button;

namespace TriviaAPP.Views;

public partial class JuegoView : ContentPage
{
	public JuegoView()
	{
		InitializeComponent();
		BindingContext = App.ViewModel;
	}

    private async void btnRespuesta_Clicked(object sender, EventArgs e)
    {
        //var collectionView = this.FindByName<CollectionView>("CVdeResp");
        //var btnResp = collectionView.FindByName<Button>("btnRespuesta");
        var b1 = CVdeResp.FadeTo(1.0, 1000, Easing.Linear);
        var b2 = CVdeResp.ScaleTo(2.0, 1000, Easing.BounceIn);

        await Task.WhenAll(b1, b2);

        var b3 = CVdeResp.FadeTo(1, 1000, Easing.Linear);
        var b4 = CVdeResp.ScaleTo(1.0, 1000, Easing.BounceOut);

        await Task.WhenAll(b3, b4);

    }

    //private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    //{
    //    //var collectionView = this.FindByName<CollectionView>("CVdeResp");
    //    //var btnResp = collectionView.FindByName<Button>("btnRespuesta");
    //    var b1 = CVdeResp.FadeTo(1.0, 1000, Easing.Linear);
    //    var b2 = CVdeResp.ScaleTo(2.0, 1000, Easing.BounceIn);

    //    await Task.WhenAll(b1, b2);

    //    var b3 = CVdeResp.FadeTo(1, 1000, Easing.Linear);
    //    var b4 = CVdeResp.ScaleTo(1.0, 1000, Easing.BounceOut);

    //    await Task.WhenAll(b3, b4);
    //}
}