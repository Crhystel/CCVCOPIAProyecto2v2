using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Models;
using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.Views.ViewsAdmin;
using CCVProyecto2v2.Views.ViewsProfesor;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CCVProyecto2v2.ViewModels
{
    public partial class ProfesorViewModel:INotifyPropertyChanged
    {
        private readonly ProfesorRepository _profesorRepository;
        public ObservableCollection<ProfesorDto> Profesores { get; set; }
        public List<MateriaEnum> MateriasDsiponibles { get; }=Enum.GetValues(typeof(MateriaEnum)).Cast<MateriaEnum>().ToList();
        public ICommand CrearProfesorCommand { get; }
        public ICommand EliminarProfesorCommand { get; }
        public ICommand ActualizarProfesorCommand { get; }
        public ICommand GuardarProfesorCommand { get; }

        private int _id;
        private string _nombre;
        private int _edad;
        private string _cedula;
        private string _contrasenia;
        private string _nombreUsuario;
        private string _mensaje;
        private MateriaEnum _materia;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); }
        }

        public int Edad
        {
            get => _edad;
            set { _edad = value; OnPropertyChanged(); }
        }

        public string Cedula
        {
            get => _cedula;
            set { _cedula = value; OnPropertyChanged(); }
        }

        public string Contrasenia
        {
            get => _contrasenia;
            set { _contrasenia = value; OnPropertyChanged(); }
        }

        public string NombreUsuario
        {
            get => _nombreUsuario;
            set { _nombreUsuario = value; OnPropertyChanged(); }
        }

        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; OnPropertyChanged(); }
        }
        public MateriaEnum Materia
        {
            get => _materia;
            set
            {
                _materia = value;
                OnPropertyChanged();
            }
        }

        public ProfesorViewModel(ProfesorRepository profesorRepository)
        {
            _profesorRepository = profesorRepository;
            Profesores = new ObservableCollection<ProfesorDto>();
            CrearProfesorCommand = new Command(async () => await CrearProfesor());
            EliminarProfesorCommand = new AsyncRelayCommand<int>(EliminarProfesor);
            ActualizarProfesorCommand=new AsyncRelayCommand<int>(ActualizarProfesor);
            GuardarProfesorCommand = new AsyncRelayCommand(GuardarCambios);
        }
        public ProfesorViewModel()
        {
            
        }
        public async Task CrearProfesor()
        {
            try
            {
                var nuevoProfesor = new Profesor
                {
                    Id = 0,
                    Nombre = Nombre,
                    Edad = Edad,
                    Cedula = Cedula,
                    Contrasenia = Contrasenia,
                    NombreUsuario = NombreUsuario,
                    Materia = Materia,
                };
                var resultado = await _profesorRepository.CrearProfesor(Materia, nuevoProfesor);
                if (resultado)
                {
                    await Application.Current.MainPage.DisplayAlert("Creado", "Profesor creado exitosamente", "Ok");
                    await Application.Current.MainPage.Navigation.PushAsync(new VerProfesorView());
                    await GetProfesores();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error al crear el profesor.", "OK");
                }
            }
            catch(Exception ex) 
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        public async Task GetProfesores()
        {
            try
            {
                var profesores = await _profesorRepository.GetProfesores();
                Profesores.Clear();
                foreach(var profesor in profesores)
                {
                    Profesores.Add(new ProfesorDto
                    {
                        Id = profesor.Id,
                        Cedula = profesor.Cedula,
                        Nombre = profesor.Nombre,
                        Rol = profesor.Rol,
                        NombreUsuario = profesor.NombreUsuario,
                        Contrasenia = profesor.Contrasenia,
                        Edad = profesor.Edad,
                        Materia = profesor.Materia,
                    });
                }
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        private async Task ActualizarProfesor(int profesorId)
        {
            var profesor = await _profesorRepository.GetProfesores();
            var profesorSeleccionado = profesor.FirstOrDefault(c => c.Id == profesorId);
            if (profesorSeleccionado != null)
            {
                Nombre = profesorSeleccionado.Nombre;
                Edad = profesorSeleccionado.Edad;
                Cedula = profesorSeleccionado.Cedula;
                Contrasenia = profesorSeleccionado.Contrasenia;
                NombreUsuario = profesorSeleccionado.NombreUsuario;
                Materia = profesorSeleccionado.Materia;
                await Application.Current.MainPage.Navigation.PushAsync(new EditarProfesorView
                {
                    BindingContext = this
                });
            }
        }
        private async Task GuardarCambios()
        {
            try
            {
                var profesorActualizado = new Profesor
                {
                    Id = Id,
                    Nombre = Nombre,
                    Edad = Edad,
                    Cedula = Cedula,
                    Contrasenia = Contrasenia,
                    NombreUsuario = NombreUsuario,
                    Materia = Materia,
                };
                var resultado=await _profesorRepository.ActualizarProfesor(Id,profesorActualizado);
                if (resultado)
                {
                    var profesorExistente = Profesores.FirstOrDefault(c => c.Id == Id);
                    if(profesorExistente != null)
                    {
                        profesorExistente.Nombre = Nombre;
                        profesorExistente.Edad = Edad;
                        profesorExistente.Cedula = Cedula;
                        profesorExistente.Contrasenia = Contrasenia;
                        profesorExistente.NombreUsuario = NombreUsuario;
                        profesorExistente.Materia = Materia;
                    }
                    await Application.Current.MainPage.DisplayAlert("Guardado", "Cambios guardados exitosamente", "Ok");
                    await Application.Current.MainPage.Navigation.PushAsync(new VerProfesorView());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error al guardar los cambios.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        private async Task EliminarProfesor(int profesorId)
        {
            try
            {
                bool confirmacion = await Application.Current.MainPage.DisplayAlert(
                   "Aviso",
                   "¿Estás seguro de que deseas eliminar este profesor?",
                   "Sí",
                   "No");

                if (confirmacion)
                {
                    bool resultado = await _profesorRepository.EliminarProfesor(profesorId);
                    if (resultado)
                    {
                        var estudianteEliminar = Profesores.FirstOrDefault(c => c.Id == profesorId);
                        if (estudianteEliminar != null)
                        {
                            Profesores.Remove(estudianteEliminar);
                            await Application.Current.MainPage.DisplayAlert("Eliminado", "Profesor eliminado exitosamente", "Ok");
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Error al eliminar el profesor.", "OK");
                    }
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
