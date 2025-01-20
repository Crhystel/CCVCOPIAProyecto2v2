
using CCVProyecto2v2.Interfaces;
using CCVProyecto2v2.Repositories;
using CCVProyecto2v2.ViewsAdmin;
using CCVProyecto2v2.ViewsClase;
using CCVProyecto2v2.ViewsModels;
using CCVProyecto2v2.ViewsProfesor;
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
            builder.Services.AddTransient<AgregarEstudianteView>();
            builder.Services.AddSingleton<EstudianteViewModel>();
            builder.Services.AddTransient<PMainPage>();

            builder.Services.AddTransient<CMainPage>();



            Routing.RegisterRoute(nameof(AgregarEstudianteView), typeof(AgregarEstudianteView));

#if DEBUG
            builder.Logging.AddDebug();
            
#endif

            return builder.Build();
        }
    }
}
