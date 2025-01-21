using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;

namespace CCVProyecto2v2.Views.ViewsEstudiante;

public partial class EditarEstudianteView : ContentPage
{
	private readonly EstudianteViewModel _viewModel;
	public EditarEstudianteView()
	{
		InitializeComponent();
		_viewModel = new EstudianteViewModel(new EstudianteRepository(new HttpClient()));
		BindingContext = _viewModel;
	}
}