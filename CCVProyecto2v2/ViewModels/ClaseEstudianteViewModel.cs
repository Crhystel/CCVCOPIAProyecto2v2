using CCVProyecto2v2.Dto;
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
        private readonly ClaseRepository _claseRepository;
        private readonly EstudianteRepository _estudianteRepository;
        public ObservableCollection<ClaseEstudiante> ClaseEstudiantes {  get; set; }
        public ObservableCollection<Estudiante> Estudiantes { get; set; } = new ObservableCollection<Estudiante>();
        public ObservableCollection<Clase> Clases { get; set; } = new ObservableCollection<Clase>();
        public ICommand CrearClaseEstudianteCommand {  get; } 
        public ICommand EliminarClaseEstudianteCommand { get; }
        public ICommand ActualizarClaseEstudianteCommand { get; }
        public ICommand GuardarClaseEstudianteCommand { get; }
        public ICommand FiltrarResultadosCommand { get; }

        private int _id;
        private int _claseId;
        private int _estudianteId;
        private string _estudianteNombre;
        private string _claseNombre;
        private Clase _selectedClase;
        private Estudiante _selectedEstudiante;
        private string _filtroResultados;

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
        public string EstudianteNombre
        {
            get => _estudianteNombre;
            set { _estudianteNombre = value;OnPropertyChanged(); }
        }
        public string ClaseNombre
        {
            get => _claseNombre;
            set { _claseNombre = value;OnPropertyChanged(); }
        }
        public Clase SelectedClase
        {
            get => _selectedClase;
            set
            {
                if (_selectedClase != value)
                {
                    _selectedClase = value;
                    ClaseId=_selectedClase?.Id ?? 0;
                    OnPropertyChanged();
                }
            }
        }
        public Estudiante SelectedEstudiante
        {
            get => _selectedEstudiante;
            set
            {
                if (_selectedEstudiante != value)
                {
                    _selectedEstudiante = value;
                    EstudianteId = _selectedEstudiante?.Id ?? 0;
                    OnPropertyChanged();
                }
            }
        }
        public string FiltroResultados
        {
            get => _filtroResultados;
            set
            {
                _filtroResultados = value;
                OnPropertyChanged();
                FiltrarResultados();
            }
        }

        public ClaseEstudianteViewModel(ClaseEstudianteRepository claseEstudianteRepository,
            ClaseRepository claseRepository,
            EstudianteRepository estudianteRepository)
        {
            _claseEstudianteRepository = claseEstudianteRepository;
            _claseRepository = claseRepository;
            _estudianteRepository = estudianteRepository;
            ClaseEstudiantes =new ObservableCollection<ClaseEstudiante>();
            CrearClaseEstudianteCommand = new Command(async () => await CrearClaseEstudiante());
            EliminarClaseEstudianteCommand = new AsyncRelayCommand<int>(EliminarClaseEstudiante);
            ActualizarClaseEstudianteCommand = new AsyncRelayCommand<int>(ActualizarClaseEstudiante);
            GuardarClaseEstudianteCommand = new AsyncRelayCommand(GuardarCambios);
            FiltrarResultadosCommand = new Command(FiltrarResultados);
            
        }
        public ClaseEstudianteViewModel()
        {
            
        }
        public async Task CrearClaseEstudiante()
        {
            try
            {
                if (SelectedClase == null || SelectedEstudiante == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar una clase y un estudiante.", "OK");
                    return;
                }
                var nuevaClaseEstudiante = new ClaseEstudiante
                {
                    Id = Id,
                    ClaseId=SelectedClase.Id,
                    EstudianteId=SelectedEstudiante.Id,
                    ClaseNombre=SelectedClase.Nombre,
                    EstudianteNombre=SelectedEstudiante.Nombre
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
                        EstudianteId = claseEstudiante.EstudianteId,
                        EstudianteNombre = claseEstudiante.EstudianteNombre,
                        ClaseNombre = claseEstudiante.ClaseNombre
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
                await CargarDatosPickers();
                var claseEstudiante= await _claseEstudianteRepository.GetClaseEstudiantes();
                var claseEstudianteSeleccionada = claseEstudiante.FirstOrDefault(c => c.Id == ceId);
                if (claseEstudianteSeleccionada != null)
                {
                    Id= claseEstudianteSeleccionada.Id;
                    ClaseId = claseEstudianteSeleccionada.ClaseId;
                    EstudianteId = claseEstudianteSeleccionada.EstudianteId;
                    EstudianteNombre = claseEstudianteSeleccionada.EstudianteNombre;
                    ClaseNombre = claseEstudianteSeleccionada.ClaseNombre;

                    SelectedEstudiante = Estudiantes.FirstOrDefault(e => e.Id == claseEstudianteSeleccionada.EstudianteId);
                    SelectedClase = Clases.FirstOrDefault(c => c.Id == claseEstudianteSeleccionada.ClaseId);
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
                    EstudianteId = EstudianteId,
                    ClaseNombre=ClaseNombre,
                    EstudianteNombre=EstudianteNombre
                };
                var resultado = await _claseEstudianteRepository.ActualizarClaseEstudiante(Id, claseEstudianteActualizada);
                if (resultado)
                {
                    var claseEstudianteExistente = ClaseEstudiantes.FirstOrDefault(c => c.Id == Id);
                    if(claseEstudianteExistente != null)
                    {
                        claseEstudianteExistente.ClaseId = ClaseId;
                        claseEstudianteExistente.EstudianteId = EstudianteId;
                        claseEstudianteExistente.ClaseNombre = ClaseNombre;
                        claseEstudianteExistente.EstudianteNombre = EstudianteNombre;
                    }
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
        public async Task CargarDatosPickers()
        {
            try
            {
                var clases = await _claseRepository.GetClases();
                Clases.Clear();
                foreach (var clase in clases)
                {
                    Clases.Add(clase);
                }
                var estudiantes = await _estudianteRepository.GetEstudiantes();
                Estudiantes.Clear();
                foreach (var estudiante in estudiantes)
                {
                    Estudiantes.Add(estudiante);
                }
                OnPropertyChanged(nameof(Clases));
                OnPropertyChanged(nameof(Estudiantes));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        private void FiltrarResultados()
        {
            if (string.IsNullOrWhiteSpace(FiltroResultados))
            {
                GetClaseEstudiantes().ConfigureAwait(false);
            }
            else
            {
                var resultadosFiltrados = ClaseEstudiantes
            .Where(ce => (ce.EstudianteNombre?.Contains(FiltroResultados, StringComparison.OrdinalIgnoreCase) ?? false) ||
                         (ce.ClaseNombre?.Contains(FiltroResultados, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();

                ClaseEstudiantes.Clear();
                foreach (var item in resultadosFiltrados)
                {
                    ClaseEstudiantes.Add(item);
                }
            }

        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
