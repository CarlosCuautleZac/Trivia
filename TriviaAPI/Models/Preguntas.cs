using System;
using System.Collections.Generic;

namespace TriviaAPI.Models;

public partial class Preguntas
{
    public int Id { get; set; }

    public string Pregunta { get; set; } = null!;

    public string Respuesta { get; set; } = null!;

    public string? Imagen { get; set; }

    public virtual ICollection<Respuestaserroneas> Respuestaserroneas { get; set; } = new List<Respuestaserroneas>();
}
