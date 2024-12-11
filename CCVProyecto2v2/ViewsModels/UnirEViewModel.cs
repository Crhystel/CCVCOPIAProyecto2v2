

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
                await CrearClaseEstudiante();
                CargarListaClaseEstudiantes();
            });
            WeakReferenceMessenger.Default.Register<EMensajeria>(this, (r, m) =>
            {
                ClasesActualizadas(m.Value);
            });
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


        [RelayCommand]
        public async Task GuardarMultiple()
        {
            try
            {
                LoadingClaseEstudiante = true;

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
                }

                await _dbContext.SaveChangesAsync();
                await CargarClases();
                await CargarDatos();

                WeakReferenceMessenger.Default.Send(new UMensajeria(new UCuerpo
                {
                    EsCrear = true,
                    ClaseEstudianteDto = ClaseEstudianteDto
                }));

                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                LoadingClaseEstudiante = false;
            }
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
                    await CargarDatos();
                }
            });
        }
        [RelayCommand]
        public async Task CrearClaseEstudiante()
        {
            if (ClaseEstudianteDto != null && ClaseEstudianteDto.ClaseId > 0 && ClaseEstudianteDto.EstudianteId > 0)
            {
                try
                {
                    var existente = _dbContext.ClaseEstudiantes.FirstOrDefault(
                        ce => ce.ClaseId == ClaseEstudianteDto.ClaseId &&
                              ce.EstudianteId == ClaseEstudianteDto.EstudianteId);

                    if (existente == null)
                    {
                        _dbContext.ClaseEstudiantes.Add(new ClaseEstudiante
                        {
                            ClaseId = ClaseEstudianteDto.ClaseId,
                            EstudianteId = ClaseEstudianteDto.EstudianteId
                        });
                        await _dbContext.SaveChangesAsync();

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Esta relación ya existe.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }
        public void CargarListaClaseEstudiantes()
        {
            ListaClaseEstudiantes.Clear();
            var relaciones = _dbContext.ClaseEstudiantes
                .Include(ce => ce.Clase)
                .Include(ce => ce.Estudiante)
                .ToList();

            foreach (var rel in relaciones)
            {
                ListaClaseEstudiantes.Add(new ClaseEstudianteDto
                {
                    ClaseId = rel.ClaseId,
                    EstudianteId = rel.EstudianteId,
                    Clase = new ClaseDto
                    {
                        Nombre = rel.Clase.Nombre,
                        Profesor = new ProfesorDto { Nombre = rel.Clase.Profesor.Nombre }
                    },
                    Estudiantes = rel.Estudiante != null ? new List<EstudianteDto> { new EstudianteDto { Nombre = rel.Estudiante.Nombre } } : new List<EstudianteDto>()
                });
            }
        }


    }
}
