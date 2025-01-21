using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Interfaces;
using CCVProyecto2v2.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CCVProyecto2v2.Repositories
{
    public class EstudianteRepository : IEstudiante
    {
        private readonly HttpClient _httpClient;
        public EstudianteRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ActualizarEstudiante(int estudianteId, Estudiante estudiante)
        {
            try
            {
                string url = $"http://localhost:5057/api/Estudiantes?estudianteId={estudianteId}";
                var estudianteJson=JsonSerializer.Serialize(estudiante);
                var content = new StringContent(estudianteJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error al actualizar estudiante. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CrearEstudiante(GradoEnum grado, Estudiante estudiante)
        {
            try
            {
                Enum.IsDefined(typeof(GradoEnum), grado);
                string gradoId = ((int)grado).ToString();
                string url = $"http://localhost:5057/api/Estudiantes?gradoId={gradoId}";

                var estudianteJson = JsonSerializer.Serialize(estudiante);
                var content = new StringContent(estudianteJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error al crear estudiante. Código de estado: {response.StatusCode}");
                    
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarEstudiante(int estudianteId)
        {
            try
            {
                string url = $"http://localhost:5057/api/Estudiantes?estudianteId={estudianteId}";
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error al eliminar estudiante. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public  async Task<List<Estudiante>> GetEstudiantes()
        {
            
                try
                {
                    string url = "http://localhost:5057/api/Estudiantes";
                    HttpResponseMessage response = await _httpClient.GetAsync(url);

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
