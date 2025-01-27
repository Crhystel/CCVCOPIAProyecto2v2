using CCVProyecto2v2.Services;
using CCVProyecto2v2.ViewsProfesor;
using Newtonsoft.Json;

namespace CCVProyecto2v2.ViewsGeneral;

public partial class ProfesorView : ContentPage
{
    public ProfesorView()
    {
        InitializeComponent();
        CargarActividades();
    }
    public void CrearActividad_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CrearActividadView());
    }
    private async void CargarActividades()
    {
        try
        {
            var actividadService = new ActividadService();
            var actividades = await actividadService.ObtenerActividadesAsync();
            ActividadesCollection.ItemsSource = actividades;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo cargar las actividades: {ex.Message}", "OK");
        }
    }


    private async void EliminarActividad_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null && button.CommandParameter is int actividadId)
        {
            try
            {
                var confirmacion = await DisplayAlert("Confirmar", "¿Deseas eliminar esta actividad?", "Sí", "No");
                if (confirmacion)
                {
                    var actividadService = new ActividadService();
                    var resultado = await actividadService.EliminarActividadAsync(actividadId);
                    if (resultado)
                    {
                        await DisplayAlert("Éxito", "Actividad eliminada correctamente", "OK");
                        CargarActividades(); // Refrescar lista
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar la actividad", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
    }


    /*private async void EditarActividad_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null && button.CommandParameter is Models.Actividad actividad)
        {
            try
            {
                var nuevoTitulo = await DisplayPromptAsync("Editar Actividad", "Nuevo título:", initialValue: actividad.Titulo);
                var nuevaDescripcion = await DisplayPromptAsync("Editar Actividad", "Nueva descripción:", initialValue: actividad.Descripcion);

                if (!string.IsNullOrWhiteSpace(nuevoTitulo) && !string.IsNullOrWhiteSpace(nuevaDescripcion))
                {
                    actividad.Titulo = nuevoTitulo;
                    actividad.Descripcion = nuevaDescripcion;

                    var actividadService = new ActividadService();
                    var resultado = await actividadService.EditarActividadAsync(actividad);
                    if (resultado)
                    {
                        await DisplayAlert("Éxito", "Actividad editada correctamente", "OK");
                        CargarActividades(); // Refrescar lista
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo editar la actividad", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
    }*/


}