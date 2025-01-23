using System.Diagnostics;

namespace CCVProyecto2v2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
        }
        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            
            if (args.Current.Location.OriginalString == "//InicioView")
            {
                
                Console.WriteLine("El usuario ya está en la página de inicio.");
            }
        }



    }
}