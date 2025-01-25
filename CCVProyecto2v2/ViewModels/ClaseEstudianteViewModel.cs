using CCVProyecto2v2.Models;
using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.Views.ViewClaseEstudiante;
using CCVProyecto2v2.Views.ViewsAdmin;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CCVProyecto2v2.ViewModels
{
    public partial class ClaseEstudianteViewModel:INotifyPropertyChanged
    {
        private readonly ClaseEstudianteRepository _claseEstudianteRepository;
        public ObservableCollection<ClaseEstudiante> ClaseEstudiantes {  get; set; }
        public ICommand CrearClaseEstudianteCommand {  get; } 
        public ICommand EliminarClaseEstudianteCommand { get; }
        public ICommand ActualizarClaseEstudianteCommand { get; }
        public ICommand GuardarClaseEstudianteCommand { get; }

        private int _id;
        private int _claseId;
        private int _estudianteId;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public int ClaseId
        {
            get => _claseId;
            set { _claseId = value; OnPropertyChanged(); }
        }
        public int EstudianteId
        {
            get => _estudianteId;
            set { _estudianteId = value; OnPropertyChanged(); }
        }

        public ClaseEstudianteViewModel(ClaseEstudianteRepository claseEstudianteRepository)
        {
            _claseEstudianteRepository = claseEstudianteRepository;
            ClaseEstudiantes=new ObservableCollection<ClaseEstudiante>();
            CrearClaseEstudianteCommand = new Command(async () => await CrearClaseEstudiante());
            EliminarClaseEstudianteCommand = new AsyncRelayCommand<int>(EliminarClaseEstudiante);
            ActualizarClaseEstudianteCommand = new AsyncRelayCommand<int>(ActualizarClaseEstudiante);
            GuardarClaseEstudianteCommand = new AsyncRelayCommand(GuardarCambios);

        }
        public ClaseEstudianteViewModel()
        {
            
        }
        public async Task CrearClaseEstudiante()
        {
            try
            {
                var nuevaClaseEstudiante = new ClaseEstudiante
                {
                    Id = 0,
                    ClaseId=ClaseId,
                    EstudianteId=EstudianteId
                };
                var resultado=await _claseEstudianteRepository.CrearClaseEstudiante(nuevaClaseEstudiante);
                if (resultado)
                {
                    await Application.Current.MainPage.DisplayAlert("Creado", "ClaseEstudiante creada exitosamente", "Ok");
                    await Application.Current.MainPage.Navigation.PushAsync(new VerClaseEstudianteView());
                    await GetClaseEstudiantes();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error al crear claseEstudiante", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        public async Task GetClaseEstudiantes()
        {
            try
            {
                var claseEstudiantes = await _claseEstudianteRepository.GetClaseEstudiantes();
                ClaseEstudiantes.Clear();
                foreach (var claseEstudiante in claseEstudiantes)
                {
                    ClaseEstudiantes.Add(new ClaseEstudiante
                    {
                        Id = claseEstudiante.Id,
                        ClaseId = claseEstudiante.ClaseId,
                        EstudianteId = claseEstudiante.EstudianteId
                    });
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        private async Task ActualizarClaseEstudiante(int ceId)
        {
            try
            {
                var claseEstudiante= await _claseEstudianteRepository.GetClaseEstudiantes();
                var claseEstudianteSeleccionada= claseEstudiante.FirstOrDefault(c => c.Id == ceId);
                if (claseEstudianteSeleccionada != null)
                {
                    Id= claseEstudianteSeleccionada.Id;
                    ClaseId = claseEstudianteSeleccionada.ClaseId;
                    EstudianteId = claseEstudianteSeleccionada.EstudianteId;
                    await Application.Current.MainPage.Navigation.PushAsync(new EditarClaseEstudianteView
                    {
                        BindingContext = this
                    });
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        private async Task EliminarClaseEstudiante(int ceId)
        {
            try
            {
                bool confirmacion = await Application.Current.MainPage.DisplayAlert(
                   "Aviso",
                   "¿Estás seguro de que deseas eliminar esta claseEstudiante?",
                   "Sí",
                   "No");
                if (confirmacion)
                {
                    bool resultado = await _claseEstudianteRepository.EliminarClaseEstudiante(ceId);
                    if (resultado)
                    {
                        var claseEliminar = ClaseEstudiantes.FirstOrDefault(c => c.Id == ceId);
                        if (claseEliminar != null)
                        {
                            ClaseEstudiantes.Remove(claseEliminar);
                            await Application.Current.MainPage.DisplayAlert("Eliminado", "ClaseEstudiante eliminado exitosamente", "Ok");
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Error al eliminar claseEstudiante.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        private async Task GuardarCambios()
        {
            try
            {
                var claseEstudianteActualizada = new ClaseEstudiante
                {
                    Id = Id,
                    ClaseId = ClaseId,
                    EstudianteId = EstudianteId
                };
                var resultado = await _claseEstudianteRepository.ActualizarClaseEstudiante(Id, claseEstudianteActualizada);
                if (resultado)
                {
                    await Application.Current.MainPage.DisplayAlert("Actualizado", "ClaseEstudiante actualizada exitosamente", "Ok");
                    await Application.Current.MainPage.Navigation.PushAsync(new VerClaseEstudianteView());
                    await GetClaseEstudiantes();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error al actualizar claseEstudiante", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
