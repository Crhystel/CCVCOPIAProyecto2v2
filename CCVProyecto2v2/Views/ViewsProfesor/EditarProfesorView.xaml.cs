using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;

namespace CCVProyecto2v2.Views.ViewsProfesor;

public partial class EditarProfesorView : ContentPage
{
	private readonly ProfesorViewModel _viewModel;

    public EditarProfesorView()
	{
		InitializeComponent();
        _viewModel = new ProfesorViewModel(new ProfesorRepository(new HttpClient()));
        BindingContext = _viewModel;
    }
}