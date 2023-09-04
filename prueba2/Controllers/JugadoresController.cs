using prueba2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace prueba2.Controllers
{
    public class JugadoresController : Controller
    {
        // GET: Jugadores
        

        string baseURL = "https://v3.football.api-sports.io/players";

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseURL);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-apisports-key", "6cb71eb3436867753aeb5c3a1bdc5b52");
            client.DefaultRequestHeaders.Add("x-rapidapi-host", "v3.football.api-sports.io");
            return client;
        }

        public async Task<ActionResult> Index()
        {
            List<Jugador> equipos = new List<Jugador>();
            using (var client = GetClient())
            {
                //HttpResponseMessage Res = await client.GetAsync("?id=33");
                HttpResponseMessage Res = await client.GetAsync("?season=2019&league=61");
                if (Res.IsSuccessStatusCode)
                {
                    var result = Res.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var jsonObject = (JObject)JsonConvert.DeserializeObject(result);
                        var response = (JArray)jsonObject["response"];
                        equipos.AddRange(response.Select(teamJObject => teamJObject["player"].ToObject<Jugador>()));
                    }
                    catch (Exception) { }
                }
            }
            return View(equipos);
        }

        public async Task<ActionResult> Detalles(int id)
        {
            Jugador equipo = new Jugador();

            using (var client = GetClient())
            {
                HttpResponseMessage res = await client.GetAsync("?season=2019&league=61&id=" + id);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var jsonObject = (JObject)JsonConvert.DeserializeObject(result);
                        var response = (JArray)jsonObject["response"];
                        equipo = response[0]["player"].ToObject<Jugador>();
                    }
                    catch (Exception) { }
                }
            }

            return View(equipo);
        }
    }
}