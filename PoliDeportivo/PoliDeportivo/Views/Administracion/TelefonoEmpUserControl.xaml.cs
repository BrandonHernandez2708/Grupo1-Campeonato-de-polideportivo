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
    /// Lógica de interacción para TelefonoEmpUserControl.xaml
    /// </summary>
    public partial class TelefonoEmpUserControl : UserControl
    {
        private int estadoGuardado = 0; // 1 = nuevo, 2 = actualizar

        public TelefonoEmpUserControl()
        {
            InitializeComponent();
            CargarTelEmp();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_new.IsEnabled = true;
            boton_guardar.IsEnabled = false;
            boton_actualizar.IsEnabled = false;
            boton_eliminar.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_new.IsEnabled = false;
            boton_guardar.IsEnabled = true;
            boton_actualizar.IsEnabled = true;
            boton_eliminar.IsEnabled = true;
        }

        private void LimpiarCampos()
        {
            txtb_Tel_Id_pk.Clear();
            txtb_tel_numero.Clear();
            txtb_fk_empleado_id.Clear();

        }

        private void CargarTelEmp()
        {
            try
            {
                D_Tel_Empleado datos = new D_Tel_Empleado();
                DataTable dt = datos.Listado_Tel();
                DGV_Tel_Empleado.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar Telefonos: " + ex.Message);
            }
        }

        private void btn_new(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_guardar(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_Tel_Emp Telefono = new Atrb_Tel_Emp()
                {
                    pk_tel_empleado_id= int.Parse(txtb_Tel_Id_pk.Text),
                    tel_empleado = int.Parse(txtb_tel_numero.Text),
                    fk_empleado_id = txtb_fk_empleado_id.Text

                };

                D_Tel_Empleado datos = new D_Tel_Empleado();
                string respuesta = datos.Guardar_Tel(estadoGuardado, Telefono);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarTelEmp();
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

        private void btn_actualizar(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2; // Actualizar registro
        }

        private void btn_eliminar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtb_Tel_Id_pk.Text, out int id))
                {
                    D_Tel_Empleado datos = new D_Tel_Empleado();
                    string srespuesta = datos.Eliminar_Tel_Emp(id);

                    if (srespuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarTelEmp();
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("Error: " + srespuesta);
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

        

        private void DGV_Tel_Empleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_Tel_Empleado.SelectedItem is DataRowView row)
            {

                txtb_Tel_Id_pk.Text = row["ID Teléfono"].ToString();
                txtb_tel_numero.Text = row["Número"].ToString();
                txtb_fk_empleado_id.Text = row["ID Empleado"].ToString();


            }
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_ayuda_tel_emp(object sender, RoutedEventArgs e)
        {
            ayuda_telefono_empleado ayudaWindow = new ayuda_telefono_empleado();
            ayudaWindow.ShowDialog();

        }
    }
}
