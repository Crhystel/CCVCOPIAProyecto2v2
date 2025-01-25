using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;
using CCVProyecto2v2.Views.ViewClaseProfesor;

namespace CCVProyecto2v2.Views.ViewsAdmin;

public partial class VerClaseProfesorView : ContentPage
{
    private readonly ClaseProfesorViewModel _viewModel;
    public VerClaseProfesorView()
    {
        InitializeComponent();
        var claseProfesorRepository = new ClaseProfesorRepository(new HttpClient());
        var claseRepository = new ClaseRepository(new HttpClient());
        var profesorRepository = new ProfesorRepository(new HttpClient());
        _viewModel = new ClaseProfesorViewModel(claseProfesorRepository, claseRepository, profesorRepository);
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.GetClaseProfesores();
    }

    private void butonAgregar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AgregarClaseProfesorView());
    }
}