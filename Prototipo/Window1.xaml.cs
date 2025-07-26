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

namespace Prototipo
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void txt_nombreUsuario(object sender, TextChangedEventArgs e)
        {

        }

        private void txt_password(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_IniciarSesion(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuP = new MenuPrincipal();
            menuP.Show();
            this.Close();
        }

        private void btn_regresar(object sender, RoutedEventArgs e)
        {
            MainWindow ventanaPrincipal = new MainWindow();
            ventanaPrincipal.Show();
            this.Close();
        }
    }
}
