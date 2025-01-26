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
            try
            {
                var url = $"http://localhost:5057/api/Login?nombreUsuario={Uri.EscapeDataString(nombreUsuario)}&contrasenia={Uri.EscapeDataString(contrasenia)}";
                var response = await _httpClient.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    var userJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Respuesta exitosa: {userJson}");

                    var usuario = JsonConvert.DeserializeObject<Usuario>(userJson);
                    await SecureStorage.SetAsync("userId", usuario.Id.ToString());
                    await SecureStorage.SetAsync("userRole", usuario.Rol.ToString());

                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error en la respuesta: {response.StatusCode} - {errorContent}");
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
