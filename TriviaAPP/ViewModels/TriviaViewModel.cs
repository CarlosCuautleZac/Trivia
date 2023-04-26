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
        //Objetos
        JuegoHub hub = new();

        //Comandos


        //Propiedades
        public string NombreUsuario { get; set; } = "Espera";
        public bool Conection
        {
            get
            {
                Actualizar(nameof(Conection));
                return App.Conection;
            }
        }

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
            Actualizar(nameof(NombreUsuario));
            Actualizar();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NombreUsuario)));
        }

        private async void Iniciar()
        {
            await hub.Conectar();
        }
        public void Actualizar(string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(nombre)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
