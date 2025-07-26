using System;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Prototipo
{
    public partial class MenuPrincipal : Window
    {
        private bool badminMenuAbierto = false;
        private bool busuariosMenuAbierto = false;

        public MenuPrincipal()
        {

            InitializeComponent();

        }

        private void btn_despliegueUsr(object sender, RoutedEventArgs e)
        {
            int to = busuariosMenuAbierto ? 0 : 1;

            var animacion = new DoubleAnimation
            {
                To = to,
                Duration = TimeSpan.FromMilliseconds(300),
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8
            };

            MenuUsuariosScale.BeginAnimation(ScaleTransform.ScaleYProperty, animacion);
            busuariosMenuAbierto = !busuariosMenuAbierto;

            AjustarPosicionAdmin();
            AjustarPosicionCerrarSesion();
        }

        private void btn_despliegueAdmin(object sender, RoutedEventArgs e)
        {
            int to = badminMenuAbierto ? 0 : 1;

            var animacion = new DoubleAnimation
            {
                To = to,
                Duration = TimeSpan.FromMilliseconds(300),
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8
            };

            MenuAdminScale.BeginAnimation(ScaleTransform.ScaleYProperty, animacion);
            badminMenuAbierto = !badminMenuAbierto;

            AjustarPosicionCerrarSesion();
        }

        private void AjustarPosicionAdmin()
        {
           
            double dbalturaMenuUsuario = btndespliegueUsuario.Height;

            if (busuariosMenuAbierto)
            {
              
                dbalturaMenuUsuario += staMenuUsuarioPanel.ActualHeight;
            }

            
            btndespliegueAdmin.Margin = new Thickness(0, 66 + dbalturaMenuUsuario, 0, 0);

            
            double dbmargenSuperiorMenuAdmin = btndespliegueAdmin.Margin.Top + btndespliegueAdmin.Height;
            borderAdminMenu.Margin = new Thickness(10, dbmargenSuperiorMenuAdmin, 10, 10);

        }

        private void AjustarPosicionCerrarSesion()
        {
            if (badminMenuAbierto && busuariosMenuAbierto)
            {
                
                double dbposicionBase = 66;

                
                double dbalturaUsuario = btndespliegueUsuario.Height + staMenuUsuarioPanel.ActualHeight;

                
                double dbalturaAdmin = btndespliegueAdmin.Height + staMenuAdminPanel.ActualHeight;

                
                double dbnuevaPosY = dbposicionBase + dbalturaUsuario + dbalturaAdmin;

                btn_cerrarSesion.Margin = new Thickness(10, dbnuevaPosY, 0, 0);
            }
            else
            {
                
                btn_cerrarSesion.Margin = new Thickness(10, 556, 0, 0);
            }
        }



    }
}