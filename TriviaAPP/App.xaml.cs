using Plugin.Maui.Audio;
using TriviaAPP.ViewModels;
using TriviaAPP.Views;

namespace TriviaAPP
{
    public partial class App : Application
    {
        public static TriviaViewModel ViewModel { get; set; }
        public static bool Conection { get { return Connectivity.Current.NetworkAccess == NetworkAccess.Internet; } }
        public App(IAudioManager audioManager)
        {
            InitializeComponent();
            ViewModel = new(audioManager);
            //Routing.RegisterRoute("//Juego", typeof(JuegoView));

            MainPage = new AppShell();
        }
    }
}