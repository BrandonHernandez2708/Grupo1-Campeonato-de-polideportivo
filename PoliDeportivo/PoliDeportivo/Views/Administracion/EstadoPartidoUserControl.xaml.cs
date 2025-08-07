using PoliDeportivo.DataAccess;
using PoliDeportivo.Model;
using PoliDeportivo.Views.Administracion.BTN_ayuda_forms;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace PoliDeportivo.Views.Administracion
{
    /// <summary>
    /// Lógica de interacción para EstadoPartidoUserControl.xaml
    /// </summary>
    public partial class EstadoPartidoUserControl : UserControl
    {
        private int iestadoguardado = 0; // 1 = nuevo, 2 = actualizar

        public EstadoPartidoUserControl()
        {
            InitializeComponent();
            Cargarestado();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_new_can.IsEnabled = true;
            boton_guardar.IsEnabled = false;
            boton_actualizar.IsEnabled = false;
            boton_eliminar.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_new_can.IsEnabled = false;
            boton_guardar.IsEnabled = true;
            boton_actualizar.IsEnabled = true;
            boton_eliminar.IsEnabled = true;
        }

        private void Cargarestado()
        {
            try
            {
                D_EstadoPartido datos = new D_EstadoPartido();
                DataTable dt = datos.Listado_estadopartido();
                DGV_estadopartido.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar Estado Partido: " + ex.Message);
            }
        }

        private void btn_new_est(object sender, RoutedEventArgs e)
        {
            iestadoguardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void btn_guardar_estado(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_estadopartidoID.Text) || string.IsNullOrWhiteSpace(txt_EstadoPartidoDescripcion.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                Atrb_estadopartido estadopartido = new Atrb_estadopartido()
                {
                    pk_estado_partido_id = int.Parse(txt_estadopartidoID.Text),
                    est_descripcion = txt_EstadoPartidoDescripcion.Text
                };

                D_EstadoPartido datos = new D_EstadoPartido();
                string respuesta = datos.Guardar_EstadoPartido(iestadoguardado, estadopartido);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    Cargarestado();
                    LimpiarCampos();
                    iestadoguardado = 0;
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

        private void btn_actualizar_estado(object sender, RoutedEventArgs e)
        {
            iestadoguardado = 2;

        }

        private void btn_eliminar_estado(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txt_estadopartidoID.Text, out int id))
                {
                    var confirm = MessageBox.Show("¿Estás seguro de eliminar este registro?", "Confirmar eliminación", MessageBoxButton.YesNo);
                    if (confirm == MessageBoxResult.Yes)
                    {
                        D_EstadoPartido datos = new D_EstadoPartido();
                        string srespuesta = datos.Eliminar_est(id);

                        if (srespuesta == "OK")
                        {
                            MessageBox.Show("Registro eliminado correctamente");
                            Cargarestado();
                            LimpiarCampos();
                        }
                        else
                        {
                            MessageBox.Show("Error: " + srespuesta);
                        }
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
            txt_estadopartidoID.Clear();
            txt_EstadoPartidoDescripcion.Clear();
        }

        private void DGV_estadopartido_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_estadopartido.SelectedItem is DataRowView row)
            {
                txt_estadopartidoID.Text = row["ID Estado Partido"].ToString();
                txt_EstadoPartidoDescripcion.Text = row["Estado"].ToString(); // Corregido alias
            }
            ConfigurarBotonesDespuesDeSeleccion();
        }

        private void txt_estadopartidoID_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void btn_ayuda_estado(object sender, RoutedEventArgs e)
        {
            ayuda_estado_partido ayudaVentana = new ayuda_estado_partido(); 
            ayudaVentana.ShowDialog();

        }
    }
}
