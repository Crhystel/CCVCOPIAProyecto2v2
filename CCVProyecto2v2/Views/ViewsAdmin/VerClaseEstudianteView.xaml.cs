
using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;
using CCVProyecto2v2.Views.ViewClaseEstudiante;

namespace CCVProyecto2v2.Views.ViewsAdmin;

public partial class VerClaseEstudianteView : ContentPage
{
    private readonly ClaseEstudianteViewModel _viewModel;
    public VerClaseEstudianteView()
	{
		InitializeComponent();
        _viewModel = new ClaseEstudianteViewModel(new ClaseEstudianteRepository(new HttpClient()));
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.GetClaseEstudiantes();
    }

    private void butonAgregar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AgregarClaseEstudianteView());
    }
}