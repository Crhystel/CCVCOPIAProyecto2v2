using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Interfaces;
using CCVProyecto2v2.Models;
using CCVProyecto2v2.Repositories;
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
        private bool _isBusy;


        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if(_isBusy!= value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }
        public EstudianteViewModel(EstudianteRepository estudianteRepository)
        {
            _estudianteRepository = estudianteRepository;
            Estudiantes = new ObservableCollection<EstudianteDto>();
        }
        public EstudianteViewModel()
        {
            
        }
        public async Task GetEstudiantes()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
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
                throw new Exception(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}

