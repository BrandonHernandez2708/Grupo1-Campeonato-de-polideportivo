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

    public partial class EmpleadosUserControl : UserControl
    {
        private int estadoGuardado = 0; // 1 = nuevo, 2 = actualizar
        public EmpleadosUserControl()
        {
            InitializeComponent();
            CargarEmpleados();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_new_emp.IsEnabled = true;
            boton_guardar_emp.IsEnabled = false;
            boton_actualizar_emp.IsEnabled = false;
            boton_eliminar_emp.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_new_emp.IsEnabled = false;
            boton_guardar_emp.IsEnabled = true;
            boton_actualizar_emp.IsEnabled = true;
            boton_eliminar_emp.IsEnabled = true;
        }

        private void LimpiarCampos()
        {
            txtb_emp_Id_pk.Clear();
            txtb_emp_nombre.Clear();
            txtb_emp_apellido.Clear();

        }

        private void CargarEmpleados()
        {
            try
            {
                D_Empleados datos = new D_Empleados();
                DataTable dt = datos.Listado_Emp();
                DGV_Empleado.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar empleados: " + ex.Message);
            }
        }

        private void btn_new_emp(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();

        }

        private void btn_guardar_emp(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_Empleados Empleado = new Atrb_Empleados()
                {
                    pk_empleado_id = int.Parse(txtb_emp_Id_pk.Text),
                    emp_nombre = txtb_emp_nombre.Text,
                    emp_apellido = txtb_emp_apellido.Text
                };

                D_Empleados datos = new D_Empleados();
                string respuesta = datos.Guardar_Emp(estadoGuardado, Empleado);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarEmpleados();
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

        private void btn_actualizar_emp(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2; // Actualizar registro
        }

        private void btn_eliminar_emp(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtb_emp_Id_pk.Text, out int id))
                {
                    D_Empleados datos = new D_Empleados();
                    string respuesta = datos.Eliminar_Emp(id);

                    if (respuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarEmpleados();
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

        private void DGV_Empleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_Empleado.SelectedItem is DataRowView row)
            {
                txtb_emp_Id_pk.Text = row["Código Empleado"].ToString();
                txtb_emp_nombre.Text = row["Nombre"].ToString();
                txtb_emp_apellido.Text = row["Apellido"].ToString();
            }
            ConfigurarBotonesDespuesDeSeleccion();
        }
    }
}
