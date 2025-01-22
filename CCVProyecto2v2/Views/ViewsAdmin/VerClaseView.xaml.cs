

using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;
using CCVProyecto2v2.Views.ViewsClase;

namespace CCVProyecto2v2.Views.ViewsAdmin;

public partial class VerClaseView : ContentPage
{
    private readonly ClaseViewModel _viewModel;
    public VerClaseView()
	{
        InitializeComponent();
        _viewModel = new ClaseViewModel(new ClaseRepository(new HttpClient()));
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.GetClases();
    }

    private void butonAgregar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AgregarClaseView());

    }
}