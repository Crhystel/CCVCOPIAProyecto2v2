using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;

namespace CCVProyecto2v2.Views.ViewsEstudiante;

public partial class AgregarEstudianteView : ContentPage
{
    private readonly EstudianteViewModel _viewModel;
    public AgregarEstudianteView()
    {
        InitializeComponent();
        _viewModel = new EstudianteViewModel(new EstudianteRepository(new HttpClient()));
        BindingContext = _viewModel;
    }
}