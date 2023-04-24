using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaAPP.Services;

namespace TriviaAPP.ViewModels
{
    public class TriviaViewModel : INotifyPropertyChanged
    {
        JuegoHub hub = new();

        public TriviaViewModel()
        {
            Iniciar();
        }

        private async void Iniciar()
        {
           await hub.Conectar();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
