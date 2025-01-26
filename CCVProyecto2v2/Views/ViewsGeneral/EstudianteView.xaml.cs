using Newtonsoft.Json;

namespace CCVProyecto2v2.ViewsGeneral;

public partial class EstudianteView : ContentPage
{
    public EstudianteView()
    {
        InitializeComponent();
        _ = CargarActividadesAsync();
    }
    private async Task CargarActividadesAsync()
    {
        try
        {
            // URL de la API
            string apiUrl = "http://localhost:5057/api/Actividades";

            // Cliente HTTP para hacer la solicitud
            using HttpClient client = new HttpClient();

            // Realizar la solicitud GET a la API
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            // Verificar si la respuesta es exitosa
            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como cadena
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializar el JSON a una lista de actividades
                var actividades = JsonConvert.DeserializeObject<List<Models.Actividad>>(jsonResponse) ?? new List<Models.Actividad>();

                // Asignar las actividades al ItemsSource
                ActividadesCollection.ItemsSource = actividades;
            }
            else
            {
                // Manejar errores de la solicitud HTTP
                await DisplayAlert("Error", "No se pudieron cargar las actividades desde la API.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Manejar excepciones
            await DisplayAlert("Error", $"Ocurrió un error al cargar las actividades: {ex.Message}", "OK");
        }
    }
}