namespace TriviaAPI.DTOs
{
    public class PreguntaDTO
    {
        public int Id { get; set; }
        public string Pregunta { get; set; } = "";
        public List<string> Respuestas { get; set; } = new();
    }
}
