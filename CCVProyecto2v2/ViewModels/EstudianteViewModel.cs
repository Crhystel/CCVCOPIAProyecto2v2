using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Interfaces;
using CCVProyecto2v2.Models;
using CCVProyecto2v2.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CCVProyecto2v2.ViewsModels
{
    public partial class EstudianteViewModel:INotifyPropertyChanged
    {
        private readonly EstudianteRepository _estudianteRepository;

        public ObservableCollection<EstudianteDto> Estudiantes { get; set; }
        public List<GradoEnum> GradosDisponibles { get; } = Enum.GetValues(typeof(GradoEnum)).Cast<GradoEnum>().ToList();
        public string GradoSeleccionado { get; set; }
        public ICommand CrearEstudianteCommand { get; }
        public ICommand EliminarEstudianteCommand { get; }

        private string _nombre;
        private int _edad;
        private string _cedula;
        private string _contrasenia;
        private string _nombreUsuario;
        private string _mensaje;
        private GradoEnum _grado;


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
                Grado = value;
                OnPropertyChanged();
            }
        }

        public EstudianteViewModel(EstudianteRepository estudianteRepository)
        {
            _estudianteRepository = estudianteRepository;
            Estudiantes = new ObservableCollection<EstudianteDto>();
            CrearEstudianteCommand = new Command(async () => await CrearEstudiante());
            EliminarEstudianteCommand = new AsyncRelayCommand<int>(EliminarEstudiante);
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

        private async Task EliminarEstudiante(int estudianteId)
        {
            try
            {
                bool resultado = await _estudianteRepository.EliminarEstudiante(estudianteId);
                if (resultado)
                {
                    var estudianteEliminar = Estudiantes.FirstOrDefault(e => e.Id == estudianteId);
                    if (estudianteEliminar != null)
                    {
                        Estudiantes.Remove(estudianteEliminar);
                    }
                }
                else
                {
                    Console.WriteLine("Error al eliminar el estudiante.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

