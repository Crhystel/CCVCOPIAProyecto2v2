using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCVProyecto2v2.Models;
using Newtonsoft.Json;

namespace CCVProyecto2v2.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<bool> LoginAsync(string nombreUsuario, string contrasenia)
        {
            var loginData = new
            {
                nombreUsuario = nombreUsuario,
                contrasenia = contrasenia
            };

            var jsonContent = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("'http://localhost:5057/api/Login", content);
                if (response.IsSuccessStatusCode)
                {
                    // Leer los datos del usuario desde la respuesta
                    var userJson = await response.Content.ReadAsStringAsync();
                    var usuario = JsonConvert.DeserializeObject<Usuario>(userJson);
                    SecureStorage.SetAsync("userId", usuario.Id.ToString());
                    SecureStorage.SetAsync("userRole", usuario.Rol.ToString());

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar hacer login: {ex.Message}");
                return false;
            }
        }
    }
}
