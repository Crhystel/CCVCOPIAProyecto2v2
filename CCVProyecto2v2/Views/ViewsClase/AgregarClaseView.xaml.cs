using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;

namespace CCVProyecto2v2.Views.ViewsClase;

public partial class AgregarClaseView : ContentPage
{
	private readonly ClaseViewModel _viewModel;
    public AgregarClaseView()
	{
		InitializeComponent();
        _viewModel = new ClaseViewModel(new ClaseRepository(new HttpClient()));
        BindingContext = _viewModel;
    }
}