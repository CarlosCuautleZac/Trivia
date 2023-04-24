using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaAPP.Models
{
    public class PreguntaDTO
    {
        public int Id { get; set; }
        public string Pregunta { get; set; } = "";
        public List<string> Respuestas { get; set; } = new();
    }
}
