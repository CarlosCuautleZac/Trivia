using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaAPP.Services
{
    public class JuegoHub
    {
        HttpClient client = new HttpClient();
        HubConnection connection;

        public JuegoHub()
        {
            connection = new HubConnectionBuilder().WithUrl("https://localhost:7096/triviaHub").Build();      
            
        }

        public async Task Algo()
        {
            connection.On("Conectado", () =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert("uwu", "uwu", "OK");
                });

            });
        }

        public async Task Conectar()
        {
            await connection.StartAsync();
        }
        public async Task Jugar()
        {
            var result = await client.GetAsync("https://localhost:7096/api/trivia/jugar");
            result.EnsureSuccessStatusCode();
        }
    }
}
