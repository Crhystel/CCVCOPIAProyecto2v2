using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CCVProyecto2v2.Dto;

namespace CCVProyecto2v2.Services
{
    internal class ClaseService
    {
        private readonly HttpClient _httpClient;

        public ClaseService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<ClaseDto>> ObtenerClasesAsync()
        {
            try
            {
                var url = "http://localhost:5057/api/Clases";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ClaseDto>>(json);
                }
                return new List<ClaseDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo clases: {ex.Message}");
                return new List<ClaseDto>();
            }
        }
    }
}
