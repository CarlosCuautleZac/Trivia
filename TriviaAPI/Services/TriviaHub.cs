using Microsoft.AspNetCore.SignalR;
using TriviaAPI.DTOs;

namespace TriviaAPI.Services
{
    public class TriviaHub : Hub
    {
        public static List<string> ActiveConnections { get; } = new List<string>();
        public static List<Jugador> Jugadores { get; } = new List<Jugador>();

        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            // creamos a un nuevo jugador
            Jugador jugador = new Jugador()
            {
                ConnectionId = connectionId,
                NombreUsuario = Alias(),
                Puntos = 0
            };

            Jugadores.Add(jugador);

            // Enviar la lista actualizada
            await Clients.All.SendAsync("NewConnection", Jugadores);
            await Clients.Client(jugador.ConnectionId).SendAsync("Datos", jugador);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

            var connectionId = Context.ConnectionId;

            //buscamos la conexion
            var jugadordesconectado = Jugadores.FirstOrDefault(x => x.ConnectionId == connectionId);

            //lo removemos de la lista
            if (jugadordesconectado != null)
                Jugadores.Remove(jugadordesconectado);

            //enviamos la lista actualizada
            await Clients.All.SendAsync("DisconnectedConnection", Jugadores);

            await base.OnDisconnectedAsync(exception);
        }

        string Alias()
        {
            Random rand = new Random();

            List<string> palabras = new List<string> {
            "creativo", "genial", "increible", "super", "fantastico",
            "impresionante", "espectacular", "sensacional", "extraordinario",
            "magnifico", "maravilloso", "excepcional", "unico", "original"
        };

            int cantidadPalabras = rand.Next(1, 4);

            string alias = "";

            for (int i = 0; i < cantidadPalabras; i++)
            {
                int indicePalabra = rand.Next(palabras.Count);
                alias += palabras[indicePalabra];
                alias += " ";
            }


            return alias.Trim();
        }

        public async void GuardarPuntaje(double puntos)
        {
            var connectionId = Context.ConnectionId;

            var jugador = Jugadores.FirstOrDefault(x => x.ConnectionId == connectionId);

            if (jugador != null)
            {
                var posicion = Jugadores.IndexOf(jugador);
                Jugadores[posicion].Puntos += puntos;
                await Clients.All.SendAsync("ActualizarLista", Jugadores);
            }
        }

        public async void Iniciar()
        {
            Jugadores.ForEach(x=>x.Puntos=0);
            await Clients.All.SendAsync("ActualizarLista", Jugadores);
            await Clients.All.SendAsync("iniciar");
        }

    }
}
