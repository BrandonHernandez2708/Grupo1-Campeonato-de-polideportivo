/// codigo principal logins
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PoliDeportivo.DataAccess;

namespace PoliDeportivo
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            this.KeyDown += Window_KeyDown;
        }

        private void txt_loginNombre(object sender, TextChangedEventArgs e)
        {

        }

        private void txt_loginPassword(object sender, RoutedEventArgs e)
        {

        }

        private void fIntentarIniciarSesion()
        {
            if (string.IsNullOrWhiteSpace(txt_usuario.Text))
            {
                MessageBox.Show("Ingresa tu nombre de usuario por favor.", "Campo sin llenar", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_password.Password))
            {
                MessageBox.Show("Ingresa tu contraseña", "Campo sin llenar", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txt_usuario.Text.Length > 20)
            {
                MessageBox.Show("Tu nombre de usuario no puede pasar de los 20 caracteres.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txt_password.Password.Length > 20)
            {
                MessageBox.Show("Tu contraseña no puede pasar de los 20 caracteres.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(txt_usuario.Text, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Tu nombre de usuario no puede contener caracteres especiales.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                D_usuarios usuarioDao = new D_usuarios();
                DataTable resultado = usuarioDao.Login(txt_usuario.Text.Trim(), txt_password.Password.Trim());
               
                if (resultado.Rows.Count > 0)
                {
                    string nombreUsuario = resultado.Rows[0]["usu_nombre"].ToString();
                    string rol = resultado.Rows[0]["rol_privilegio"].ToString();
                    string privilegio = resultado.Rows[0]["priv_descripcion"].ToString();

                    MessageBox.Show($"Bienvenido {nombreUsuario}\nRol: {rol}\nPrivilegio: {privilegio}", "Validación de datos correcta", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                    {
                    MenuPrincipalAdmin menu = new MenuPrincipalAdmin();
                    menu.Show();
                    this.Close();
                }
                else if (rol.Equals("Usuario", StringComparison.OrdinalIgnoreCase))
                {
                        MenuPrincipalUsuario menuUsuario = new MenuPrincipalUsuario();
                        menuUsuario.Show();
                        this.Close();
                }
                else
                {
                        MessageBox.Show("Rol no reconocido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
                else
                    {
                        MessageBox.Show("El usuario o la contraseña no son correctos.", "Error en la autenticacion", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar sesion" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         

       private void btn_IniciarSesion(object sender, RoutedEventArgs e)
        {
            fIntentarIniciarSesion();
        }

        private void btn_regresar(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                fIntentarIniciarSesion();
            }
        }
    }
}


