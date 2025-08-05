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
    /// Lógica de interacción para AsistenciaUserControl.xaml
    /// </summary>
    public partial class AsistenciaUserControl : UserControl
    {
        private int estadoGuardado = 0; // 1 = nuevo, 2 = actualizar

        public AsistenciaUserControl()
        {
            InitializeComponent();
            CargarAsistencia();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_newasistencia.IsEnabled = true;
            boton_asistencia_guardar.IsEnabled = false;
            boton_asistencia_actualizar.IsEnabled = false;
            boton_asistencia_eliminar.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_newasistencia.IsEnabled = false;
            boton_asistencia_guardar.IsEnabled = true;
            boton_asistencia_actualizar.IsEnabled = true;
            boton_asistencia_eliminar.IsEnabled = true;
        }

        private void CargarAsistencia()
        {
            try
            {
                D_asistencia datos = new D_asistencia();
                DataTable dt = datos.Listado_Asist();
                DGV_asistencia.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar asistencias: " + ex.Message);
            }
        }

        private void btn_newasistencia(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_asistencia_guardar(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_asistencia asistencia = new Atrb_asistencia()
                {
                    pk_asistencia_id = int.Parse(txt_asistencia_Id_pk.Text),
                    fk_anotacion_id = int.Parse(txt_asistencia_anotacion_Id_fk.Text),
                    fk_jugador_id = int.Parse(txt_asistencia_jugador_Id_fk.Text)
                };

                D_asistencia datos = new D_asistencia();
                string respuesta = datos.Guardar_Asist(estadoGuardado, asistencia);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarAsistencia();
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

        private void btn_asistencia_actualizar(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2; // Actualizar registro
        }

        private void btn_asistencia_eliminar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txt_asistencia_Id_pk.Text, out int id))
                {
                    D_asistencia datos = new D_asistencia();
                    string srespuesta = datos.Eliminar_Asist(id);

                    if (srespuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarAsistencia();
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
            txt_asistencia_Id_pk.Clear();
            txt_asistencia_anotacion_Id_fk.Clear();
            txt_asistencia_jugador_Id_fk.Clear();
            
        }

        private void DGV_asistencia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_asistencia.SelectedItem is DataRowView row)
            {

                txt_asistencia_Id_pk.Text = row["Código Asistencia"].ToString();
                txt_asistencia_anotacion_Id_fk.Text = row["Código Anotación"].ToString();
                txt_asistencia_jugador_Id_fk.Text = row["Código Jugador"].ToString();
                

            }
            ConfigurarBotonesDespuesDeSeleccion();
        }
    }
}
