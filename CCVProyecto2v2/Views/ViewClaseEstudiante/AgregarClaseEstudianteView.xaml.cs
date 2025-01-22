using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;

namespace CCVProyecto2v2.Views.ViewClaseEstudiante;

public partial class AgregarClaseEstudianteView : ContentPage
{
	private readonly ClaseEstudianteViewModel _viewModel;
    public AgregarClaseEstudianteView()
	{
		InitializeComponent();
        _viewModel = new ClaseEstudianteViewModel(new ClaseEstudianteRepository(new HttpClient()));
        BindingContext = _viewModel;
    }
}