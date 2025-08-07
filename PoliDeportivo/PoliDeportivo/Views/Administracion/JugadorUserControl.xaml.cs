using PoliDeportivo.DataAccess;
using PoliDeportivo.Model;
using PoliDeportivo.Views.Administracion.BTN_ayuda_forms;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace PoliDeportivo.Views.Administracion
{
    public partial class JugadorUserControl : UserControl
    {
        private int ijugadorguardado = 0; // 1 = nuevo, 2 = actualizar

        public JugadorUserControl()
        {
            InitializeComponent();
            CargarJugadores();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_newjug.IsEnabled = true;
            boton_guardar_jug.IsEnabled = false;
            boton_actualizar_jug.IsEnabled = false;
            boton_eliminar_jug.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_newjug.IsEnabled = false;
            boton_guardar_jug.IsEnabled = true;
            boton_actualizar_jug.IsEnabled = true;
            boton_eliminar_jug.IsEnabled = true;
        }

        private void CargarJugadores()
        {
            try
            {
                D_Jugadores datos = new D_Jugadores();
                DataTable dt = datos.Listado_Jugadores();
                DGV_jugador.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar Jugador: " + ex.Message);
            }
        }

        private void LimpiarCampos()
        {
            txt_jugador_id.Clear();
            txt_jugador_nombre.Clear();
            txt_jugador_apellido.Clear();
            txt_jugador_posicion.Clear();
            txt_fkequipo_id.Clear();
        }

        private void btn_newjug(object sender, RoutedEventArgs e)
        {
            ijugadorguardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_guardar_jugador(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_jugador_id.Text) ||
                    string.IsNullOrWhiteSpace(txt_jugador_nombre.Text) ||
                    string.IsNullOrWhiteSpace(txt_jugador_apellido.Text) ||
                    string.IsNullOrWhiteSpace(txt_jugador_posicion.Text) ||
                    string.IsNullOrWhiteSpace(txt_fkequipo_id.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                Atrb_Jugadores jugador = new Atrb_Jugadores()
                {
                    pk_jugador_id = int.Parse(txt_jugador_id.Text),
                    jug_nombre = txt_jugador_nombre.Text,
                    jug_apellido = txt_jugador_apellido.Text,
                    jug_posicion = txt_jugador_posicion.Text,
                    fk_equipo_id = int.Parse(txt_fkequipo_id.Text)
                };

                D_Jugadores datos = new D_Jugadores();
                string respuesta = datos.Guardar_Jugador(ijugadorguardado, jugador);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarJugadores();
                    LimpiarCampos();
                    ijugadorguardado = 0;
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

        private void btn_actualizar_jugador(object sender, RoutedEventArgs e)
        {
            ijugadorguardado = 2; // Actualizar registro
        }

        private void btn_eliminar_jugador(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txt_jugador_id.Text, out int id))
                {
                    D_Jugadores datos = new D_Jugadores();
                    string respuesta = datos.Eliminar_Jugador(id);

                    if (respuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarJugadores();
                        LimpiarCampos();
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

        private void DGV_jugador_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_jugador.SelectedItem is DataRowView row)
            {
                txt_jugador_id.Text = row["Código Jugador"]?.ToString() ?? "";
                txt_jugador_nombre.Text = row["Nombre del Jugador"]?.ToString() ?? "";
                txt_jugador_apellido.Text = row["Apellido del Jugador"]?.ToString() ?? "";
                txt_jugador_posicion.Text = row["Posición"]?.ToString() ?? "";
                txt_fkequipo_id.Text = row["ID Equipo"]?.ToString() ?? "";
                ConfigurarBotonesDespuesDeSeleccion();
            }
        }

        private void txtb_equipo_nombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Si no usas este evento, puedes eliminarlo
        }

        private void btn_ayuda_jugador(object sender, RoutedEventArgs e)
        {
            ayuda_jugador ventanaAyuda = new ayuda_jugador();
            ventanaAyuda.ShowDialog();

        }
    }
}
