using PoliDeportivo;
using PoliDeportivo.DataAccess;
using PoliDeportivo.Model;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace PoliDeportivo.Views.Administracion
{
    public partial class GestionUsrUserControl : UserControl
    {
        private int estadoGuardado = 0; // 1 = nuevo, 2 = actualizar

        public GestionUsrUserControl()
        {
            InitializeComponent();
            CargarDeportes();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_newdep.IsEnabled = true;
            boton_guardar.IsEnabled = false;
            boton_actualizar.IsEnabled = false;
            boton_eliminar.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_newdep.IsEnabled = false;
            boton_guardar.IsEnabled = true;
            boton_actualizar.IsEnabled = true;
            boton_eliminar.IsEnabled = true;
        }

        private void CargarDeportes()
        {
            try
            {
                D_Deportes datos = new D_Deportes();
                DataTable dt = datos.Listado_Dep();
                DTGV_deporte.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar deportes: " + ex.Message);
            }
        }

        private void btn_newdep(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_guardar(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_deportes deporte = new Atrb_deportes()
                {
                    depID_pk = int.Parse(txtb_depId_pk.Text),
                    depnombre = txtb_depnombre.Text,
                    depcantidad_jugadores_equipo = int.Parse(txtb_CantTotPlay.Text),
                    depcantidad_jugadores_campo = int.Parse(txtb_cant_play_camp.Text),
                    depcantidad_de_tiemposdep = int.Parse(txtb_cant_tot_tiempos.Text),
                    depduracion_de_cada_tiempo = int.Parse(txtb_dur_tiempos.Text),
                    depduracion_total_del_partido = int.Parse(dur_total_partido.Text),
                };

                D_Deportes datos = new D_Deportes();
                string respuesta = datos.Guardar_Dep(estadoGuardado, deporte);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarDeportes();
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
                if (int.TryParse(txtb_depId_pk.Text, out int id))
                {
                    D_Deportes datos = new D_Deportes();
                    string respuesta = datos.Eliminar_Dep(id);

                    if (respuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarDeportes();
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
        }

        private void LimpiarCampos()
        {
            txtb_depId_pk.Clear();
            txtb_depnombre.Clear();
            txtb_CantTotPlay.Clear();
            txtb_cant_play_camp.Clear();
            txtb_cant_tot_tiempos.Clear();
            txtb_dur_tiempos.Clear();
            dur_total_partido.Clear();
        }

        private void DTGV_deporte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DTGV_deporte.SelectedItem is DataRowView row)
            {
                txtb_depId_pk.Text = row["Código Deporte"].ToString();
                txtb_depnombre.Text = row["Nombre Deporte"].ToString();
                txtb_CantTotPlay.Text = row["Tamaño de Equipos"].ToString();
                txtb_cant_play_camp.Text = row["Jugadores en el campo"].ToString();
                txtb_cant_tot_tiempos.Text = row["Cantidad de tiempos"].ToString();
                txtb_dur_tiempos.Text = row["Duración de Tiempos"].ToString();
                dur_total_partido.Text = row["Duración del partido"].ToString();
            }
            ConfigurarBotonesDespuesDeSeleccion();
        }
    }
}
