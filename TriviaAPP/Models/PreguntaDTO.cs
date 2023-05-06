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
        public string RespuestaCorrecta { get; set; } = "";
        public string? Imagen { get; set; }
        public List<Respuesta> Respuestas { get; set; } = new();
    }

    public class Respuesta
    {
        public string Titulo { get; set; } = "";
    }
}
