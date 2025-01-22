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
    public class ClaseRepository : IClase
    {
        private readonly HttpClient _httpClient;
        public ClaseRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> ActualizarClase(int claseId, Clase clase)
        {
            try
            {
                string url = "http://localhost:5057/api/Clases";
                var claseJson = JsonSerializer.Serialize(clase);
                var content = new StringContent(claseJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error al actualizar clase. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CrearClase(Clase clase)
        {
            try
            {
                string url = "http://localhost:5057/api/Clases";
                var claseJson = JsonSerializer.Serialize(clase);
                var content = new StringContent(claseJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                if(response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error al crear clase. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public Task<bool> EliminarClase(int claseId)
        {
            try
            {
                string url = $"http://localhost:5057/api/Clases?claseId={claseId}";
                HttpResponseMessage response = _httpClient.DeleteAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Task.FromResult(true);
                }
                else
                {
                    throw new Exception($"Error al eliminar clase. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return Task.FromResult(false);
            }
        }

        public async Task<List<Clase>> GetClases()
        {
            try
            {
                string url = "http://localhost:5057/api/Clases";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var clases = await response.Content.ReadFromJsonAsync<List<Clase>>();
                    return clases ?? new List<Clase>();
                }
                else
                {
                    throw new Exception($"Error al obtener clases. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<Clase>();
            }
        }
    }
}
