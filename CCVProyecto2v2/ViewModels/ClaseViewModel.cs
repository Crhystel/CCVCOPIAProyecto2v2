using CCVProyecto2v2.Models;
using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.Views.ViewsAdmin;
using CCVProyecto2v2.Views.ViewsClase;
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
    public partial class ClaseViewModel:INotifyPropertyChanged
    {
        private readonly ClaseRepository _claseRepository;
        public ObservableCollection<Clase> Clases { get; set; }
        public ICommand CrearClaseCommand { get; }
        public ICommand EliminarClaseCommand { get; }
        public ICommand ActualizarClaseCommand { get; }
        public ICommand GuardarClaseCommand { get; }
        private int _id;
        private string _nombre;
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
        public ClaseViewModel(ClaseRepository claseRepository)
        {
            _claseRepository = claseRepository;
            Clases = new ObservableCollection<Clase>();
            CrearClaseCommand = new Command(async () => await CrearClase());
            EliminarClaseCommand = new AsyncRelayCommand<int>(EliminarClase);
            ActualizarClaseCommand = new AsyncRelayCommand<int>(ActualizarClase);
            GuardarClaseCommand = new AsyncRelayCommand(GuardarCambios);
        }
        public ClaseViewModel()
        {
            
        }
        public async Task CrearClase()
        {
            try
            {
                var nuevaClase = new Clase
                {
                    Id=0,
                    Nombre = Nombre
                };
                var resultado = await _claseRepository.CrearClase(nuevaClase);
                if (resultado)
                {
                    await Application.Current.MainPage.DisplayAlert("Creado", "Clase creada exitosamente", "Ok");
                    await Application.Current.MainPage.Navigation.PushAsync(new VerClaseView());
                    await GetClases();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error al crear clase", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        public async Task GetClases()
        {
            try
            {
                var clases = await _claseRepository.GetClases();
                Clases.Clear();
                foreach (var clase in clases)
                {
                    Clases.Add(new Clase
                    {
                        Id = clase.Id,
                        Nombre = clase.Nombre
                    });
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        private async Task ActualizarClase(int claseId)
        {
            try
            {
                var clase = await _claseRepository.GetClases();
                var claseSeleccionada = clase.FirstOrDefault(c => c.Id == claseId);
                if (claseSeleccionada != null)
                {
                    Id = claseSeleccionada.Id;
                    Nombre = claseSeleccionada.Nombre;
                    await Application.Current.MainPage.Navigation.PushAsync(new EditarClaseView
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

        private async Task EliminarClase(int claseId)
        {
            try
            {
                bool confirmacion = await Application.Current.MainPage.DisplayAlert(
                   "Aviso",
                   "¿Estás seguro de que deseas eliminar esta clase?",
                   "Sí",
                   "No");
                if (confirmacion)
                {
                    bool resultado = await _claseRepository.EliminarClase(claseId);
                    if (resultado)
                    {
                        var claseEliminar = Clases.FirstOrDefault(c => c.Id == claseId);
                        if (claseEliminar != null)
                        {
                            Clases.Remove(claseEliminar);
                            await Application.Current.MainPage.DisplayAlert("Eliminado", "Estudiante eliminado exitosamente", "Ok");
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Error al eliminar el estudiante.", "OK");
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
                var claseActualizada = new Clase
                {
                    Id = Id,
                    Nombre = Nombre
                };
                var resultado = await _claseRepository.ActualizarClase(Id, claseActualizada);
                if (resultado)
                {
                    await Application.Current.MainPage.DisplayAlert("Actualizado", "Clase actualizada exitosamente", "Ok");
                    await Application.Current.MainPage.Navigation.PushAsync(new VerClaseView());
                    await GetClases();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Error al actualizar clase", "Ok");
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
