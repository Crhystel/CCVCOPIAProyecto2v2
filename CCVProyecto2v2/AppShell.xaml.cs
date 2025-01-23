using System.Diagnostics;

namespace CCVProyecto2v2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
        }
        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (args.Target.Location.OriginalString != "//InicioView")
            {
             
                args.Cancel();
                Shell.Current.GoToAsync("//InicioView");
            }
        }


    }
}