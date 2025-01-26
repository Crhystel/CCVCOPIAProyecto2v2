using CCVProyecto2v2.Models;
using CCVProyecto2v2.ViewsGeneral;
using CCVProyecto2v2.Services;

namespace CCVProyecto2v2.ViewLogin;

public partial class LoginView : ContentPage
{
    private readonly AuthService _authService;
    public LoginView()
    {
        InitializeComponent();
        _authService = new AuthService(); 
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
            // Esto es redundante porque ya está en la lista
            await Navigation.PushAsync(new AdministradorView());
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
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
        /*string usuario = UsuarioEntry.Text;
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
            await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
        }*/
        string username = UsuarioEntry.Text;
        string password = ContraseniaEntry.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
            return;
        }

        bool isAuthenticated = await _authService.LoginAsync(username, password);
        if (isAuthenticated)
        {
           
            /*string userRole = await SecureStorage.GetAsync("userRole");
            switch (userRole)
            {
                case "0":*/
                    await Navigation.PushAsync(new AdministradorView());
                   /* break;
                case "1":
                    await Navigation.PushAsync(new ProfesorView());
                    break;
                case "2":
                    await Navigation.PushAsync(new EstudianteView());
                    break;
                default:
                    await DisplayAlert("Error", "Rol de usuario no válido.", "OK");
                    break;
            }*/
        }
        else
        {
            await DisplayAlert("Error", "Nombre de usuario o contraseña incorrectos.", "OK");
        }
    }
}