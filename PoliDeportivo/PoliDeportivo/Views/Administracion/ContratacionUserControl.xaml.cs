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
    /// Lógica de interacción para ContratacionUserControl.xaml
    /// </summary>
    public partial class ContratacionUserControl : UserControl
    {
        int estadoGuardado = 0;
        public ContratacionUserControl()
        {
            InitializeComponent();
            CargarCon();
            ConfigurarBotonesEstadoInicial();
        }
        private void ConfigurarBotonesEstadoInicial()
        {
            boton_new_Con.IsEnabled = true;
            boton_guardar_Con.IsEnabled = false;
            boton_actualizar_Con.IsEnabled = false;
            boton_eliminar_Con.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_new_Con.IsEnabled = false;
            boton_guardar_Con.IsEnabled = true;
            boton_actualizar_Con.IsEnabled = true;
            boton_eliminar_Con.IsEnabled = true;
        }

        private void LimpiarCampos()
        {
            txtb_con_empleado_Id_pk_fk.Clear();
            txtb_con_Puesto_Id_pk_fk.Clear();
            txtb_con_fecha.Clear();
            txtb_con_operacion.Clear();
        }

        private void CargarCon()
        {
            try
            {
                D_Contratacion datos = new D_Contratacion();
                DataTable dt = datos.Listado_Contratacion();
                DGV_Contratacion.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar Contratacion: " + ex.Message);
            }
        }
        private void btn_new_con(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_guardar_con(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.Parse(txtb_con_operacion.Text) > 2 )
                {
                    MessageBox.Show("Valor invalido de Operacion ");
                    return;
                }


                if (!DateTime.TryParse(txtb_con_fecha.Text, out DateTime fecha))
                {
                    MessageBox.Show("El formato de la fecha es inválido. Por favor, ingrese una fecha válida.");
                    return; 
                }

               
                Atrb_Contratacion Contratacion = new Atrb_Contratacion()
                {
                    fk_empleado_id = int.Parse(txtb_con_empleado_Id_pk_fk.Text),
                    fk_puesto_id = int.Parse(txtb_con_Puesto_Id_pk_fk.Text),
                    con_fecha = fecha, 
                    con_tipo_operacion = int.Parse(txtb_con_operacion.Text)
                };

               
                D_Contratacion datos = new D_Contratacion();
                string respuesta = datos.Guardar_Con(estadoGuardado, Contratacion);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarCon();
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

        private void btn_actualizar_con(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2; // Actualizar registro
        }

        private void btn_eliminar_Con(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar ambos IDs
                bool empleadoValido = int.TryParse(txtb_con_empleado_Id_pk_fk.Text, out int idEmpleado);
                bool puestoValido = int.TryParse(txtb_con_Puesto_Id_pk_fk.Text, out int idPuesto);

                if (empleadoValido && puestoValido)
                {
                    D_Contratacion datos = new D_Contratacion();
                    string srespuesta = datos.Eliminar_Con(idPuesto, idEmpleado);

                    if (srespuesta == "OK")
                    {
                        MessageBox.Show("Registro de contratación eliminado correctamente");
                        CargarCon();
                        LimpiarCampos(); 
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar: " + srespuesta);
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un puesto y empleado válidos para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado al eliminar: " + ex.Message);
            }

            ConfigurarBotonesEstadoInicial(); 
        }

        private void DGV_Contratacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_Contratacion.SelectedItem is DataRowView row)
            {
                txtb_con_Puesto_Id_pk_fk.Text = row["Código Puesto"].ToString();
                txtb_con_empleado_Id_pk_fk.Text = row["Código Empleado"].ToString();
                txtb_con_fecha.Text = row["Fecha de la operación"].ToString();
                txtb_con_operacion.Text = row["Tipo de operación"].ToString();

            }
            ConfigurarBotonesDespuesDeSeleccion();
        }
    }
}
