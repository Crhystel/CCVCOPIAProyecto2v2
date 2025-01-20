
using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.Views.ViewsEstudiante;
using CCVProyecto2v2.ViewsModels;

namespace CCVProyecto2v2.ViewsAdmin;

public partial class AgregarEstudianteView : ContentPage
{
    private readonly EstudianteViewModel _viewModel;
    public AgregarEstudianteView()
    {
        InitializeComponent();
        _viewModel = new EstudianteViewModel(new EstudianteRepository(new HttpClient()));
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.GetEstudiantes();
    }

    private void butonAgregar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AgregarEstudiante());
    }
}