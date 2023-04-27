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
using static System.Runtime.InteropServices.JavaScript.JSType;
using TriviaAPP.Models;
using System.Collections.ObjectModel;

namespace TriviaAPP.ViewModels
{
    public class TriviaViewModel : INotifyPropertyChanged
    {
        #region objetos
        JuegoHub hub = new();
        Jugador Jugador = new Jugador();
        #endregion


        #region comandos

        #endregion


        #region propiedades

        public ObservableCollection<Jugador> Jugadores { get; set; } = new();

        public string NombreUsuario { get; set; } = "Espera";

        public bool Conection { get; set; } = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;


        //public bool Conection
        //{
        //    get
        //    {
        //        Actualizar(nameof(Conection));
        //        return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        //    }
        //}
        #endregion

        //Constructor

        public TriviaViewModel()
        {
            Iniciar();
            hub.Conectarse += Hub_Conectarse;
            hub.Iniciar += Hub_Iniciar;
            hub.ActualizarLista += Hub_ActualizarLista;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Hub_ActualizarLista(List<Jugador> jugadoresactualizados)
        {
            ////Verificiar
            ////Si no estaba, que lo agregue
            //foreach (var jugador in jugadoresactualizados)
            //{
            //    if(!Jugadores.Contains(jugador))
            //        Jugadores.Add(jugador);
            //}

            ////Si ya no esta, que lo quite

            //foreach (var jugador in Jugadores)
            //{
            //    if (!jugadoresactualizados.Contains(jugador))
            //        Jugadores.Remove(jugador);
            //}

            Jugadores = new(jugadoresactualizados);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Jugadores)));
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {

            if(Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                Conection = true;
            else
                Conection = false;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Conection)));
        }

        private void Hub_Iniciar()
        {
        }

        private void Hub_Conectarse(Jugador jugador)
        {
            NombreUsuario = jugador.NombreUsuario;
            Jugador = jugador;
            Actualizar(nameof(NombreUsuario));
            Actualizar();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NombreUsuario)));
        }

        private async void Iniciar()
        {
            if (Conection)
                await hub.Conectar();
        }
        public void Actualizar(string nombre = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(nombre)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
