using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaAPP.Models;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace TriviaAPP.Services
{
    public class JuegoHub
    {
        HttpClient client;
        HubConnection connection;
        public event Action<Jugador> Conectarse;
        public event Action Iniciar;
        Jugador jugador = new();
        public event Action<List<Jugador>> ActualizarLista;
        public event Action<PreguntaDTO> ActualizarPregunta;
        

        string url = "https://triviamario.sistemas19.com/";
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

            //Datos del usuario cuando se conecta, esto es para su nombre de usurio
            connection.On<Jugador>("Datos", (j) =>
            {
                jugador = j;
                Conectarse?.Invoke(jugador);

            });

            connection.On<PreguntaDTO>("recibirpregunta", (preguntaDTO) =>
            {
                ActualizarPregunta?.Invoke(preguntaDTO);

            });

            //Cada vez que hay una nueva conexion
            connection.On<List<Jugador>>("NewConnection", (jugadores) =>
            {
                ActualizarLista?.Invoke(jugadores);

            });

            //Cada vez que se desconecte una conexion
            connection.On<List<Jugador>>("DisconnectedConnection", (jugadores) =>
            {
                ActualizarLista?.Invoke(jugadores);

            });

            

            //Cada vez que se mande una puntuacion
            connection.On<List<Jugador>>("ActualizarLista", (jugadores) =>
            {
                ActualizarLista?.Invoke(jugadores);
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
            var result = await client.GetAsync("api/trivia/jugar");
            result.EnsureSuccessStatusCode();
        }

        public async Task IniciarJuego()
        {
            //var result = await client.GetAsync("api/trivia/iniciar");
            //result.EnsureSuccessStatusCode();
            await connection.InvokeAsync("Iniciar");
        }

        public async Task EnviarPuntos(double puntos)
        {
            await connection.InvokeAsync("GuardarPuntaje", puntos);
        }

    }
}
