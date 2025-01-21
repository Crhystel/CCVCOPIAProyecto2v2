using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.Views.ViewsEstudiante;
using CCVProyecto2v2.ViewModels;

namespace CCVProyecto2v2.Views.ViewsAdmin;

public partial class VerEstudianteView : ContentPage
{
    private readonly EstudianteViewModel _viewModel;
    public VerEstudianteView()
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
        Navigation.PushAsync(new AgregarEstudianteView());
    }
}