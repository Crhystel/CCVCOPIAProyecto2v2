using CCVProyecto2v2.Interfaces;
using CCVProyecto2v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Repositories
{
    public class EstudianteRepository : IEstudiante
    {
        private readonly HttpClient _httpClient;
        public EstudianteRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        

        public  async Task<List<Estudiante>> GetEstudiantes()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://localhost:5057/api/Estudiantes";
                    client.DefaultRequestHeaders.Add("Accept", "text/plain");

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var estudiantes = await response.Content.ReadFromJsonAsync<List<Estudiante>>();
                        return estudiantes ?? new List<Estudiante>();
                    }
                    else
                    {
                        throw new Exception($"Error al obtener estudiantes. Código de estado: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return new List<Estudiante>();
                }
            }
        }
    }
}
