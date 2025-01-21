﻿using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Models;
using CCVProyecto2v2.Repositories;
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

        private string _nombre;
        private int _edad;
        private string _cedula;
        private string _contrasenia;
        private string _nombreUsuario;
        private string _mensaje;
        private MateriaEnum _materia;

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
                    Mensaje = "Profesor creado exitosamente";
                    await GetProfesores();
                }
                else
                {
                    Mensaje = "Error al crear el estudiante";
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
        private async Task EliminarProfesor(int profesorId)
        {
            try
            {
                bool resultado = await _profesorRepository.EliminarProfesor(profesorId);
                if (resultado)
                {
                    var profesorEliminar = Profesores.FirstOrDefault(c => c.Id == profesorId);
                    if(profesorEliminar != null)
                    {
                        Profesores.Remove(profesorEliminar);
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
