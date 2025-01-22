using CCVProyecto2v2.Interfaces;
using CCVProyecto2v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CCVProyecto2v2.Repositories
{
    public class ClaseEstudianteRepository : IClaseEstudiante
    {
        private readonly HttpClient _httpClient;
        public ClaseEstudianteRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> ActualizarClaseEstudiante(int claseEstudianteId, ClaseEstudiante claseEstudiante)
        {
            try
            {
                string url = $"http://localhost:5057/api/ClaseEstudiantes?ceId={claseEstudianteId}";
                var claseEstudianteJson = JsonSerializer.Serialize(claseEstudiante);
                var content = new StringContent(claseEstudianteJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error al actualizar claseEstudiante. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CrearClaseEstudiante(int claseId, int estudianteId)
        {
            try
            {
                string url = "http://localhost:5057/api/ClaseEstudiantes/";
                var body = new
                {
                    id = 0,
                    claseId = claseId,
                    estudianteId = estudianteId
                };
                var claseEstudianteJson = JsonSerializer.Serialize(body);
                var content = new StringContent(claseEstudianteJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error al crear claseEstudiante. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarClaseEstudiante(int claseEstudianteId)
        {
            try
            {
                string url = $"http://localhost:5057/api/ClaseEstudiantes?ceId={claseEstudianteId}";
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error al eliminar claseEstudiante. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ClaseEstudiante>> GetClaseEstudiantes()
        {
            try
            {
                string url = "http://localhost:5057/api/ClaseEstudiantes";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var claseEstudiantes = await response.Content.ReadFromJsonAsync<List<ClaseEstudiante>>();
                    return claseEstudiantes ?? new List<ClaseEstudiante>();
                }
                else
                {
                    throw new Exception($"Error al obtener claseEstudiantes. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<ClaseEstudiante>();
            }
        }
    }
}
