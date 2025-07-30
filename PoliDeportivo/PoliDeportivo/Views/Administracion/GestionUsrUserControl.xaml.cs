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
        }

        private void btn_guardar(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_deportes deporte = new Atrb_deportes()
                {
                        depID_pk = int.Parse(txt_depId_pk.Text),
                    depnombre = txt_depnombre.Text,
                    depcantidad_jugadores_equipo = int.Parse(txt_CantTotPlay.Text),
                    depcantidad_jugadores_campo = int.Parse(txt_cant_play_camp.Text),
                    depcantidad_de_tiemposdep = int.Parse(txt_cant_tot_tiempos.Text),
                    depduracion_de_cada_tiempo = int.Parse(txt_dur_tiempos.Text),
                    depduracion_total_del_partido = int.Parse(txt_dur_total_partido.Text),
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
        }

        private void btn_actualizar(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 2; // Actualizar registro
        }

        private void btn_eliminar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txt_depId_pk.Text, out int id))
                {
                    D_Deportes datos = new D_Deportes();
                    string srespuesta = datos.Eliminar_Dep(id);

                    if (srespuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarDeportes();
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
        }

        private void LimpiarCampos()
        {
            txt_depId_pk.Clear();
            txt_depnombre.Clear();
            txt_CantTotPlay.Clear();
            txt_cant_play_camp.Clear();
            txt_cant_tot_tiempos.Clear();
            txt_dur_tiempos.Clear();
            txt_dur_total_partido.Clear();
        }

        private void DTGV_deporte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DTGV_deporte.SelectedItem is DataRowView row)
            {
                txt_depId_pk.Text = row["depID_pk"].ToString();
                txt_depnombre.Text = row["DEPNOMBRE"].ToString();
                txt_CantTotPlay.Text = row["DEPCANTIDAD_JUGADORES_EQUIPO"].ToString();
                txt_cant_play_camp.Text = row["DEPCANTIDAD_JUGADORES_EN_EL_CAMPO"].ToString();
                txt_cant_tot_tiempos.Text = row["DEPCANTIDAD_DE_TIEMPOSDEP"].ToString();
                txt_dur_tiempos.Text = row["DEPDURACION_DE_CADA_TIEMPO"].ToString();
                txt_dur_total_partido.Text = row["DEPDURACION_TOTAL_DEL_PARTIDO"].ToString();
            }
        }
    }
}
