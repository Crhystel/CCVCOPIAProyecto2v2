using Newtonsoft.Json;
using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Services;

namespace CCVProyecto2v2.ViewsProfesor;

public partial class CrearActividadView : ContentPage
{
    public CrearActividadView()
    {
        InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarClasesAsync();
    }

    private async Task CargarClasesAsync()
    {
        var claseService = new ClaseService(); // Servicio para obtener las clases
        var clases = await claseService.ObtenerClasesAsync();
        clasePicker.ItemsSource = clases;
    }
    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (clasePicker.SelectedItem is ClaseDto claseSeleccionada)
        {
            var actividad = new Models.Actividad
            {
                Titulo = editorNombre.Text,
                Descripcion = editorDescripción.Text,
                FechaCreacion = FechaInicio.Date,
                FechaEntrega = FechaEntrega.Date,
                claseId = new List<int> { claseSeleccionada.Id }
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
        else
        {
            await DisplayAlert("Error", "Seleccione una clase antes de guardar.", "OK");
        }
    }


}