using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;

namespace CCVProyecto2v2.Views.ViewsClase;

public partial class EditarClaseView : ContentPage
{
	private readonly ClaseViewModel _viewModel;
    public EditarClaseView()
	{
		InitializeComponent();
        _viewModel = new ClaseViewModel(new ClaseRepository(new HttpClient()));
        BindingContext = _viewModel;
    }
}