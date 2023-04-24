using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TriviaAPI.DTOs;
using TriviaAPI.Models;
using TriviaAPI.Repositories;
using TriviaAPI.Services;

namespace TriviaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriviaController : ControllerBase
    {
        private readonly IHubContext<TriviaHub> hub;
        Repository<Preguntas> repositoryP;
        Repository<Respuestaserroneas> repositoryR;
        Random random = new();

        public TriviaController(Sistem21TriviaContext context, IHubContext<TriviaHub> hub)
        {
            repositoryP = new(context);
            repositoryR = new(context);
            this.hub = hub;
        }



        [HttpGet("Jugar")]
        public async Task<IActionResult> EnviarPregunta()
        {
            //Primero hacer que le envien una pregunta a los usuarios conectados, despues hacer que no se repitan las 
            //preguntas hechas

            var totalpreguntas = repositoryP.GetAll().Count();
            var numeropregunta = random.Next(1, totalpreguntas + 1);

            var pregunta = repositoryP.GetbyId(numeropregunta);
            var respuestas = repositoryR.GetAll().Where(x=>x.Idpregunta==numeropregunta).Select(x=>x.Respuesta).ToList();

            PreguntaDTO preguntaDTO = new PreguntaDTO()
            {
                Id = pregunta.Id,
                Pregunta = pregunta.Pregunta,
                Respuestas = respuestas
            };

            //a;adimos la respuesta buena
            preguntaDTO.Respuestas.Add(pregunta.Respuesta);

            //ahora cambiamos de orden las respuestas
            preguntaDTO.Respuestas = RevolverRespuestas(preguntaDTO.Respuestas).ToList();


            //Se lo enviamos a los clientes de signalr
            await hub.Clients.All.SendAsync("recibirpregunta", preguntaDTO);

            return Ok(preguntaDTO);
        }

        IEnumerable<string> RevolverRespuestas(List<string> respuestas)
        {
            Random rand = new Random();

            for (int i = 0; i < respuestas.Count; i++)
            {
                int randomIndex = rand.Next(respuestas.Count);
                var temp = respuestas[i];
                respuestas[i] = respuestas[randomIndex];
                respuestas[randomIndex] = temp;
            }

            return respuestas;
        }


    }
}
