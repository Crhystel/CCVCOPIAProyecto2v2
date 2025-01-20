using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewsModels;

namespace CCVProyecto2v2.Views.ViewsEstudiante;

public partial class AgregarEstudiante : ContentPage
{
    private readonly EstudianteViewModel _viewModel;
    public AgregarEstudiante()
	{
		InitializeComponent();
        _viewModel = new EstudianteViewModel(new EstudianteRepository(new HttpClient()));
        BindingContext = _viewModel;
    }
}