
using CCVProyecto2v2.Interfaces;
using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewModels;
using CCVProyecto2v2.Views.ViewsAdmin;
using CCVProyecto2v2.Views.ViewsEstudiante;
using CCVProyecto2v2.Views.ViewsProfesor;
using CCVProyecto2v2.ViewsClase;
using Microsoft.Extensions.Logging;

namespace CCVProyecto2v2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("TheStudentsTeacher-Regular.ttf", "TheStudentsTeacherFont");
                    fonts.AddFont("Schoolwork-Regular.ttf", "SchoolworkFont");

                });
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<IEstudiante, EstudianteRepository>();
            builder.Services.AddSingleton<EstudianteViewModel>();
            builder.Services.AddSingleton<IProfesor, ProfesorRepository>();
            builder.Services.AddSingleton<ProfesorViewModel>();

            builder.Services.AddTransient<VerProfesorView>();
            builder.Services.AddTransient<VerEstudianteView>();
            builder.Services.AddTransient<AgregarEstudianteView>();
            builder.Services.AddTransient<AgregarProfesorView>();
            builder.Services.AddTransient<EditarEstudianteView>();
            builder.Services.AddTransient<EditarProfesorView>();
            builder.Services.AddTransient<CMainPage>();



            //Routing.RegisterRoute(nameof(AgregarEstudianteView), typeof(AgregarEstudianteView));

#if DEBUG
            builder.Logging.AddDebug();
            
#endif

            return builder.Build();
        }
    }
}
