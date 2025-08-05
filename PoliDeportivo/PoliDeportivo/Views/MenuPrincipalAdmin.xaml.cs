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
using PoliDeportivo.Views;
using PoliDeportivo.Views.Administracion;
using PoliDeportivo.Views.Usuario;

namespace PoliDeportivo
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipalAdmin.xaml
    /// </summary>
    public partial class MenuPrincipalAdmin : Window
    {
        public MenuPrincipalAdmin()
        {
            InitializeComponent();
        }


        private void btn_despliegueUsuario(object sender, RoutedEventArgs e)
        {
            MenuUsuario.Visibility = MenuUsuario.Visibility == Visibility.Visible
            ? Visibility.Collapsed
            : Visibility.Visible;

        }

        private void btn_despliegueAdmin(object sender, RoutedEventArgs e)
        {
            MenuAdmin.Visibility = MenuAdmin.Visibility == Visibility.Visible
            ? Visibility.Collapsed
            : Visibility.Visible;
        }

        private void btn_cerrarSesion(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();

        }

        // Botones de Usuario
        private void btn_usr_campeonatos(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new UsrCampeonatosUserControl();
        }


        private void btn_campeonatos(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new CampeonatosUserControl();
        }

        private void btn_equipos(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new EquiposUserControl();
        }
        private void btn_jugadores(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new JugadorUserControl();
        }
        private void btn_partidos(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new PartidoUserControl();
        }
        private void btn_empleados(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new EmpleadosUserControl();
        }

        private void btn_gestionUsr(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new GestionUsrUserControl();
        }

        private void btn_puesto(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new PuestosUserControl();
        }

        private void btn_entrenador(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new EntrenadoresUserControl();
        }

        //Agregar nuevo boton de jornadas/partidos
        private void btn_jornadas(object sender, RoutedEventArgs e)
        {
            JornadasUserControl vistaJornadas = new JornadasUserControl();
            contenedor.Content = vistaJornadas;
        }
        private void btn_cancha(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new CanchaUserControl();
        }
       private void btn_estado_partido(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new EstadoPartidoUserControl();
        }

        private void btn_contratacion(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new ContratacionUserControl();
        }

        private void btn_tel_empleados(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new TelefonoEmpUserControl();
        }

        private void btn_usuario(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new UsuariosUserControl();
        }

        private void btn_bitacora(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new BitacoraUserControl();
        }
        private void btn_asistencia(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new AsistenciaUserControl();
        }
        private void btn_sancion(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new SancionUserControl();
        }

        private void btn_falta(object sender, RoutedEventArgs e)
        {
            contenedor.Content = new FaltaUserControl();
        }
    }
}
