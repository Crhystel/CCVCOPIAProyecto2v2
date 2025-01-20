using CCVProyecto2v2.Dto;
using CCVProyecto2v2.Models;

namespace CCVProyecto2v2
{
    public partial class App : Application
    {
        public static Estudiante estudiante;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
