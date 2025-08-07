using MySqlX.XDevAPI.Relational;
using PoliDeportivo.DataAccess;
using PoliDeportivo.Model;
using PoliDeportivo.Views.Administracion.BTN_ayuda_forms;
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
    /// Lógica de interacción para UsuariosUserControl.xaml
    /// </summary>
    public partial class UsuariosUserControl : UserControl
    {
        int estadoGuardado = 0;
        public UsuariosUserControl()
        {
            InitializeComponent();
            CargarUsuarios();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_usu_new.IsEnabled = true;
            boton_usu_guardar.IsEnabled = false;
            boton_usu_actualizar.IsEnabled = false;
            boton_usu_eliminar.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_usu_new.IsEnabled = false;
            boton_usu_guardar.IsEnabled = true;
            boton_usu_actualizar.IsEnabled = true;
            boton_usu_eliminar.IsEnabled = true;
        }

        private void LimpiarCampos()
        {
            txt_usuario_Id_pk.Clear();
            txt_usuario_nombre.Clear();
            txt_usuario_email.Clear();
            txt_usuario_contrasena.Clear();
            txt_usuario_privilegio_id_fk.Clear();
            txt_usuario_rol_id_fk.Clear();

        }

        private void CargarUsuarios()
        {
            try
            {
                D_usuarios datos = new D_usuarios();
                DataTable dt = datos.Listado_usuario();
                DGV_usuario.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
        }

        private void btn_usu_new(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();

        }

        private void btn_usu_guardar(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_usuario usuario = new Atrb_usuario()
                {
                    pk_usuario_id = int.Parse(txt_usuario_Id_pk.Text),
                    usu_nombre = txt_usuario_nombre.Text,
                    usu_email = txt_usuario_email.Text,
                    usu_contrasena = txt_usuario_contrasena.Text,
                    fk_privilegio_id = int.Parse(txt_usuario_privilegio_id_fk.Text),
                    fk_rol_id = int.Parse(txt_usuario_rol_id_fk.Text)
                };

                D_usuarios datos = new D_usuarios();
                string respuesta = datos.Guardar_Usuario(estadoGuardado, usuario);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarUsuarios();
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
        }

        private void btn_usu_actualizar(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2; // Actualizar registro
        }

        private void btn_usu_eliminar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txt_usuario_Id_pk.Text, out int id))
                {
                    D_usuarios datos = new D_usuarios();
                    string respuesta = datos.Eliminar_Usuario(id);

                    if (respuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarUsuarios();
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
        }

        private void DGV_Usuario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_usuario.SelectedItem is DataRowView row)
            {
               txt_usuario_Id_pk.Text = row["Código Usuario"].ToString();
                txt_usuario_nombre.Text = row["Nombre de Usuario"].ToString();
                txt_usuario_email.Text = row["Correo Electrónico"].ToString();
                txt_usuario_contrasena.Text = row["Contraseña"].ToString();
                txt_usuario_privilegio_id_fk.Text = row["Código Privilegio"].ToString();
                txt_usuario_rol_id_fk.Text = row["Código Rol"].ToString();
            }
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_usuario(object sender, RoutedEventArgs e)
        {
            ayuda_usuarios ventanaAyuda = new ayuda_usuarios();
            ventanaAyuda.ShowDialog();

        }
    }
}
