using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaAPP.Models
{
    public class Jugador
    {
        public string ConnectionId { get; set; } = "";
        public string NombreUsuario { get; set; } = "";
        public double Puntos { get; set; }
    }
}
