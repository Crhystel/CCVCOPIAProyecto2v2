using CCVProyecto2v2.Models;
using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.Views.ViewClaseProfesor;
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
    public partial class ClaseProfesorViewModel:INotifyPropertyChanged
    {
        private readonly ClaseProfesorRepository _claseProfesorRepository;
        private readonly ClaseRepository _claseRepository;
        private readonly ProfesorRepository _profesorRepository;

        public ObservableCollection<ClaseProfesor> ClaseProfesores { get; set; }
        public ObservableCollection<Profesor> Profesors { get; set; } = new ObservableCollection<Profesor>();
        public ObservableCollection<Clase> Clases { get; set; } = new ObservableCollection<Clase>();
        public ICommand CrearClaseProfesorCommand { get; }
        public ICommand EliminarClaseProfesorCommand { get; }
        public ICommand ActualizarClaseProfesorCommand { get; }
        public ICommand GuardarClaseProfesorCommand { get; }


        private int _id;
        private int _claseId;
        private int _profesorId;
        private string _profesorNombre;
        private string _claseNombre;
        private Clase _selectedClase;
        private Profesor _selectedProfesor;

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
        public int ProfesorId
        {
            get => _profesorId;
            set { _profesorId = value; OnPropertyChanged(); }
        }
        public string ProfesorNombre
        {
            get => _profesorNombre;
            set { _profesorNombre = value; OnPropertyChanged(); }
        }
        public string ClaseNombre
        {
            get => _claseNombre;
            set { _claseNombre = value; OnPropertyChanged(); }
        }
        public Clase SelectedClase
        {
            get => _selectedClase;
            set
            {
                if (_selectedClase != value)
                {
                    _selectedClase = value;
                    ClaseId = _selectedClase?.Id ?? 0;
                    OnPropertyChanged();
                }
            }
        }
        public Profesor SelectedProfesor
        {
            get => _selectedProfesor;
            set
            {
                if (_selectedProfesor != value)
                {
                    _selectedProfesor = value;
                    ProfesorId = _selectedProfesor?.Id ?? 0;
                    OnPropertyChanged();
                }
            }
        }
        public ClaseProfesorViewModel(ClaseProfesorRepository claseProfesorRepository,
            ClaseRepository claseRepository,
            ProfesorRepository profesorRepository)
        {
            _claseProfesorRepository = claseProfesorRepository;
            _claseRepository = claseRepository;
            _profesorRepository = profesorRepository;
            ClaseProfesores = new ObservableCollection<ClaseProfesor>();
            CrearClaseProfesorCommand = new Command(async () => await CrearClaseProfesor());
            EliminarClaseProfesorCommand = new AsyncRelayCommand<int>(EliminarClaseProfesor);
            ActualizarClaseProfesorCommand = new AsyncRelayCommand<int>(ActualizarClaseProfesor);
            GuardarClaseProfesorCommand = new AsyncRelayCommand(GuardarCambios);

        }
        public ClaseProfesorViewModel()
        {

        }
        public async Task CrearClaseProfesor()
        {
            try
            {
                if (SelectedClase == null || SelectedProfesor == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar una clase y un profesor.", "OK");
                    return;
                }
                var nuevaClaseProfesor = new ClaseProfesor
                {
                    Id = Id,
                    ClasePId = SelectedClase.Id,
                    ProfesorId = SelectedProfesor.Id,
                    ClaseNombre = SelectedClase.Nombre,
                    ProfesorNombre = SelectedProfesor.Nombre
                };
                var resultado = await _claseProfesorRepository.CrearClaseProfesor(nuevaClaseProfesor);
                if (resultado)
                {
                    await Application.Current.MainPage.DisplayAlert("Creado", "ClaseEstudiante creada exitosamente", "Ok");
                    await Application.Current.MainPage.Navigation.PushAsync(new VerClaseProfesorView());
                    await GetClaseProfesores();
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
        public async Task GetClaseProfesores()
        {
            try
            {
                var claseProfesorss = await _claseProfesorRepository.GetClaseProfesores();
                ClaseProfesores.Clear();
                foreach (var claseProfesor in claseProfesorss)
                {
                    ClaseProfesores.Add(new ClaseProfesor
                    {
                        Id = claseProfesor.Id,
                        ClasePId = claseProfesor.ClasePId,
                        ProfesorId = claseProfesor.ProfesorId,
                        ProfesorNombre = claseProfesor.ProfesorNombre,
                        ClaseNombre = claseProfesor.ClaseNombre
                    });
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        private async Task ActualizarClaseProfesor(int ceId)
        {
            try
            {
                await CargarDatosPickers();
                var claseProfesor = await _claseProfesorRepository.GetClaseProfesores();
                var claseProfesorSeleccionada = claseProfesor.FirstOrDefault(c => c.Id == ceId);
                if (claseProfesorSeleccionada != null)
                {
                    Id = claseProfesorSeleccionada.Id;
                    ClaseId = claseProfesorSeleccionada.ClasePId;
                    ProfesorId = claseProfesorSeleccionada.ProfesorId;
                    ProfesorNombre = claseProfesorSeleccionada.ProfesorNombre;
                    ClaseNombre = claseProfesorSeleccionada.ClaseNombre;

                    SelectedProfesor = Profesors.FirstOrDefault(e => e.Id == claseProfesorSeleccionada.ProfesorId);
                    SelectedClase = Clases.FirstOrDefault(c => c.Id == claseProfesorSeleccionada.ClasePId);
                    await Application.Current.MainPage.Navigation.PushAsync(new EditarClaseProfesorView
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
        private async Task EliminarClaseProfesor(int ceId)
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
                    bool resultado = await _claseProfesorRepository.EliminarClaseProfesor(ceId);
                    if (resultado)
                    {
                        var claseEliminar = ClaseProfesores.FirstOrDefault(c => c.Id == ceId);
                        if (claseEliminar != null)
                        {
                            ClaseProfesores.Remove(claseEliminar);
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
                var claseProfesorActualizada = new ClaseProfesor
                {
                    Id = Id,
                    ClasePId = ClaseId,
                    ProfesorId = ProfesorId,
                    ClaseNombre = ClaseNombre,
                    ProfesorNombre = ProfesorNombre
                };
                var resultado = await _claseProfesorRepository.ActualizarClaseProfesor(Id, claseProfesorActualizada);
                if (resultado)
                {
                    var claseProfesorExistente = ClaseProfesores.FirstOrDefault(c => c.Id == Id);
                    if (claseProfesorExistente != null)
                    {
                        claseProfesorExistente.ClasePId = ClaseId;
                        claseProfesorExistente.ProfesorId = ProfesorId;
                        claseProfesorExistente.ClaseNombre = ClaseNombre;
                        claseProfesorExistente.ProfesorNombre = ProfesorNombre;
                    }
                    await Application.Current.MainPage.DisplayAlert("Actualizado", "ClaseProfesor actualizada exitosamente", "Ok");
                    await Application.Current.MainPage.Navigation.PushAsync(new VerClaseProfesorView());
                    await GetClaseProfesores();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error al actualizar claseProfesor", "Ok");
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
                var profesores = await _profesorRepository.GetProfesores();
                Profesors.Clear();
                foreach (var profesor in profesores)
                {
                    Profesors.Add(profesor);
                }
                OnPropertyChanged(nameof(Clases));
                OnPropertyChanged(nameof(Profesors));
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
