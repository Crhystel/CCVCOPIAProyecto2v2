using CCVProyecto2v2.Models;
using CCVProyecto2v2.ViewsGeneral;

namespace CCVProyecto2v2.ViewLogin;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();
    }
    /*private async void Ingresar_Clicked(object sender, EventArgs e)
    {
        string usuario = UsuarioEntry.Text;
        string contrasenia = ContraseniaEntry.Text;

        // Buscar usuario autenticado primero
        var usuarioAutenticado = await AutenticarUsuarioAsync(usuario, contrasenia);

        if (usuarioAutenticado != null)
        {
            switch (usuarioAutenticado.Rol)
            {
                case RolEnum.Administrador:
                    await Navigation.PushAsync(new AdministradorView());
                    break;
                case RolEnum.Profesor:
                    await Navigation.PushAsync(new ProfesorView());
                    break;
                case RolEnum.Estudiante:
                    await Navigation.PushAsync(new EstudianteView());
                    break;
            }
        }
        else if (usuario == "admin" && contrasenia == "admin")
        {
            // Esto es redundante porque ya est� en la lista
            await Navigation.PushAsync(new AdministradorView());
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contrase�a incorrectos", "OK");
        }
    }



    private async Task<Usuario> AutenticarUsuarioAsync(string nombreUsuario, string contrasenia)
    {

        List<Usuario> usuarios = new List<Usuario>
    {
        new Administrador { NombreUsuario = "admin", Contrasenia = "admin", Rol = RolEnum.Administrador },
        new Profesor { NombreUsuario = "profesor1", Contrasenia = "1234", Rol = RolEnum.Profesor },
        new Estudiante { NombreUsuario = "estudiante1", Contrasenia = "1234", Rol = RolEnum.Estudiante }
    };

        return usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.Contrasenia == contrasenia);
    }*/


    private async void Ingresar_Clicked(object sender, EventArgs e)
    {
        string usuario = UsuarioEntry.Text;
        string contrasenia = ContraseniaEntry.Text;

        // Verificar credenciales quemadas
        if (usuario == "profesor1" && contrasenia == "1234")
        {
            await Navigation.PushAsync(new ProfesorView());
        }
        else if (usuario == "estudiante1" && contrasenia == "1234")
        {
            await Navigation.PushAsync(new EstudianteView());
        }
        else if (usuario == "admin" && contrasenia == "admin")
        {
            await Navigation.PushAsync(new AdministradorView());
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contrase�a incorrectos", "OK");
        }
    }

}