using PoliDeportivo.DataAccess;
using PoliDeportivo.Model;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PoliDeportivo.Views.Administracion
{
    public partial class JornadasUserControl : UserControl
    {
        private int estadoGuardado = 0;
        public JornadasUserControl()
        {
            InitializeComponent();
            CargarJornada();
            ConfigurarBotonesEstadoInicial();
        }
        private void ConfigurarBotonesEstadoInicial()
        {
            boton_newjornada.IsEnabled = true;
            boton_jornada_guardar.IsEnabled = false;
            boton_jornada_actualizar.IsEnabled = false;
            boton_jornada_eliminar.IsEnabled = false;
        }
        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_newjornada.IsEnabled = false;
            boton_jornada_guardar.IsEnabled = true;
            boton_jornada_actualizar.IsEnabled = true;
            boton_jornada_eliminar.IsEnabled = true;
        }

        private void CargarJornada()
        {
            try
            {
                D_Jornadas datos = new D_Jornadas();
                DataTable dt = datos.Listado_Jornadas();
                DTGV_jornada.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar jornadas: " + ex.Message);
            }
        }

        private void btn_newjornada(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1;
            LimpiarCampos();
            boton_newjornada.IsEnabled = false;
            boton_jornada_guardar.IsEnabled = true;
            boton_jornada_actualizar.IsEnabled = false;
            boton_jornada_eliminar.IsEnabled = false;
        }
        private void btn_jornada_guardar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txt_Jornada_Id_pk.Text, out int id))
                {
                    MessageBox.Show("Por favor ingrese un ID válido.");
                    return;
                }

                Atrib_Jornadas jornadas = new Atrib_Jornadas()
                {
                    pk_jornada_id = id,
                    fk_campeonato_id = int.Parse(txt_Jornada_Campeonato_Id_fk.Text),
                    jor_cant_partidos = int.TryParse(txt_Jornada_cant_partidos.Text.Trim(), out int cantPartidos) ? cantPartidos : 0,
                    
                };

                D_Jornadas datos = new D_Jornadas();
                string respuesta = datos.Guardar_Jornada(estadoGuardado, jornadas);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarJornada();
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

        private void btn_jornada_actualizar(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2;
        }

        private void btn_jornada_eliminar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txt_Jornada_Id_pk.Text, out int id))
                {
                    D_Jornadas datos = new D_Jornadas();
                    string respuesta = datos.Eliminar_Jornada(id);

                    if (respuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarJornada();
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
        }

        private void LimpiarCampos()
        {
            txt_Jornada_Id_pk.Clear();
            txt_Jornada_Campeonato_Id_fk.Text.Trim();
            txt_Jornada_cant_partidos.Clear();
        }

        private void DTGV_jornada_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DTGV_jornada.SelectedItem is DataRowView row)
            {
                txt_Jornada_Id_pk.Text = row["Código Jornada"].ToString();
                txt_Jornada_cant_partidos.Text = row["cantidad partidos"].ToString();
                txt_Jornada_Campeonato_Id_fk.Text = row["Id campeonato"].ToString();

                ConfigurarBotonesDespuesDeSeleccion();
                estadoGuardado = 2;
            }
        }

        private void btn_ayuda_jornada(object sender, RoutedEventArgs e)
        {
            var ventanaAyuda = new BTN_ayuda_forms.ayuda_jornadas();
            ventanaAyuda.ShowDialog();

        }
    }
}