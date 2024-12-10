
using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Models;
using CCVProyecto2v2.Utilidades;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CCVProyecto2v2.ViewsModels
{
    public partial class UnirEViewModel : ObservableObject, IQueryAttributable
    {
        private readonly DbbContext _dbContext;

        [ObservableProperty]
        private ClaseEstudianteDto claseEstudianteDto = new ClaseEstudianteDto();

        [ObservableProperty]
        private ObservableCollection<ClaseEstudianteDto> listaClaseEstudiantes = new();

        [ObservableProperty]
        private ObservableCollection<ClaseDto> clasesDisponibles = new();

        [ObservableProperty]
        private ObservableCollection<EstudianteDto> estudiantesSeleccionados = new();

        [ObservableProperty]
        private string tituloPagina;

        private int IdClaseEstudiante;

        [ObservableProperty]
        private bool loadingClaseEstudiante = false;

        public UnirEViewModel(DbbContext context)
        {
            _dbContext = context;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await CargarDatos();
                await CargarClases();
                await CargarEstudiantesPorClase();
            });
            WeakReferenceMessenger.Default.Register<EMensajeria>(this, (r, m) =>
            {
                ClasesActualizadas(m.Value);
            });
        }
        public UnirEViewModel()
        {
            
        }
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse(query["id"].ToString());
            //LoadingClaseEstudiante = true;
            IdClaseEstudiante = id;

            if (IdClaseEstudiante == 0)
            {
                TituloPagina = "Unir Estudiante";
            }
            else
            {
                TituloPagina = "Editar";
                LoadingClaseEstudiante = true;
                await Task.Run(async () =>
                {
                    var encontrado = await _dbContext.ClaseEstudiantes
                    .Include(c => c.Clase)
                    .Include(c => c.Estudiante)
                    .FirstAsync(c => c.Id == id);
                    if (encontrado != null)
                    {
                        ClaseEstudianteDto = new ClaseEstudianteDto
                        {
                            Id = encontrado.Id,
                            ClaseId = encontrado.ClaseId,
                            EstudianteId = encontrado.EstudianteId,
                            Estudiante = new EstudianteDto
                            {
                                Id = encontrado.Estudiante.Id,
                                Nombre = encontrado.Estudiante.Nombre,
                                Grado = encontrado.Estudiante.Grado
                            },
                            Clase = new ClaseDto
                            {
                                Id = encontrado.Clase.Id,
                                Nombre = encontrado.Clase.Nombre
                            }
                        };
                    }
                    MainThread.BeginInvokeOnMainThread(() => { LoadingClaseEstudiante = false; });
                });
            }
        }

        //[RelayCommand]
        //public async Task Guardar()
        //{
        //    LoadingClaseEstudiante = true;
        //    var mensaje = new Cuerpo();

        //    await Task.Run(async () =>
        //    {
        //        if (IdClaseEstudiante == 0)
        //        {
        //            var nuevaClaseEstudiante = new ClaseEstudiante
        //            {
        //                EstudianteId = ClaseEstudianteDto.EstudianteId,
        //                ClaseId = ClaseEstudianteDto.ClaseId
        //            };

        //            _dbContext.ClaseEstudiantes.Add(nuevaClaseEstudiante);
        //            await _dbContext.SaveChangesAsync();

        //            ClaseEstudianteDto.Id = nuevaClaseEstudiante.Id;

        //            mensaje = new Cuerpo
        //            {
        //                EsCrear = true,
        //                ClaseEstudianteDto = ClaseEstudianteDto
        //            };
        //        }
        //        else
        //        {
        //            var encontrado = await _dbContext.ClaseEstudiantes
        //                .FirstOrDefaultAsync(c => c.Id == IdClaseEstudiante);

        //            if (encontrado != null)
        //            {
        //                encontrado.EstudianteId = ClaseEstudianteDto.EstudianteId;
        //                encontrado.ClaseId = ClaseEstudianteDto.ClaseId;

        //                await _dbContext.SaveChangesAsync();

        //                mensaje = new Cuerpo
        //                {
        //                    EsCrear = false,
        //                    ClaseEstudianteDto = ClaseEstudianteDto
        //                };
        //            }
        //        }

        //        MainThread.BeginInvokeOnMainThread(() =>
        //        {
        //            LoadingClaseEstudiante = false;
        //            WeakReferenceMessenger.Default.Send(new Mensajeria(mensaje));
        //            Shell.Current.Navigation.PopAsync();
        //        });
        //    });
                
            
            
        //}

        [RelayCommand]
        public async Task GuardarMultiple()
        {
            
            await Task.Run(async () =>
            {
                LoadingClaseEstudiante = true;
                UCuerpo mensaje= new UCuerpo();
                await Task.Run(async () =>
                {
                    if (ClaseEstudianteDto.ClaseId == 0)
                    {
                        await Shell.Current.DisplayAlert("Error", "Selecciona una clase válida.", "OK");
                        return;
                    }
                    foreach (var estudiante in EstudiantesSeleccionados.Where(c => c.IsSelected))
                    {
                        var nuevaClaseEstudiante = new ClaseEstudiante
                        {
                            EstudianteId = estudiante.Id,
                            ClaseId = ClaseEstudianteDto.ClaseId
                        };

                        _dbContext.ClaseEstudiantes.Add(nuevaClaseEstudiante);
                        await _dbContext.SaveChangesAsync();
                    }
                    mensaje = new UCuerpo
                    {
                        EsCrear = true,
                        ClaseEstudianteDto = ClaseEstudianteDto
                    };

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        LoadingClaseEstudiante = false;
                        WeakReferenceMessenger.Default.Send(new UMensajeria(mensaje));
                        Shell.Current.Navigation.PopAsync();
                    });
                });
                
            });
        }


        public async Task CargarClases()
        {
            var clases = await _dbContext.Clase.Include(c => c.Profesor).ToListAsync();
            ClasesDisponibles.Clear();

            foreach (var clase in clases)
            {
                ClasesDisponibles.Add(new ClaseDto
                {
                    Id = clase.Id,
                    Nombre = clase.Nombre,
                    ProfesorId = clase.ProfesorId,
                    Profesor = new ProfesorDto
                    {
                        Id = clase.Profesor.Id,
                        Nombre = clase.Profesor.Nombre
                    }
                });
            }
        }


        public async Task CargarEstudiantesPorClase()
        {
            var claseEstudiantes = await _dbContext.ClaseEstudiantes
                .Include(ce => ce.Clase)
                .Include(ce => ce.Estudiante)
                .ToListAsync();

            ListaClaseEstudiantes = new ObservableCollection<ClaseEstudianteDto>(
                claseEstudiantes.Select(ce => new ClaseEstudianteDto
                {
                    Id = ce.Id,
                    Clase = new ClaseDto
                    {
                        Id = ce.Clase.Id,
                        Nombre = ce.Clase.Nombre,
                        Profesor = new ProfesorDto
                        {
                            Id = ce.Clase.Profesor.Id,
                            Nombre = ce.Clase.Profesor.Nombre
                        }
                    },
                    Estudiante = new EstudianteDto
                    {
                        Id = ce.Estudiante.Id,
                        Nombre = ce.Estudiante.Nombre
                    }
                }));
        }

        public async Task CargarDatos()
        {
            var estudiantes = await _dbContext.Estudiante.ToListAsync();
            EstudiantesSeleccionados.Clear();
            foreach (var estudiante in estudiantes)
            {
                EstudiantesSeleccionados.Add(new EstudianteDto
                {
                    Id = estudiante.Id,
                    Nombre = estudiante.Nombre,
                    IsSelected = false
                });
            }
        }
        private void ClasesActualizadas(ECuerpo mensaje)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (mensaje.EsCrear)
                {
                    await CargarClases();
                }
            });
        }

    }
}
