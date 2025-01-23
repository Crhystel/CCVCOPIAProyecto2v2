using System.Diagnostics;
using CCVProyecto2v2.ViewsGeneral;

namespace CCVProyecto2v2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(InicioView), typeof(InicioView));
            Routing.RegisterRoute(nameof(SoporteView), typeof(SoporteView));
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var currentShell = Shell.Current;
        }


    }
}