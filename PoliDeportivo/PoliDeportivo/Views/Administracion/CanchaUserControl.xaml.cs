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
    /// Lógica de interacción para CanchaUserControl.xaml
    /// </summary>
    public partial class CanchaUserControl : UserControl
    {
        private int estadoGuardado = 0; // 1 = nuevo, 2 = actualizar

        public CanchaUserControl()
        {
            InitializeComponent();
            Cargarcancha();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_new_can.IsEnabled = true;
            boton_guardar.IsEnabled = false;
            boton_actualizar.IsEnabled = false;
            boton_eliminar.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_new_can.IsEnabled = false;
            boton_guardar.IsEnabled = true;
            boton_actualizar.IsEnabled = true;
            boton_eliminar.IsEnabled = true;
        }

        private void Cargarcancha()
        {
            try
            {
                D_canchas datos = new D_canchas();
                DataTable dt = datos.Listado_Can();
                DGV_cancha.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar cancha: " + ex.Message);
            }
        }

        private void btn_new_can(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_guardar(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_cancha cancha = new Atrb_cancha()
                {
                    pk_cancha_id = int.Parse(txtb_canchaId_pk.Text),
                    can_capacidad = int.Parse(txtb_can_capacidad.Text),
                    can_direccion = txtb_Can_direccion.Text,
                    
                };

                D_canchas datos = new D_canchas();
                string respuesta = datos.Guardar_cancha(estadoGuardado, cancha);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    Cargarcancha();
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
                if (int.TryParse(txtb_canchaId_pk.Text, out int id))
                {
                    D_canchas datos = new D_canchas();
                    string srespuesta = datos.Eliminar_Can(id);

                    if (srespuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        Cargarcancha();
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

        private void LimpiarCampos()
        {
            txtb_canchaId_pk.Clear();
            txtb_can_capacidad.Clear();
            txtb_Can_direccion.Clear();
           
        }

        private void DGV_cancha_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_cancha.SelectedItem is DataRowView row)
            {

                txtb_canchaId_pk.Text = row["Código Cancha"].ToString();
                txtb_can_capacidad.Text = row["Capacidad"].ToString();
                txtb_Can_direccion.Text = row["Dirección"].ToString();
            

            }
            ConfigurarBotonesDespuesDeSeleccion();
        }
    }
}
