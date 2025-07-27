/// codigo principal
using System;
using System.Collections.Generic;
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

        private void IntentarIniciarSesion()
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Ingresa tu nombre de usuario por favor.", "Campo sin llenar", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Ingresa tu contraseña", "Campo sin llenar", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtUsuario.Text.Length > 20)
            {
                MessageBox.Show("Tu nombre de usuario no puede pasar de los 20 caracteres.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtPassword.Password.Length > 20)
            {
                MessageBox.Show("Tu contraseña no puede pasar de los 20 caracteres.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(txtUsuario.Text, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Tu nombre de usuario no puede contener caracteres especiales.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (txtUsuario.Text == "grupo1" && txtPassword.Password == "analisis")
                {
                    MenuPrincipalAdmin menuPrincipal = new MenuPrincipalAdmin();
                    menuPrincipal.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("El usuario o contraseña no son correctos.", "Error de autenticación", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar sesión: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_IniciarSesion(object sender, RoutedEventArgs e)
        {
            IntentarIniciarSesion();
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
                IntentarIniciarSesion();
        }
    }
}
}

