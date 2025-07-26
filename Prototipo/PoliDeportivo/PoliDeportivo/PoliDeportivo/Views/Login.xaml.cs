using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }

        private void txt_loginNombre(object sender, TextChangedEventArgs e)
        {

        }

        private void txt_loginPassword(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_IniciarSesion(object sender, RoutedEventArgs e)
        { 
            MenuPrincipalAdmin menuPrincipal = new MenuPrincipalAdmin();
            menuPrincipal.Show();
            this.Close();
        }

        private void btn_regresar(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

        }
    }
}
