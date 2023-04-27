using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaAPP.Models;

namespace TriviaAPP.Services
{
    public class JuegoHub
    {
        HttpClient client;
        HubConnection connection;
        public event Action<Jugador> Conectarse;
        public event Action Iniciar;
        Jugador jugador = new();
        

        string url = "https://longaquarock52.conveyor.cloud/";
        public List<Jugador> Jugadores = new List<Jugador>();


        public JuegoHub()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };

            connection = new HubConnectionBuilder().WithUrl($"{url}triviaHub").Build();

            //connection.On("Conectado", () =>
            //{
            //    //Conectarse?.Invoke(connection.ConnectionId);
            //});


            connection.On("iniciar", () =>
            {
                Iniciar?.Invoke();

            });

            connection.On<PreguntaDTO>("recibirpregunta", (preguntaDTO) =>
            {
                var p = preguntaDTO;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert("uwu", "Juego Iniciado", "OK");
                });

            });

            connection.On<List<Jugador>>("NewConnection", (jugadores) =>
            {
                var p = jugadores;

            });

            connection.On<List<Jugador>>("DisconnectedConnection", (jugadores) =>
            {
                var p = jugadores;

            });

            connection.On<Jugador>("Datos", (j) =>
            {
                jugador = j;
                Conectarse?.Invoke(jugador);

            });

        }


        public async Task Conectar()
        {
            await connection.StartAsync();
            //var result = await client.GetAsync("https://localhost:7096/api/trivia/jugar");
            //result.EnsureSuccessStatusCode();
        }
        public async Task Jugar()
        {
            var result = await client.GetAsync("api/jugar");
            result.EnsureSuccessStatusCode();
        }


    }
}
