using CCVProyecto2v2.Views.ViewsAdmin;
using CCVProyecto2v2.Views.ViewsProfesor;
using CCVProyecto2v2.ViewsProfesor;

namespace CCVProyecto2v2.ViewsGeneral;

public partial class AdministradorView : ContentPage
{
        public AdministradorView()
        {
            InitializeComponent();
        }
        public void CrearProfesor_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VerProfesorView());
        }
    public void CrearEstudiante_Clicked(object sender, EventArgs e)
    {
        
        Navigation.PushAsync(new VerEstudianteView());
    }
        public void CrearCurso_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VerClaseProfesorView());
        }
    public void UnirEstudiante_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new VerClaseEstudianteView());
    }

    private void butun_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new VerClaseView());
    }
}