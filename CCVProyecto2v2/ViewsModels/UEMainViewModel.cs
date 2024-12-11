using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Models;
using CCVProyecto2v2.Utilidades;
using CCVProyecto2v2.ViewsAdmin;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CCVProyecto2v2.ViewsModels
{
    public partial class UEMainViewModel : ObservableObject
    {
        private readonly DbbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<ClaseEstudianteDto> listaClaseEstudiantes = new ObservableCollection<ClaseEstudianteDto>();
        [ObservableProperty]
        private ObservableCollection<ClaseDto> listaClases = new ObservableCollection<ClaseDto>();

        public UEMainViewModel(DbbContext context)
        {
            _dbContext = context;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await ObtenerClases();
                await ObtenerClasesParaPicker();
            });
            WeakReferenceMessenger.Default.Register<UMensajeria>(this, (r, m) =>
            {
                ClaseMensajeRecibido(m.Value);
            });
        }
        public async Task ObtenerClases()
        {
            var lista = await _dbContext.ClaseEstudiantes
                .Include(c => c.Estudiante)
                .Include(c => c.Clase)
                .ThenInclude(c => c.Profesor)
                .ToListAsync();


            var clasesAgrupadas = lista.GroupBy(c => c.Clase.Id) .Select(c => new ClaseEstudianteDto
           {
               ClaseId = c.Key,
               Clase = new ClaseDto
               {
                   Id = c.First().Clase.Id,
                   Nombre = c.First().Clase.Nombre,
                   Profesor = new ProfesorDto
                   {
                       Id = c.First().Clase.Profesor.Id,
                       Nombre = c.First().Clase.Profesor.Nombre
                   }
               },
               Estudiantes = c.Select(c => new EstudianteDto
               {
                   Id = c.Estudiante.Id,
                   Nombre = c.Estudiante.Nombre,
                   Cedula = c.Estudiante.Cedula,
                   Edad = c.Estudiante.Edad,
                   Grado = c.Estudiante.Grado
               }).ToList()
           })
           .ToList();

                ListaClaseEstudiantes.Clear();

                foreach (var clase in clasesAgrupadas)
                {
                    ListaClaseEstudiantes.Add(clase);
                }
        }
        public async Task ObtenerClasesParaPicker()
        {
            var clases = await _dbContext.Clase.ToListAsync();

            ListaClases = new ObservableCollection<ClaseDto>(
                clases.Select(c => new ClaseDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    ProfesorId = c.ProfesorId,
                    //Profesor = new ProfesorDto
                    //{
                    //    Id = c.Profesor.Id,
                    //    Nombre = c.Profesor.Nombre,
                    //} 
                }));
       


        }
        private void ClaseMensajeRecibido(UCuerpo claseCuerpo)
        {
            var claseDto = claseCuerpo.ClaseEstudianteDto;

            if (claseDto == null)
            {
                Debug.WriteLine("El objeto ClaseEstudianteDto es nulo.");
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (claseCuerpo.EsCrear)
                {
                    ListaClaseEstudiantes.Add(claseDto);
                }
                else
                {
                    var encontrada = ListaClaseEstudiantes.FirstOrDefault(c => c.ClaseId == claseDto.ClaseId);
                    if (encontrada != null)
                    {
                        encontrada.Clase.Nombre = claseDto.Clase.Nombre;
                        encontrada.Clase.ProfesorId = claseDto.Clase.ProfesorId;
                        encontrada.Clase.Profesor = claseDto.Clase.Profesor;

                        OnPropertyChanged(nameof(ListaClaseEstudiantes));
                    }
                    else
                    {
                        Debug.WriteLine("No se encontró la clase para actualizar.");
                    }
                }
            });
        }




        [RelayCommand]
        private async Task Crear()
        {
            var uri = $"{nameof(UnirEstudianteView)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Editar(ClaseEstudianteDto claseDto)
        {
            var uri = $"{nameof(UnirEstudianteView)}?id={claseDto.Id}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Eliminar(ClaseEstudianteDto claseDto)
        {
            try
            {
                bool anwser = await Shell.Current.DisplayAlert("Mensaje", "¿Desea eliminar esta clase junto con los estudiantes relacionados?", "Sí", "No");
                if (anwser)
                {
                    var relacionados = await _dbContext.ClaseEstudiantes
                        .Where(c => c.ClaseId == claseDto.ClaseId)
                        .ToListAsync();

                    if (relacionados.Any())
                    {
                        _dbContext.ClaseEstudiantes.RemoveRange(relacionados);

                        await _dbContext.SaveChangesAsync();

                        var elementosAEliminar = ListaClaseEstudiantes
                            .Where(c => c.ClaseId == claseDto.ClaseId)
                            .ToList();

                        foreach (var elemento in elementosAEliminar)
                        {
                            ListaClaseEstudiantes.Remove(elemento);
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "No se encontraron registros relacionados para eliminar.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar la clase y los estudiantes relacionados: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Ocurrió un error al intentar eliminar los datos.", "OK");
            }
        }

        


        [RelayCommand]
        public async Task Guardar(ClaseEstudianteDto claseEstudianteDto)
        {
            var nuevaClaseEstudiante = new ClaseEstudiante
            {
                EstudianteId = claseEstudianteDto.EstudianteId,
                ClaseId = claseEstudianteDto.ClaseId
            };

            if (claseEstudianteDto.Id == 0)
            {
                _dbContext.ClaseEstudiantes.Add(nuevaClaseEstudiante);
                await _dbContext.SaveChangesAsync();

                claseEstudianteDto.Id = nuevaClaseEstudiante.Id;
                var mensaje = new UCuerpo { EsCrear = true, ClaseEstudianteDto = claseEstudianteDto };
                WeakReferenceMessenger.Default.Send(new UMensajeria(new UCuerpo { EsCrear = true }));

            }
            await ObtenerClases();
            await ObtenerClasesParaPicker();
            await Shell.Current.GoToAsync(".."); 
        }
    }
}
