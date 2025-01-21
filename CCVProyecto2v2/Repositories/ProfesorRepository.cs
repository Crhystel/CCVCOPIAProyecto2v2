using CCVProyecto2v2.Interfaces;
using CCVProyecto2v2.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CCVProyecto2v2.Repositories
{
    public class ProfesorRepository : IProfesor
    {
        private readonly HttpClient _httpClient;
        public ProfesorRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CrearProfesor(MateriaEnum materia, Profesor profesor)
        {
            try
            {
                Enum.IsDefined(typeof(MateriaEnum), materia);
                string materiaId = ((int)materia).ToString();
                string url = $"http://localhost:5057/api/Profesores?materiaId={materiaId}";
                var profesorJson = JsonSerializer.Serialize(profesor);
                var content = new StringContent(profesorJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error al crear profesor. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarProfesor(int profesorId)
        {
            try
            {
                string url= $"http://localhost:5057/api/Profesores?profesorId={profesorId}";
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url),
                    Content = new StringContent(
                        JsonSerializer.Serialize(new Profesor
                        {
                            Id = profesorId,
                            Nombre = "string",
                            Edad = 0,
                            Cedula = "string",
                            NombreUsuario = "string",
                            Contrasenia = "string",

                        }),
                        Encoding.UTF8,
                        "application/json"
                        )
                };
                HttpResponseMessage response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error al eliminar profesor. Código de estado: {response.StatusCode}");
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Profesor>> GetProfesores()
        {
            try
            {
                string url = "http://localhost:5057/api/Profesores";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if(response.IsSuccessStatusCode)
                {
                    var profesores = await response.Content.ReadFromJsonAsync<List<Profesor>>();
                    return profesores ?? new List<Profesor>();
                }
                else
                {
                    throw new Exception($"Error al obtener profesores. Código de estado: {response.StatusCode}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<Profesor>();
            }
        }
    }
}
