using PoliDeportivo.Views.Administracion;
using PoliDeportivo.Views.Usuario;
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
    /// Lógica de interacción para MenuPrincipalUsuario.xaml
    /// </summary>
    public partial class MenuPrincipalUsuario : Window
    {
        public MenuPrincipalUsuario()
        {
            InitializeComponent();
        }

        private void btn_campeonatos(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new CampeonatosUserControl();
        }

        private void btn_cerrarSesion(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();


        }

        private void Btn_Reglasjuego_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_usrCampeonatos(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new UsrCampeonatosUserControl();

        }

        private void Btn_usrEquipos(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new UserEquiposUserControl();
        }
    }
}
