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
    private void CargarActividades()
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "actividades.txt");

        if (File.Exists(path))
        {
            var actividades = JsonConvert.DeserializeObject<List<Models.Actividad>>(File.ReadAllText(path)) ?? new List<Models.Actividad>();
            ActividadesCollection.ItemsSource = actividades;
        }
    }

    private void EliminarActividad_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null && button.CommandParameter is int actividadId)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ACTIVIDADES.txt");

            if (File.Exists(path))
            {
                try
                {
                   
                    var actividades = JsonConvert.DeserializeObject<List<Models.Actividad>>(File.ReadAllText(path)) ?? new List<Models.Actividad>();

                    
                    var actividadEliminar = actividades.FirstOrDefault(a => a.Id == actividadId);
                    if (actividadEliminar != null)
                    {
                        actividades.Remove(actividadEliminar);

                        
                        File.WriteAllText(path, JsonConvert.SerializeObject(actividades));

                        
                        CargarActividades();

                        DisplayAlert("�xito", "Actividad eliminada correctamente", "OK");
                    }
                    else
                    {
                        DisplayAlert("Error", "No se encontr� la actividad", "OK");
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"No se pudo eliminar la actividad: {ex.Message}", "OK");
                }
            }
            else
            {
                DisplayAlert("Error", "El archivo de actividades no existe", "OK");
            }
        }
    }
}