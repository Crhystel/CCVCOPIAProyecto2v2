using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;

namespace CCVProyecto2v2.Views.ViewClaseEstudiante;

public partial class EditarClaseEstudianteView : ContentPage
{
	private readonly ClaseEstudianteViewModel _viewModel;
    public EditarClaseEstudianteView()
	{
        InitializeComponent();
        var claseEstudianteRepository = new ClaseEstudianteRepository(new HttpClient());
        var claseRepository = new ClaseRepository(new HttpClient());
        var estudianteRepository = new EstudianteRepository(new HttpClient());
        _viewModel = new ClaseEstudianteViewModel(claseEstudianteRepository, claseRepository, estudianteRepository);
        BindingContext = _viewModel;
    }
}