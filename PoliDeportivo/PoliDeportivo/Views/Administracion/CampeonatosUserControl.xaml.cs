using PoliDeportivo.DataAccess;
using PoliDeportivo.Model;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PoliDeportivo.Views.Administracion
{
    public partial class CampeonatosUserControl : UserControl
    {
        private int estadoGuardado = 0;
        public CampeonatosUserControl()
        {
            InitializeComponent();
            CargarCampeonatos();
            ConfigurarBotonesEstadoInicial();

        }
        private void ConfigurarBotonesEstadoInicial()
        {
            boton_newcamp.IsEnabled = true;
            boton_camp_guardar.IsEnabled = false;
            boton_camp_actualizar.IsEnabled = false;
            boton_camp_eliminar.IsEnabled = false;
        }
        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_newcamp.IsEnabled = false;
            boton_camp_guardar.IsEnabled = true;
            boton_camp_actualizar.IsEnabled = true;
            boton_camp_eliminar.IsEnabled = true;
        }

        private void CargarCampeonatos()
        {
            try
            {
                D_Campeonatos datos = new D_Campeonatos();
                DataTable dt = datos.Listado_Campeonatos();
                DTGV_campeonato.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar campeonatos: " + ex.Message);
            }
        }

        private void btn_newcamp(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1;
            LimpiarCampos();
            boton_newcamp.IsEnabled = false;
            boton_camp_guardar.IsEnabled = true;
            boton_camp_actualizar.IsEnabled = false;
            boton_camp_eliminar.IsEnabled = false;
        }
        private void btn_camp_guardar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txt_Campeonato_Id_pk.Text, out int id))
                {
                    MessageBox.Show("Por favor ingrese un ID válido.");
                    return;
                }

                Atrb_campeonatos campeonato = new Atrb_campeonatos()
                {
                    pk_campeonato_id = id,
                    cam_nombre = txt_Campeonato_nombre.Text.Trim(),
                    cam_modalidad = txt_Campeonato_modalidad.Text.Trim(),
                    cam_cant_equipos = int.TryParse(txt_Campeonato_cantidad_equipos.Text.Trim(), out int cantEquipos) ? cantEquipos : 0,
                    cam_fecha_inicio = DateTime.TryParse(txt_Campeonato_fecha_inicio.Text.Trim(), out DateTime fechaInicio) ? fechaInicio : DateTime.MinValue,
                    cam_fecha_final = DateTime.TryParse(txt_Campeonato_fecha_final.Text.Trim(), out DateTime fechaFinal) ? fechaFinal : DateTime.MinValue,
                    jor_cant_partidos = int.TryParse(txt_Campeonatojor_cant_partidos.Text.Trim(), out int cantPartidos) ? cantPartidos : 0,
                };

                D_Campeonatos datos = new D_Campeonatos();
                string respuesta = datos.Guardar_Campeonato(estadoGuardado, campeonato);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarCampeonatos();
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

        private void btn_camp_actualizar(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2;
        }

        private void btn_camp_eliminar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txt_Campeonato_Id_pk.Text, out int id))
                {
                    D_Campeonatos datos = new D_Campeonatos();
                    string respuesta = datos.Eliminar_Campeonato(id);

                    if (respuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarCampeonatos();
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
            txt_Campeonato_Id_pk.Clear();
            txt_Campeonato_nombre.Clear();
            txt_Campeonato_modalidad.Clear();
            txt_Campeonato_cantidad_equipos.Clear();
            txt_Campeonato_fecha_inicio.Clear();
            txt_Campeonato_fecha_final.Clear();
            txt_Campeonatojor_cant_partidos.Clear();
        }

        private void DTGV_campeonato_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DTGV_campeonato.SelectedItem is DataRowView row)
            {
                txt_Campeonato_Id_pk.Text = row["Id campeonato"].ToString();
                txt_Campeonato_nombre.Text = row["Nombre"].ToString();
                txt_Campeonato_modalidad.Text= row["Modalidad"].ToString();
                txt_Campeonato_cantidad_equipos.Text = row["Cantidad equipos"].ToString();
                txt_Campeonato_fecha_inicio.Text = row["Fecha inicio"].ToString();
                txt_Campeonato_fecha_final.Text = row["Fecha final"].ToString();
                txt_Campeonatojor_cant_partidos.Text = row["Cantidad partidos"].ToString();

                ConfigurarBotonesDespuesDeSeleccion();
                estadoGuardado = 2;
            }
        }
    }
}