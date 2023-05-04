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
using Plugin.Maui.Audio;

namespace TriviaAPP.ViewModels
{
    public class TriviaViewModel : INotifyPropertyChanged
    {
        #region objetos
        JuegoHub hub = new();
        Timer Timer; //Este timer es para que el host envie el metodo de jugar cada cierto tiempo
        Timer TimerCliente; //Este timer es para darle a conocer el tiempo que tiene el cliente para responder
        Thread hilosonido;

        #endregion


        #region comandos

        public Command IniciarCommand { get; set; }
        public Command VolverAlInicioCommand { get; set; }
        public Command ResponderCommand { get; set; }

        #endregion


        #region propiedades

        //Para el juego
        public PreguntaDTO Pregunta { get; set; }
        public Respuesta Respuesta { get; set; }
        public ObservableCollection<Jugador> Jugadores { get; set; } = new();
        public bool Respondido { get; set; }
        public string Mensaje { get; set; } = "";
        private readonly string fin = "Assets/end.mp3";
        private readonly IAudioManager audioManager;
        private readonly IAudioManager audioManager2;

        //Usuario
        public string NombreUsuario { get; set; } = "Espera";
        public bool EsHost { get; set; } = false;
        Jugador Jugador = new Jugador();

        //Configuracion del juego
        int tiempo = 30;
        public int contador { get; set; }
        public int Ronda { get; set; } = 0;
        public int TiempoRestante { get; set; }

        public int TiempoRestanteSiguientePregunta;
        int limiterondas = 2;
        public bool Conection { get; set; } = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

        #endregion

        //Constructor

        public TriviaViewModel(IAudioManager audioManager)
        {
            Iniciar();
            hub.Conectarse += Hub_Conectarse;
            hub.Iniciar += Hub_Iniciar;
            hub.ActualizarLista += Hub_ActualizarLista;
            hub.ActualizarPregunta += Hub_ActualizarPregunta;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IniciarCommand = new Command(IniciarJuego);
            VolverAlInicioCommand = new Command(VolverAlInicio);
            ResponderCommand = new Command<string>(Reponder);
            TiempoRestante = tiempo;
            this.audioManager = audioManager;
            audioManager2 = audioManager;
            PlaySound("inicio.wav");
            PlayBackground();

        }

        private async void PlayAnswerSound()
        {
            Random r = new Random();
            var random = r.Next(1, 8);
            string sonidoelegido = random + ".wav";

            var file = await FileSystem.OpenAppPackageFileAsync(sonidoelegido);
            var AnswerSound = audioManager.CreatePlayer(file);
            AnswerSound.Volume = .3;
            AnswerSound.Play();
            AnswerSound.PlaybackEnded += AnswerSound_PlaybackEnded;
        }

        private void AnswerSound_PlaybackEnded(object sender, EventArgs e)
        {
            PlayBackground();
        }

        private async void PlayBackground()
        {

            Random r = new Random();
            var random = r.Next(11, 18);
            string sonidoelegido = random + ".wav";


            var file = await FileSystem.OpenAppPackageFileAsync(sonidoelegido);
            var background = audioManager2.CreatePlayer(file);
            background.Volume = .3;
            background.Play();
            background.PlaybackEnded += Background_PlaybackEnded;
        }

        private void Background_PlaybackEnded(object sender, EventArgs e)
        {
            PlayBackground();
        }

        private async void PlaySound(string sound)
        {
            var file = await FileSystem.OpenAppPackageFileAsync(sound);
            var player = audioManager.CreatePlayer(file);
            player.Volume = .1;
            player.Play();
        }

        private async void Reponder(string respuesta)
        {
            //Aqui modificas para que lo que esta entre parentesis te de 10

            PlayAnswerSound();

            if (respuesta == Pregunta.RespuestaCorrecta)
            {
                var puntos = Math.Round(((TiempoRestante / 3.0) * 100), 0);
                await hub.EnviarPuntos(puntos);
            }

            Respondido = true;

            Actualizar();
        }

        private async void VolverAlInicio()
        {
            EsHost = false;
            Actualizar();
            PlayBackground();
            await Shell.Current.GoToAsync("//Main");
        }

        //Metodos que hacen los botones


        private async void IniciarJuego()
        {

            EsHost = true;
            await hub.Jugar();
            Actualizar();
            await hub.IniciarJuego();
            PlayBackground();
        }


        //Eventos


        private async void Hub_Iniciar()
        {
            var file = await FileSystem.OpenAppPackageFileAsync("Start.wav");
            var background = audioManager.CreatePlayer(file);
            background.Volume = .1;
            background.Play();

            Ronda = 1;
            TiempoRestante = tiempo;
            Actualizar();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                //Shell.Current.Navigation.RemovePage(Shell.Current.Navigation.NavigationStack[0]);
                await Shell.Current.GoToAsync("//Juego");
            });

            TimerCliente = new Timer(async (state) =>
            {
                Mensaje = "";

                if (TiempoRestante > 0)
                {
                    TiempoRestante -= 1;

                }
                else
                {
                    Mensaje = "Esperando Jugadores";
                }
                Actualizar();

                if (TiempoRestante == 10 && Respondido == false)
                {
                    var file = await FileSystem.OpenAppPackageFileAsync("harryup.wav");
                    var background = audioManager.CreatePlayer(file);
                    background.Volume = .1;
                    background.Play();
                }

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
                            Jugadores = new(Jugadores.OrderByDescending(x => x.Puntos));
                            Actualizar();

                            if (Jugadores[0].ConnectionId == Jugador.ConnectionId)
                                PlaySound("win.wav");
                            else
                                PlaySound("gameover.wav");
                            await Shell.Current.GoToAsync("//FinDeJuego");
                            PlayBackground();

                        });
                    }
                    else
                    {
                        TiempoRestante = tiempo;
                        Actualizar();
                    }

                    Ronda += 1;
                    Actualizar();
                }


            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void Hub_ActualizarPregunta(PreguntaDTO p)
        {
            Respondido = false;
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

            //Le a;adimos lo puntos del jugador- esto solo se hara cuando esten jugando

            if (Jugador.ConnectionId != "")
            {
                var puntos = Jugadores.FirstOrDefault(x => x.ConnectionId == Jugador.ConnectionId).Puntos;
                Jugador.Puntos = puntos;
            }

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
