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
    /// Lógica de interacción para SancionUserControl.xaml
    /// </summary>
    public partial class SancionUserControl : UserControl
    {
        private int estadoGuardado = 0; // 1 = nuevo, 2 = actualizar

        public SancionUserControl()
        {
            InitializeComponent();
            CargarSancion();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_newsancion.IsEnabled = true;
            boton_sancion_guardar.IsEnabled = false;
            boton_sancion_actualizar.IsEnabled = false;
            boton_sancion_eliminar.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_newsancion.IsEnabled = false;
            boton_sancion_guardar.IsEnabled = true;
            boton_sancion_actualizar.IsEnabled = true;
            boton_sancion_eliminar.IsEnabled = true;
        }

        private void CargarSancion()
        {
            try
            {
                D_sancion datos = new D_sancion();
                DataTable dt = datos.Listado_san();
                DGV_sancion.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar sanciones: " + ex.Message);
            }
        }

        private void btn_newsancion(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_sancion_guardar(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_sancion sancion = new Atrb_sancion()
                {
                    pk_sancion_id = int.Parse(txt_sancion_Id_pk.Text),
                    san_descripcion = txt_sancion_san_descripcion.Text,
                    san_minuto = int.Parse(txt_sancion_san_minuto.Text),
                    fk_partido_id = int.Parse(txt_sancion_partido_Id_fk.Text),
                    fk_jugador_id = int.Parse(txt_sancion_jugador_Id_fk.Text),
                };

                D_sancion datos = new D_sancion();
                string respuesta = datos.Guardar_san(estadoGuardado, sancion);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarSancion();
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

        private void btn_sancion_actualizar(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2; // Actualizar registro
        }

        private void btn_sancion_eliminar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txt_sancion_Id_pk.Text, out int id))
                {
                    D_sancion datos = new D_sancion();
                    string srespuesta = datos.Eliminar_san(id);

                    if (srespuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarSancion();
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
            txt_sancion_Id_pk.Clear();
            txt_sancion_san_descripcion.Clear();
            txt_sancion_san_minuto.Clear();
            txt_sancion_partido_Id_fk.Clear();
            txt_sancion_jugador_Id_fk.Clear();

        }

        private void DGV_sancion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_sancion.SelectedItem is DataRowView row)
            {

                txt_sancion_Id_pk.Text = row["Código Sanción"].ToString();
                txt_sancion_san_descripcion.Text = row["Descripción"].ToString();
                txt_sancion_san_minuto.Text = row["Minuto"].ToString();
                txt_sancion_partido_Id_fk.Text = row["Código Partido"].ToString();
                txt_sancion_jugador_Id_fk.Text = row["Código Jugador"].ToString();


            }
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_ayuda_sancion(object sender, RoutedEventArgs e)
        {
            ayuda_sanciones ventana = new ayuda_sanciones();
            ventana.ShowDialog();

        }
    }
}
