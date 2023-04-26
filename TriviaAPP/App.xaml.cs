using TriviaAPP.ViewModels;

namespace TriviaAPP
{
    public partial class App : Application
    {
        //public static TriviaViewModel ViewModel { get; set; } = new();
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}