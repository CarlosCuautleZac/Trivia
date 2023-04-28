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
        Timer Timer; //Este timer es para que el host envie el metodo de jugar cada cierto tiempo
        Timer TimerCliente; //Este timer es para darle a conocer el tiempo que tiene el cliente para responder
        #endregion


        #region comandos

        public Command IniciarCommand { get; set; }

        #endregion


        #region propiedades

        //Para el juego
        public PreguntaDTO Pregunta { get; set; }
        public ObservableCollection<Jugador> Jugadores { get; set; } = new();

        //Usuario
        public string NombreUsuario { get; set; } = "Espera";
       

        //Configuracion del juego
        public int contador { get; set; }
        public int Ronda { get; set; } = 0;
        public int TiempoRestante { get; set; } = 15;

        public int TiempoRestanteSiguientePregunta;
        int limiterondas = 2;
        public bool Conection { get; set; } = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

        #endregion

        //Constructor

        public TriviaViewModel()
        {
            Iniciar();
            hub.Conectarse += Hub_Conectarse;
            hub.Iniciar += Hub_Iniciar;
            hub.ActualizarLista += Hub_ActualizarLista;
            hub.ActualizarPregunta += Hub_ActualizarPregunta;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IniciarCommand = new Command(IniciarJuego);
        }

        //Metodos que hacen los botones

       
        private async void IniciarJuego()
        {
            await hub.IniciarJuego();
            await hub.Jugar();
            Timer = new Timer(async (state) =>
            {

                TiempoRestanteSiguientePregunta = TiempoRestante;
                
                Actualizar();

                if (TiempoRestanteSiguientePregunta == 1)
                {
                    if (Ronda >= limiterondas)
                        Timer.Dispose();
                    else
                    {
                        await hub.Jugar();
                    }

                }
              


            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        }


        //Eventos


        private async void Hub_Iniciar()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("//Juego");
            });

            TimerCliente = new Timer((state) =>
            {
                TiempoRestante -= 1;

                Actualizar();

                if (TiempoRestante == 0)
                {
                    //TiempoRestante = 15;
                    Ronda += 1;
                    if (Ronda >= limiterondas)
                    {
                        TimerCliente.Dispose();
                    }
                    else
                    {
                        TiempoRestante = 15;
                    }
                }


            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void Hub_ActualizarPregunta(PreguntaDTO p)
        {
            Pregunta = p;
            Actualizar();
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
            Actualizar();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Jugadores)));
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {

            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                Conection = true;
            else
                Conection = false;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Conection)));
        }


        private void Hub_Conectarse(Jugador jugador)
        {
            NombreUsuario = jugador.NombreUsuario;
            Jugador = jugador;
            Actualizar(nameof(NombreUsuario));
            Actualizar();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NombreUsuario)));
        }


        //Al iniciar el juego
        private async void Iniciar()
        {
            if (Conection)
                await hub.Conectar();
        }
        public void Actualizar(string nombre = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
