using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CCVProyecto2v2.Models;

namespace CCVProyecto2v2.Services
{
    internal class ActividadService
    {
        private readonly HttpClient _httpClient;

        public ActividadService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> CrearActividadAsync(Models.Actividad actividad, int claseId)
        {
            try
            {
                var url = $"http://localhost:5057/api/Actividades?claseId={claseId}";
                var json = JsonConvert.SerializeObject(actividad);
                Console.WriteLine($"Request URL: {url}");
                Console.WriteLine($"Request Body: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                // Registrar detalles de la respuesta
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Status Code: {response.StatusCode}");
                Console.WriteLine($"Response Body: {responseBody}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creando actividad: {ex.Message}");
                return false;
            }
        }




        public async Task<List<Models.Actividad>> ObtenerActividadesAsync()
        {
            try
            {
                var url = "http://localhost:5057/api/Actividades";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Models.Actividad>>(json);
                }

                return new List<Models.Actividad>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo actividades: {ex.Message}");
                return new List<Models.Actividad>();
            }
        }

        public async Task<List<Models.Actividad>> ObtenerActividadesPorClaseAsync(int claseId)
        {
            try
            {
                var url = $"http://localhost:5057/api/Actividades/PorClase/{claseId}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Models.Actividad>>(json);
                }
                return new List<Models.Actividad>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo actividades por clase: {ex.Message}");
                return new List<Models.Actividad>();
            }
        }

        public async Task<bool> EliminarActividadAsync(int actividadId)
        {
            try
            {
                var url = $"http://localhost:5057/api/Actividades/{actividadId}";
                var response = await _httpClient.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error eliminando actividad: {ex.Message}");
                return false;
            }
        }

        /*public async Task<bool> EditarActividadAsync(Models.Actividad actividad)
        {
            try
            {
                var url = $"http://localhost:5057/api/Actividades";
                var json = JsonConvert.SerializeObject(actividad);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error editando actividad: {ex.Message}");
                return false;
            }
        }*/

    }
}
