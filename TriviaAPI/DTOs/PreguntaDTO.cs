namespace TriviaAPI.DTOs
{
    public class PreguntaDTO
    {
        public int Id { get; set; }
        public string Pregunta { get; set; } = "";
        public List<Respuesta> Respuestas { get; set; } = new();
    }

    public class Respuesta
    {
        public string Titulo { get; set; } = "";
    }
}
