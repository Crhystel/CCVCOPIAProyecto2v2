using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;

namespace CCVProyecto2v2.Views.ViewClaseProfesor;

public partial class EditarClaseProfesorView : ContentPage
{
	private readonly ClaseProfesorViewModel _viewModel;
	public EditarClaseProfesorView()
	{
		InitializeComponent();
		_viewModel = new ClaseProfesorViewModel(new ClaseProfesorRepository(new HttpClient()),
            new ClaseRepository(new HttpClient()), new ProfesorRepository(new HttpClient()));
		BindingContext = _viewModel;
		_viewModel.CargarDatosPickers();
	}
}