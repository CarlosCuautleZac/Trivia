using System;
using System.Collections.Generic;

namespace TriviaAPI.Models;

public partial class Respuestaserroneas
{
    public int Id { get; set; }

    public string Respuesta { get; set; } = null!;

    public int? Idpregunta { get; set; }

    public virtual Preguntas? IdpreguntaNavigation { get; set; }
}
