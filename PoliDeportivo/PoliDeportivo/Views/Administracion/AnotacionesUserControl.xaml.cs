using PoliDeportivo.DataAccess;
using PoliDeportivo.Model;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PoliDeportivo.Views.Administracion
{
    public partial class AnotacionesUserControl : UserControl
    {
        private int estadoGuardado = 0;
        public AnotacionesUserControl()
        {
            InitializeComponent();
            CargarAnotaciones();
            ConfigurarBotonesEstadoInicial();

        }
        private void ConfigurarBotonesEstadoInicial()
        {
            boton_newanot.IsEnabled = true;
            boton_anot_guardar.IsEnabled = false;
            boton_anot_actualizar.IsEnabled = false;
            boton_anot_eliminar.IsEnabled = false;
        }
        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_newanot.IsEnabled = false;
            boton_anot_guardar.IsEnabled = true;
            boton_anot_actualizar.IsEnabled = true;
            boton_anot_eliminar.IsEnabled = true;
        }

        private void CargarAnotaciones()
        {
            try
            {
                D_Anotaciones datos = new D_Anotaciones();
                DataTable dt = datos.Listado_Anotaciones();
                DTGV_anotacion.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar Anotaciones: " + ex.Message);
            }
        }

        private void btn_newanot(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1;
            LimpiarCampos();
            boton_newanot.IsEnabled = false;
            boton_anot_guardar.IsEnabled = true;
            boton_anot_actualizar.IsEnabled = false;
            boton_anot_eliminar.IsEnabled = false;
        }
        private void btn_anot_guardar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txt_Anotacion_Id_pk.Text, out int id))
                {
                    MessageBox.Show("Por favor ingrese un ID válido.");
                    return;
                }

                Atrb_Anotaciones anotacion = new Atrb_Anotaciones()
                {
                    pk_anotacion_id = id,
                    ano_minuto = int.TryParse(txt_Anotacion_Minuto.Text.Trim(), out int minuto) ? minuto : 0,
                    ano_descripcion = txt_Anotacion_Descripcion.Text.Trim(),
                    fk_partido_id = int.TryParse(txt_Anotacion_partido_Id_fk.Text.Trim(), out int partidoId) ? partidoId : 0,
                    fk_jugador_id = int.TryParse(txt_Anotacion_jugador_Id_fk.Text.Trim(), out int jugadorId) ? jugadorId : 0
                };


                D_Anotaciones datos = new D_Anotaciones();
                string respuesta = datos.Guardar_Anotacion(estadoGuardado, anotacion);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarAnotaciones();
                    LimpiarCampos();
                    estadoGuardado = 0;
                    ConfigurarBotonesEstadoInicial();
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
        }

        private void btn_anot_actualizar(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2;
        }

        private void btn_anot_eliminar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txt_Anotacion_Id_pk.Text, out int id))
                {
                    D_Anotaciones datos = new D_Anotaciones();
                    string respuesta = datos.Eliminar_Anotacion(id);

                    if (respuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarAnotaciones();
                        LimpiarCampos();
                        estadoGuardado = 0;
                        ConfigurarBotonesEstadoInicial();
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
        }

        private void LimpiarCampos()
        {
            txt_Anotacion_Id_pk.Clear();
            txt_Anotacion_Minuto.Clear();
            txt_Anotacion_Descripcion.Clear();
            txt_Anotacion_partido_Id_fk.Clear();
            txt_Anotacion_jugador_Id_fk.Clear();
        }

        private void DTGV_anotacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DTGV_anotacion.SelectedItem is DataRowView row)
            {
                txt_Anotacion_Id_pk.Text = row["Id anotacion"].ToString();
                txt_Anotacion_Minuto.Text = row["Minuto"].ToString();
                txt_Anotacion_Descripcion.Text = row["Descripcion"].ToString();
                txt_Anotacion_partido_Id_fk.Text = row["Id partido"].ToString();
                txt_Anotacion_jugador_Id_fk.Text = row["Id jugador"].ToString();

                ConfigurarBotonesDespuesDeSeleccion();
                estadoGuardado = 2;
            }
        }
    }
}