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

namespace PoliDeportivo.Views
{
    public partial class PartidoUserControl : UserControl
    {
        private int iestadoGuardado = 0; // 1 = nuevo, 2 = actualizar

        public PartidoUserControl()
        {
            InitializeComponent();
            CargarPartidos();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_newpar.IsEnabled = true;
            boton_guardar_partido.IsEnabled = false;
            boton_actualizar_partido.IsEnabled = false;
            boton_eliminar_partido.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_newpar.IsEnabled = false;
            boton_guardar_partido.IsEnabled = true;
            boton_actualizar_partido.IsEnabled = true;
            boton_eliminar_partido.IsEnabled = true;
        }

        private void LimpiarCampos()
        {
            txt_partidoid.Clear();
            txt_jornada.Clear();
            txt_idequipo1.Clear();
            txt_idequipo2.Clear();
            txt_idcancha.Clear();
            txt_fechahora.Clear();
            txtb_idestado.Clear();
            txt_arbritoid.Clear();
            txt_puntaje1.Clear();
            txt_puntaje2.Clear();
            txt_tiempoextra.Clear();
            txt_equipoganadorid.Clear();
        }

        private void CargarPartidos()
        {
            try
            {
                D_Partidos datos = new D_Partidos();
                DataTable dt = datos.Listado_Partidos();
                DGV_partido.ItemsSource = dt.DefaultView;

                // Ocultar columnas con IDs
                if (DGV_partido.Columns.Count > 0)
                {
                    foreach (var col in DGV_partido.Columns)
                    {
                        DataGridColumn column = (DataGridColumn)col;
                        string header = column.Header.ToString();

                        if (header == "ID Equipo 1" ||
                            header == "ID Equipo 2" ||
                            header == "ID Estado" ||
                            header == "ID Equipo Ganador" ||
                            header == "Árbitro ID" ||
                            header == "ID Jornada")
                        {
                            column.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar Partidos: " + ex.Message);
            }
        }

        

        private void btn_newpar(object sender, RoutedEventArgs e)
        {
            iestadoGuardado = 1;
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_guardar_partido(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_Partidos equipo = new Atrb_Partidos()
                {
                    pk_partido_id = int.Parse(txt_partidoid.Text),
                    fk_jornada_id = int.Parse(txt_jornada.Text),
                    fk_equipo1_id = int.Parse(txt_idequipo1.Text),
                    fk_equipo2_id = int.Parse(txt_idequipo2.Text),
                    fk_cancha_id = int.Parse(txt_idcancha.Text),
                    par_fecha_hora = DateTime.Parse(txt_fechahora.Text),
                    fk_estado_id = int.Parse(txtb_idestado.Text),
                    fk_empleado_arbitro_id = int.Parse(txt_arbritoid.Text),
                    par_puntaje_equipo1 = int.Parse(txt_puntaje1.Text),
                    par_puntaje_equipo2 = int.Parse(txt_puntaje2.Text),
                    fk_equipo_ganador_id = int.Parse(txt_equipoganadorid.Text),
                };

                D_Partidos datos = new D_Partidos();
                string respuesta = datos.Guardar_Partido(iestadoGuardado, equipo);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarPartidos();
                    LimpiarCampos();
                    iestadoGuardado = 0;
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
            CargarPartidos();
        }

        private void btn_actualizar_partido(object sender, RoutedEventArgs e)
        {
            iestadoGuardado = 2;
        }

        private void btn_eliminar_partido(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txt_partidoid.Text, out int id))
                {
                    D_Partidos datos = new D_Partidos();
                    string respuesta = datos.Eliminar_Partido(id);

                    if (respuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarPartidos();
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
            CargarPartidos();
        }

        private void DGV_partido_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_partido.SelectedItem is DataRowView row)
            {
                txt_partidoid.Text = row["Código Partido"]?.ToString();
                txt_jornada.Text = row["ID Jornada"]?.ToString();
                txt_idequipo1.Text = row["ID Equipo 1"]?.ToString();
                txt_idequipo2.Text = row["ID Equipo 2"]?.ToString();
                txt_idcancha.Text = row["Código Cancha"]?.ToString();
                txt_arbritoid.Text = row["Árbitro ID"]?.ToString();
                txt_fechahora.Text = row["Fecha y Hora"]?.ToString();
                txt_puntaje1.Text = row["Puntaje Equipo 1"]?.ToString();
                txt_puntaje2.Text = row["Puntaje Equipo 2"]?.ToString();
                txt_equipoganadorid.Text = row["ID Equipo Ganador"]?.ToString();
                txtb_idestado.Text = row["ID Estado"]?.ToString();
                txt_tiempoextra.Text = row["Tiempo Extra"]?.ToString();

                ConfigurarBotonesDespuesDeSeleccion();
            }


        }

        private void btn_ayuda_partido(object sender, RoutedEventArgs e)
        {
            ayuda_partido ventana = new ayuda_partido();
            ventana.ShowDialog();
        }
    }
}