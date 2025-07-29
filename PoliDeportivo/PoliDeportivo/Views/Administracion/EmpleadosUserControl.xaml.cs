using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para EmpleadosUserControl.xaml
    /// </summary>
    public partial class EmpleadosUserControl : UserControl
    {
        public class Empleado
        {
            public required string Nombre { get; set; }
            public required string Apellido { get; set; }
            public required string Direccion { get; set; }
        }

        public ObservableCollection<Empleado> ListaEmpleados = new ObservableCollection<Empleado>();
        private Empleado? empleadoSeleccionado = null;

        public EmpleadosUserControl()
        {
            InitializeComponent();
            dgEmpleados.ItemsSource = ListaEmpleados;
        }

        private void btn_guardar(object sender, RoutedEventArgs e)
        {
            var nuevo = new Empleado
            {
                Nombre = txt_Nombre.Text,
                Apellido = txt_Apellido.Text,
                Direccion = txt_Direccion.Text
            };

            ListaEmpleados.Add(nuevo);
            LimpiarCampos();
        }

        private void btn_actualizar(object sender, RoutedEventArgs e)
        {
            if (empleadoSeleccionado != null)
            {
                empleadoSeleccionado.Nombre = txt_Nombre.Text;
                empleadoSeleccionado.Apellido = txt_Apellido.Text;
                empleadoSeleccionado.Direccion = txt_Direccion.Text;

                dgEmpleados.Items.Refresh();
                LimpiarCampos();
            }
        }

        private void btn_eliminar(object sender, RoutedEventArgs e)
        {
            if (empleadoSeleccionado != null)
            {
                ListaEmpleados.Remove(empleadoSeleccionado);
                LimpiarCampos();
            }
        }

        private void btn_limpiar(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private void dgEmpleados_SelectionChanged(object sender, RoutedEventArgs e)
        {
            empleadoSeleccionado = dgEmpleados.SelectedItem as Empleado;

            if (empleadoSeleccionado != null)
            {
                txt_Nombre.Text = empleadoSeleccionado.Nombre;
                txt_Apellido.Text = empleadoSeleccionado.Apellido;
                txt_Direccion.Text = empleadoSeleccionado.Direccion;
            }
        }

        private void LimpiarCampos()
        {
            txt_Nombre.Text = "";
            txt_Apellido.Text = "";
            txt_Direccion.Text = "";
            empleadoSeleccionado = null;
            dgEmpleados.SelectedItem = null;
        }
    }
}
