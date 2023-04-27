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
        public event Action<string> Conectarse;
        public event Action Iniciar;

        string url = "https://nextpurpleapple59.conveyor.cloud/";

        public JuegoHub()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };

            connection = new HubConnectionBuilder().WithUrl($"{url}triviaHub").Build();

            connection.On("Conectado", () =>
            {
                Conectarse?.Invoke(connection.ConnectionId);
            });


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

        }


        public async Task Conectar()
        {
            await connection.StartAsync();
            Conectarse?.Invoke(connection.ConnectionId);
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
