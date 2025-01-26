using Newtonsoft.Json;

namespace CCVProyecto2v2.ViewsProfesor;

public partial class CrearActividadView : ContentPage
{
    public CrearActividadView()
    {
        InitializeComponent();
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        var actividad = new Models.Actividad
        {
            Titulo = editorNombre.Text,
            Descripcion = editorDescripción.Text,
            FechaCreacion = FechaInicio.Date,
            FechaEntrega = FechaEntrega.Date,
            // Rellenar IDs si es necesario
        };

        var actividadService = new Services.ActividadService();

        if (await actividadService.CrearActividadAsync(actividad))
        {
            await DisplayAlert("Éxito", "Actividad creada correctamente", "OK");
        }
        else
        {
            await DisplayAlert("Error", "No se pudo crear la actividad.", "OK");
        }
    }

}