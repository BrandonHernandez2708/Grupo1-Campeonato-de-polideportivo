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
    /// Lógica de interacción para FaltaUserControl.xaml
    /// </summary>
    public partial class FaltaUserControl : UserControl
    {
        private int estadoGuardado = 0; // 1 = nuevo, 2 = actualizar
        public FaltaUserControl()
        {
            InitializeComponent();
            Cargarfalta();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_new_fal.IsEnabled = true;
            boton_guardar_fal.IsEnabled = false;
            boton_actualizar_fal.IsEnabled = false;
            boton_eliminar_fal.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_new_fal.IsEnabled = false;
            boton_guardar_fal.IsEnabled = true;
            boton_actualizar_fal.IsEnabled = true;
            boton_eliminar_fal.IsEnabled = true;
        }

        private void Cargarfalta()
        {
            try
            {
                D_Falta datos = new D_Falta();
                DataTable dt = datos.Listado_Falta();
                DGV_falta.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar faltas: " + ex.Message);
            }
        }
        private void btn_new_fal(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();

        }

        private void btn_guardar_fal(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_Falta falta = new Atrb_Falta()
                {
                    pk_falta_id = int.Parse(txtb_pk_falta_id.Text),
                    fal_descripcion = txtb_fal_descripcion.Text,
                    fal_minuto = int.Parse(txtb_fal_minuto.Text),
                    fk_partido_id = int.Parse(txtb_partido_id.Text),
                    fk_jugador_id = int.Parse(txtb_fk_jugador_id.Text)


                };

                D_Falta datos = new D_Falta();
                string respuesta = datos.Guardar_Falta(estadoGuardado, falta);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    Cargarfalta();
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

        private void btn_actualizar_fal(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2; // Actualizar registro
        }

        private void btn_eliminar_fal(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtb_pk_falta_id.Text, out int id))
                {
                    D_Falta datos = new D_Falta();
                    string srespuesta = datos.Eliminar_Falta(id);

                    if (srespuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        Cargarfalta();
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
            txtb_pk_falta_id.Clear();
            txtb_fal_descripcion.Clear();
            txtb_fal_minuto.Clear();
            txtb_partido_id.Clear();
            txtb_fk_jugador_id.Clear();

        }

        private void DGV_falta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_falta.SelectedItem is DataRowView row)
            {
                txtb_pk_falta_id.Text = row["ID Falta"].ToString();
                txtb_fal_descripcion.Text = row["Descripción"].ToString();
                txtb_fal_minuto.Text = row["Minuto"].ToString();
                txtb_partido_id.Text = row["ID Partido"].ToString();
                txtb_fk_jugador_id.Text = row["ID Jugador"].ToString();
            }
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_ayuda_fal(object sender, RoutedEventArgs e)
        {
            ayuda_faltas ayudaVentana = new ayuda_faltas();
            ayudaVentana.ShowDialog();

        }
    }
}
