using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PoliDeportivo.Views.Administracion
{
    /// <summary>
    /// Lógica de interacción para GestionUsrUserControl.xaml
    /// </summary>
    public partial class GestionUsrUserControl : UserControl
    {
        public class Usuario
        {
            public int IdUsuario { get; set; }
            public string Empleado { get; set; }
            public string Correo { get; set; }
            public string Contrasena { get; set; }
            public DateTime FechaCreacion { get; set; }
        }

        private ObservableCollection<Usuario> listaUsuarios = new ObservableCollection<Usuario>();
        private Usuario usuarioSeleccionado = null;

        public GestionUsrUserControl()
        {
            InitializeComponent();
            dgUsuarios.ItemsSource = listaUsuarios;
        }

        private void btn_guardar(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtEmpleado.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            Usuario nuevo = new Usuario
            {
                IdUsuario = int.Parse(txtIdUsuario.Text),
                Empleado = txtEmpleado.Text,
                Correo = txtCorreo.Text,
                Contrasena = txtPassword.Password,
                FechaCreacion = dpFecha.SelectedDate ?? DateTime.Now
            };

            listaUsuarios.Add(nuevo);
            LimpiarCampos();
        }

        private void btn_actualizar(object sender, RoutedEventArgs e)
        {
            if (usuarioSeleccionado != null)
            {
                usuarioSeleccionado.IdUsuario = int.Parse(txtIdUsuario.Text);
                usuarioSeleccionado.Empleado = txtEmpleado.Text;
                usuarioSeleccionado.Correo = txtCorreo.Text;
                usuarioSeleccionado.Contrasena = txtPassword.Password;
                usuarioSeleccionado.FechaCreacion = dpFecha.SelectedDate ?? DateTime.Now;

                dgUsuarios.Items.Refresh();
                LimpiarCampos();
            }
        }

        private void btn_eliminar(object sender, RoutedEventArgs e)
        {
            if (usuarioSeleccionado != null)
            {
                listaUsuarios.Remove(usuarioSeleccionado);
                LimpiarCampos();
            }
        }

        private void btn_limpiar(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            usuarioSeleccionado = dgUsuarios.SelectedItem as Usuario;
            if (usuarioSeleccionado != null)
            {
                txtIdUsuario.Text = usuarioSeleccionado.IdUsuario.ToString();
                txtEmpleado.Text = usuarioSeleccionado.Empleado;
                txtCorreo.Text = usuarioSeleccionado.Correo;
                txtPassword.Password = usuarioSeleccionado.Contrasena;
                dpFecha.SelectedDate = usuarioSeleccionado.FechaCreacion;
            }
        }

        private void LimpiarCampos()
        {
            txtIdUsuario.Text = "";
            txtEmpleado.Text = "";
            txtCorreo.Text = "";
            txtPassword.Password = "";
            dpFecha.SelectedDate = null;
            usuarioSeleccionado = null;
            dgUsuarios.UnselectAll();
        }
    }
}
