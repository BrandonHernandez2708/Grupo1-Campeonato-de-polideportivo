

using PoliDeportivo.DataAccess;
using PoliDeportivo.Model;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PoliDeportivo.Views.Administracion
{
    /// <summary>
    /// Lógica de interacción para LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        private int estadoGuardado = 0; // 1 = nuevo, 2 = actualizar
        public LoginUserControl()
        {
            InitializeComponent();
            CargarLogin();
            ConfigurarBotonesEstadoInicial();
        }
        private void ConfigurarBotonesEstadoInicial()
        {
            boton_nuevo_login.IsEnabled = true;
            boton_guardar_login.IsEnabled = false;
            boton_actualizar_login.IsEnabled = false;
            boton_eliminar_login.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_nuevo_login.IsEnabled = false;
            boton_guardar_login.IsEnabled = true;
            boton_actualizar_login.IsEnabled = true;
            boton_eliminar_login.IsEnabled = true;
        }
        private void LimpiarCampos()
        {
            txtb_usuario_id_pk.Clear();
            txtb_usuario.Clear();
            txtb_user_email.Clear();
            txtb_user_contraseña.Clear();
            txtb_id_privilegios.Clear();
            txtb_id_roles.Clear();
        }
        private void CargarLogin()
        {
            try
            {
                D_Login datos = new D_Login();
                DataTable dt = datos.Listado_Login();
                DGV_login.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar Login: " + ex.Message);
            }
        }
        private void btn_nuevo_login(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_guardar_login(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_Login Login = new Atrb_Login()
                {
                    pk_usuario_id = int.Parse(txtb_usuario_id_pk.Text),
                    usuario = txtb_usuario.Text,
                    user_email = txtb_user_email.Text,
                    user_contraseña = txtb_user_contraseña.Text,
                    fk_id_privilegios = int.Parse(txtb_id_privilegios.Text),
                    fk_id_roles = int.Parse(txtb_id_roles.Text)




                };

                D_Login datos = new D_Login();
                string respuesta = datos.Guardar_Login(estadoGuardado, Login);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarLogin();
                    LimpiarCampos();
                    estadoGuardado = 0;
                }
                else
                {
                    MessageBox.Show("Error: " + respuesta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
            ConfigurarBotonesEstadoInicial();
            CargarLogin();
        }
        

        private void btn_actualizar_login(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2; // Actualizar registro
            CargarLogin();
        }

        private void btn_eliminar_login(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtb_usuario.Text, out int id))
                {
                    D_Login datos = new D_Login();
                    string respuesta = datos.Eliminar_Login(id);

                    if (respuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarLogin();
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("Error: " + respuesta);
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un registro válido para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
            ConfigurarBotonesEstadoInicial();
            CargarLogin();
        }

        private void DGV_login_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if (DGV_login.SelectedItem is DataRowView row)
            {
                txtb_usuario.Text = row["Código Login"].ToString();
                txtb_usuario.Text = row["Nombre del Usuario"].ToString();
                txtb_user_email.Text = row["Email"].ToString();
                txtb_user_contraseña.Text = row["Contraseña"].ToString();
                txtb_id_privilegios.Text = row["Privilegios"].ToString();
                txtb_id_roles.Text = row["Roles"].ToString();

            }
            ConfigurarBotonesDespuesDeSeleccion();

        }
    }
}