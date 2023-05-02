using Plugin.Maui.Audio;
using TriviaAPP.ViewModels;
using TriviaAPP.Views;

namespace TriviaAPP
{
    public partial class App : Application
    {
        private static readonly IAudioManager audioManager;
        public static TriviaViewModel ViewModel { get; set; } = new(audioManager);
        public static bool Conection { get { return Connectivity.Current.NetworkAccess == NetworkAccess.Internet; } }
        public App()
        {
            InitializeComponent();

            //Routing.RegisterRoute("//Juego", typeof(JuegoView));

            MainPage = new AppShell();
        }
    }
}