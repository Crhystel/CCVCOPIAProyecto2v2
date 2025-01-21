using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;
using CCVProyecto2v2.ViewsGeneral;

namespace CCVProyecto2v2.Views.ViewsAdmin;

public partial class VerProfesorView : ContentPage
{
	private readonly ProfesorViewModel _viewModel;
	public VerProfesorView()
	{
		InitializeComponent();
		_viewModel = new ProfesorViewModel(new ProfesorRepository(new HttpClient()));
		BindingContext=_viewModel;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.GetProfesores();
    }
    private void butunAgregar_Clicked(object sender, EventArgs e)
    {


    }
}