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
    public class ClaseProfesorRepository : IClaseProfesor
    {
        private readonly HttpClient _httpClient;
        public ClaseProfesorRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> ActualizarClaseProfesor(int claseProfesorId, ClaseProfesor claseProfesor)
        {
            try
            {
                string url = $"http://localhost:5057/api/ClaseProfesores?cpId={claseProfesorId}";
                var claseProfesorJson = JsonSerializer.Serialize(claseProfesor);
                var content = new StringContent(claseProfesorJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error al actualizar claseProfesor. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CrearClaseProfesor(ClaseProfesor claseProfesor)
        {
            try
            {
                string url = "http://localhost:5057/api/ClaseProfesores";
                var claseProfesorJson = JsonSerializer.Serialize(claseProfesor);
                var content = new StringContent(claseProfesorJson, Encoding.UTF8, "application/json");
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

        public async Task<bool> EliminarClaseProfesor(int claseProfesorId)
        {
            try
            {
                string url = $"http://localhost:5057/api/ClaseProfesores?cpId={claseProfesorId}";
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

        public async  Task<List<ClaseProfesor>> GetClaseProfesores()
        {
            try
            {
                string url = "http://localhost:5057/api/ClaseProfesores";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var claseProfesor = await response.Content.ReadFromJsonAsync<List<ClaseProfesor>>();
                    return claseProfesor ?? new List<ClaseProfesor>();
                }
                else
                {
                    throw new Exception($"Error al obtener claseEstudiantes. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<ClaseProfesor>();
            }
        }
    }
}
