using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaAPP.Services;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Maui.Controls;


namespace TriviaAPP.ViewModels
{
    public class TriviaViewModel : INotifyPropertyChanged
    {
        JuegoHub hub = new();
        public string NombreUsuario { get; set; }

        public TriviaViewModel()
        {
            Iniciar();
            hub.Conectarse += Hub_Conectarse;
            hub.Iniciar += Hub_Iniciar;
        }


        private void Hub_Iniciar()
        {
        }

        private void Hub_Conectarse(string obj)
        {
            NombreUsuario = obj;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(NombreUsuario));
        }

        private async void Iniciar()
        {
            await hub.Conectar();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
