using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Interfaces;
using CCVProyecto2v2.Models;
using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.Views.ViewsEstudiante;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CCVProyecto2v2.ViewModels
{
    public partial class EstudianteViewModel:INotifyPropertyChanged
    {
        private readonly EstudianteRepository _estudianteRepository;

        public ObservableCollection<EstudianteDto> Estudiantes { get; set; }
        public List<GradoEnum> GradosDisponibles { get; } = Enum.GetValues(typeof(GradoEnum)).Cast<GradoEnum>().ToList();
       
        public ICommand CrearEstudianteCommand { get; }
        public ICommand EliminarEstudianteCommand { get; }
        public ICommand ActualizarEstudianteCommand { get; }
        public ICommand GuardarEstudianteCommand { get; }

        private int _id;
        private string _nombre;
        private int _edad;
        private string _cedula;
        private string _contrasenia;
        private string _nombreUsuario;
        private string _mensaje;
        private GradoEnum _grado;

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
        public GradoEnum Grado
        {
            get => _grado;
            set
            {
                _grado = value;
                OnPropertyChanged();
            }
        }

        public EstudianteViewModel(EstudianteRepository estudianteRepository)
        {
            _estudianteRepository = estudianteRepository;
            Estudiantes = new ObservableCollection<EstudianteDto>();
            CrearEstudianteCommand = new Command(async () => await CrearEstudiante());
            EliminarEstudianteCommand = new AsyncRelayCommand<int>(EliminarEstudiante);
            ActualizarEstudianteCommand=new AsyncRelayCommand<int>(ActualizarEstudiante);
            GuardarEstudianteCommand = new AsyncRelayCommand(GuardarCambios);
        }
        public EstudianteViewModel()
        {
            
        }
        public async Task CrearEstudiante()
        {
            try
            {
                var nuevoEstudiante = new Estudiante
                {
                    Id = 0,
                    Nombre = Nombre,
                    Edad = Edad,
                    Cedula = Cedula,
                    Contrasenia = Contrasenia,
                    NombreUsuario = NombreUsuario,
                    Grado=Grado,
                    
                };


                var resultado = await _estudianteRepository.CrearEstudiante(Grado, nuevoEstudiante);

                if (resultado)
                {
                    Mensaje = "Estudiante creado exitosamente.";
                    await GetEstudiantes(); 
                }
                else
                {
                    Mensaje = "Error al crear el estudiante.";
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error: {ex.Message}";
            }
        }

        public async Task GetEstudiantes()
        {
            try
            {
                var estudiantes = await _estudianteRepository.GetEstudiantes();
                Estudiantes.Clear();
                foreach (var estudiante in estudiantes)
                {
                    Estudiantes.Add(new EstudianteDto
                    {
                        Id = estudiante.Id,
                        Cedula = estudiante.Cedula,
                        Nombre = estudiante.Nombre,
                        Rol = estudiante.Rol,
                        NombreUsuario = estudiante.NombreUsuario,
                        Contrasenia = estudiante.Contrasenia,
                        Edad = estudiante.Edad,
                        Grado = estudiante.Grado,
                    });
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error: {ex.Message}";
            }
        }
       
                  
        private async Task ActualizarEstudiante(int estudianteId)
        {
            var estudiante = await _estudianteRepository.GetEstudiantes();
            var estudianteSeleccionado = estudiante.FirstOrDefault(c => c.Id == estudianteId);
            if (estudianteSeleccionado != null)
            {
                Id = estudianteSeleccionado.Id;
                Nombre = estudianteSeleccionado.Nombre;
                Edad = estudianteSeleccionado.Edad;
                Cedula = estudianteSeleccionado.Cedula;
                Contrasenia = estudianteSeleccionado.Contrasenia;
                NombreUsuario = estudianteSeleccionado.NombreUsuario;
                Grado = estudianteSeleccionado.Grado;
                await Application.Current.MainPage.Navigation.PushAsync(new EditarEstudianteView
                {
                    BindingContext = this
                });
            }
        }
        private async Task EliminarEstudiante(int estudianteId)
        {
            try
            {
                bool resultado = await _estudianteRepository.EliminarEstudiante(estudianteId);
                if (resultado)
                {
                    var estudianteEliminar = Estudiantes.FirstOrDefault(c => c.Id == estudianteId);
                    if (estudianteEliminar != null)
                    {
                        Estudiantes.Remove(estudianteEliminar);
                    }
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task GuardarCambios()
        {
            try
            {
                var estudianteActualizado = new Estudiante
                {
                    Id = Id,
                    Nombre = Nombre,
                    Edad = Edad,
                    Cedula = Cedula,
                    Contrasenia = Contrasenia,
                    NombreUsuario = NombreUsuario,
                    Grado = Grado,
                };
                var resultado = await _estudianteRepository.ActualizarEstudiante(Id,estudianteActualizado);
                if (resultado)
                {
                    Mensaje = "Cambios guardados exitosamente.";
                    var estudianteEnLista = Estudiantes.FirstOrDefault(e => e.Id == Id);
                    if (estudianteEnLista != null)
                    {
                        estudianteEnLista.Nombre = Nombre;
                        estudianteEnLista.Edad = Edad;
                        estudianteEnLista.Cedula = Cedula;
                        estudianteEnLista.Contrasenia = Contrasenia;
                        estudianteEnLista.NombreUsuario = NombreUsuario;
                        estudianteEnLista.Grado = Grado;
                    }
                }
                else
                {
                    Mensaje = "Error al guardar los cambios.";
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error: {ex.Message}";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

