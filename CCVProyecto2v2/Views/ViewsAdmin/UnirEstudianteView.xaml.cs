using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.Dto;
using CCVProyecto2v2.ViewsModels;

namespace CCVProyecto2v2.ViewsAdmin;

public partial class UnirEstudianteView : ContentPage
{
	public UnirEstudianteView()
	{
		InitializeComponent();
        BindingContext = new UnirEViewModel(new DbbContext());

    }
    private void OnClaseSelected(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedItem is ClaseDto claseSeleccionada)
        {
            if (BindingContext is UnirEViewModel viewModel)
            {
                viewModel.ClaseEstudianteDto.ClaseId = claseSeleccionada.Id;
            }
        }
    }

    private void OnEstudianteSelected(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedItem is EstudianteDto estudianteSeleccionado)
        {
            if (BindingContext is UnirEViewModel viewModel)
            {
                viewModel.ClaseEstudianteDto.EstudianteId = estudianteSeleccionado.Id;
            }
        }
    }

}