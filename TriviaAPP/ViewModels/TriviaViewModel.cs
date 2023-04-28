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
        public Command VolverAlInicioCommand { get; set; }

        #endregion


        #region propiedades

        //Para el juego
        public PreguntaDTO Pregunta { get; set; }
        public Respuesta Respuesta { get; set; }
        public ObservableCollection<Jugador> Jugadores { get; set; } = new();

        //Usuario
        public string NombreUsuario { get; set; } = "Espera";
        public bool EsHost { get; set; } = false;

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
            VolverAlInicioCommand = new Command(VolverAlInicio);
        }

        private async void VolverAlInicio()
        {
            await Shell.Current.GoToAsync("//Main");
        }

        //Metodos que hacen los botones


        private async void IniciarJuego()
        {
            EsHost= true;
            await hub.Jugar();            
            Actualizar();
            await hub.IniciarJuego();                   
        }


        //Eventos


        private void Hub_Iniciar()
        {
            Ronda = 1;
            TiempoRestante = 15;
            Actualizar();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                //Shell.Current.Navigation.RemovePage(Shell.Current.Navigation.NavigationStack[0]);
                await Shell.Current.GoToAsync("//Juego");
            });

            TimerCliente = new Timer(async (state) =>
            {
                TiempoRestante -= 1;

                Actualizar();

                if (TiempoRestante == 1)
                {

                    if (EsHost)
                        await hub.Jugar();

                    //TiempoRestante = 15;
                    
                    if (Ronda >= limiterondas)
                    {
                        TimerCliente.Dispose();
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            await Shell.Current.GoToAsync("//FinDeJuego");
                        });
                    }
                    else
                    {
                        TiempoRestante = 15;
                    }

                    Ronda += 1;
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
