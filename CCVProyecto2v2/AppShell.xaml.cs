using System.Diagnostics;
using CCVProyecto2v2.ViewsGeneral;

namespace CCVProyecto2v2
{
    public partial class AppShell : Shell
    {
        public Command NavigateToInicioCommand { get; }
        public AppShell()
        {
            InitializeComponent();
            NavigateToInicioCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("//InicioView"); 
            });
            BindingContext = this;
        }
    }
}