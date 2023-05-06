using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.InteropServices;
using TriviaAPI.DTOs;
using TriviaAPI.Models;
using TriviaAPI.Repositories;
using TriviaAPI.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TriviaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriviaController : ControllerBase
    {
        private readonly IHubContext<TriviaHub> hub;
        private readonly TriviaHub _hub;
        Repository<Preguntas> repositoryP;
        Repository<Respuestaserroneas> repositoryR;
        Random random = new();
        public List<Jugador> Jugadores = new List<Jugador>();
        string wwwrootpath = "";

        public TriviaController(Sistem21TriviaContext context, IHubContext<TriviaHub> hub, TriviaHub _hub, IWebHostEnvironment environment)
        {
            repositoryP = new(context);
            repositoryR = new(context);
            this.hub = hub;
            this._hub = _hub;
            wwwrootpath = environment.WebRootPath;
        }

        [HttpGet("conectar")]
        public async Task<IActionResult> Clientes()
        {   
            await hub.Clients.All.SendAsync("Conectado");
            return Ok();
        }

        [HttpGet("Iniciar")]
        public async Task<IActionResult> Iniciar()
        {
            //el host de la partida llama a este metodo para que a todos en la partida se les cambie la pantalla a la de juego
            await hub.Clients.All.SendAsync("iniciar");
            return Ok();
        }


        [HttpGet("Jugar")]
        public async Task<IActionResult> EnviarPregunta()
        {
            //Primero hacer que le envien una pregunta a los usuarios conectados, despues hacer que no se repitan las 
            //preguntas hechas

            var totalpreguntas = repositoryP.GetAll().Count();
            var numeropregunta = random.Next(1, totalpreguntas + 1);

            var pregunta = repositoryP.GetbyId(numeropregunta);
            var respuestas = repositoryR.GetAll().Where(x => x.Idpregunta == numeropregunta).Select(x => new Respuesta() { Titulo = x.Respuesta}).ToList();

            PreguntaDTO preguntaDTO = new PreguntaDTO()
            {
                Id = pregunta.Id,
                Pregunta = pregunta.Pregunta,
                RespuestaCorrecta = pregunta.Respuesta,
                Respuestas = respuestas,
                Imagen = pregunta.Imagen
            };

            //a;adimos la respuesta buena
            Respuesta r = new() { Titulo = pregunta.Respuesta };
            preguntaDTO.Respuestas.Add(r);

            //ahora cambiamos de orden las respuestas
            preguntaDTO.Respuestas = RevolverRespuestas(preguntaDTO.Respuestas).ToList();


            //Se lo enviamos a los clientes de signalr
            await hub.Clients.All.SendAsync("recibirpregunta", preguntaDTO);

            return Ok(preguntaDTO);
        }

        private string GetImagen(int id)
        {
            string ruta = $"{wwwrootpath}\\img";
            string[] archivo = Directory.GetFiles(ruta);       
            Byte[] bytes = System.IO.File.ReadAllBytes(archivo[0]);
            String file = Convert.ToBase64String(bytes);
            return file;
        }

        IEnumerable<Respuesta> RevolverRespuestas(List<Respuesta> respuestas)
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