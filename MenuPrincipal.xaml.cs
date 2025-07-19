using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Prototipo
{
    public partial class MenuPrincipal : Window
    {
        private bool adminMenuAbierto = false;
        private bool usuariosMenuAbierto = false;

        public MenuPrincipal()
        {

            InitializeComponent();
            
        }

        private void btn_despliegueUsr(object sender, RoutedEventArgs e)
        {
            int to = usuariosMenuAbierto ? 0 : 1;

            var animacion = new DoubleAnimation
            {
                To = to,
                Duration = TimeSpan.FromMilliseconds(300),
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8
            };

            MenuUsuariosScale.BeginAnimation(ScaleTransform.ScaleYProperty, animacion);
            usuariosMenuAbierto = !usuariosMenuAbierto;

            AjustarPosicionAdmin();
            AjustarPosicionCerrarSesion();
        }

        private void btn_despliegueAdmin(object sender, RoutedEventArgs e)
        {
            int to = adminMenuAbierto ? 0 : 1;

            var animacion = new DoubleAnimation
            {
                To = to,
                Duration = TimeSpan.FromMilliseconds(300),
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8
            };

            MenuAdminScale.BeginAnimation(ScaleTransform.ScaleYProperty, animacion);
            adminMenuAbierto = !adminMenuAbierto;

            AjustarPosicionCerrarSesion();
        }

        private void AjustarPosicionAdmin()
        {
            // Altura visible del menú Usuario (botón + menú desplegado)
            double alturaMenuUsuario = boton_despliegueUsuario.Height;

            if (usuariosMenuAbierto)
            {
                // Sumar altura real del menú Usuario desplegado
                alturaMenuUsuario += MenuUsuarioPanel.ActualHeight;
            }

            // Ajustar margen superior del botón Administración
            // Aquí ponemos 66 + alturaMenuUsuario para que quede justo debajo del menú Usuario
            boton_despliegueAdmin.Margin = new Thickness(0, 66 + alturaMenuUsuario, 0, 0);

            // Ajustar margen superior del menú Administración (Border)
            double margenSuperiorMenuAdmin = boton_despliegueAdmin.Margin.Top + boton_despliegueAdmin.Height;
            borderAdminMenu.Margin = new Thickness(10, margenSuperiorMenuAdmin, 10, 10);

        }

        private void AjustarPosicionCerrarSesion()
        {
            if (adminMenuAbierto && usuariosMenuAbierto)
            {
                // Base desde donde se suman alturas, por ejemplo el Label superior (66 px)
                double posicionBase = 66;

                // Altura botón usuario + menú usuario desplegado
                double alturaUsuario = boton_despliegueUsuario.Height + MenuUsuarioPanel.ActualHeight;

                // Altura botón admin + menú admin desplegado
                double alturaAdmin = boton_despliegueAdmin.Height + MenuAdminPanel.ActualHeight;

                // Posición Y nueva, justo debajo de ambos menús desplegados
                double nuevaPosY = posicionBase + alturaUsuario + alturaAdmin;

                btn_cerrarSesion.Margin = new Thickness(10, nuevaPosY, 0, 0);
            }
            else
            {
                // Volver a posición original si no están ambos desplegados
                btn_cerrarSesion.Margin = new Thickness(10, 556, 0, 0);
            }
        }








    }
}
