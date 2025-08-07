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

    public partial class PuestosUserControl : UserControl
    {
        private int estadoGuardado = 0; // 1 = nuevo, 2 = actualizar
        public PuestosUserControl()
        {
            InitializeComponent();
            CargarPuestos();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_new_puesto.IsEnabled = true;
            boton_guardar_puesto.IsEnabled = false;
            boton_actualizar_puesto.IsEnabled = false;
            boton_eliminar_puesto.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_new_puesto.IsEnabled = false;
            boton_guardar_puesto.IsEnabled = true;
            boton_actualizar_puesto.IsEnabled = true;
           boton_eliminar_puesto.IsEnabled = true;
        }

        private void LimpiarCampos()
        {
            txtb_puesto_Id_pk.Clear();
            txtb_puesto_nombre.Clear();
            txtb_puesto_descripcion.Clear();

        }

        private void CargarPuestos()
        {
            try
            {
                D_Puesto datos = new D_Puesto();
                DataTable dt = datos.Listado_Puesto();
                DGV_Puesto.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar Puestos: " + ex.Message);
            }
        }

        private void btn_new_puesto(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_guardar_puesto(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_Puesto Puesto = new Atrb_Puesto()
                {
                    pk_puesto_id = int.Parse(txtb_puesto_Id_pk.Text),
                    pue_nombre = txtb_puesto_nombre.Text,
                    pue_descripcion = txtb_puesto_descripcion.Text
                };

                D_Puesto datos = new D_Puesto();
                string respuesta = datos.Guardar_Puesto(estadoGuardado, Puesto);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarPuestos();
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
            CargarPuestos();
        }

        private void btn_actualizar_puesto(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2; // Actualizar registro
            CargarPuestos();
        }

        private void btn_eliminar_puesto(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtb_puesto_Id_pk.Text, out int id))
                {
                    D_Puesto datos = new D_Puesto();
                    string respuesta = datos.Eliminar_puesto(id);

                    if (respuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarPuestos();
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
            CargarPuestos();
        }


        private void DGV_Puesto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_Puesto.SelectedItem is DataRowView row)
            {
               txtb_puesto_Id_pk.Text = row["Código Puesto"].ToString();
                txtb_puesto_nombre.Text = row["Nombre del Puesto"].ToString();
                txtb_puesto_descripcion.Text = row["Descripción"].ToString();
            }
            ConfigurarBotonesDespuesDeSeleccion();

        }

        private void btn_ayuda_puesto(object sender, RoutedEventArgs e)
        {
            ayuda_puesto ayuda = new ayuda_puesto();
            ayuda.ShowDialog();

        }
    }
}
