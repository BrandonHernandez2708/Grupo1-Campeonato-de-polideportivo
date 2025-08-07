using PoliDeportivo.DataAccess;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PoliDeportivo.Views.Usuario
{
    public partial class UsrCampeonatosUserControl : UserControl
    {
        private DataTable campeonatosOriginal;

        public UsrCampeonatosUserControl()
        {
            InitializeComponent();
            CargarCampeonatos();
        }

        private void CargarCampeonatos()
        {
            try
            {
                D_Campeonatos datos = new D_Campeonatos();
                campeonatosOriginal = datos.Listado_Campeonatos();
                dgCampeonatos.ItemsSource = campeonatosOriginal.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar campeonatos: " + ex.Message);
            }
        }

        private void btn_buscar(object sender, RoutedEventArgs e)
        {
            if (campeonatosOriginal == null)
                return;

            string filtroTexto = txtBuscar.Text.Trim();
            DateTime? fechaSeleccionada = dpFecha.SelectedDate;

            // Filtro texto busca en Nombre y Modalidad
            string filtroNombre = string.Empty;
            if (!string.IsNullOrWhiteSpace(filtroTexto) && filtroTexto != "Buscar campeonato...")
            {
                filtroNombre = $"([Nombre] LIKE '%{filtroTexto}%' OR [Modalidad] LIKE '%{filtroTexto}%')";
            }

            // Filtro por fecha ([Fecha inicio])
            string filtroFecha = string.Empty;
            if (fechaSeleccionada.HasValue)
            {
                string fechaStr = $"#{fechaSeleccionada.Value:yyyy-MM-dd}#";
                filtroFecha = $"[Fecha inicio] = {fechaStr}";
            }

            // Combinar filtros
            string filtroFinal = filtroNombre;
            if (!string.IsNullOrEmpty(filtroFecha))
            {
                if (!string.IsNullOrEmpty(filtroFinal))
                    filtroFinal += " AND ";
                filtroFinal += filtroFecha;
            }

            try
            {
                DataView vistaFiltrada = campeonatosOriginal.DefaultView;
                vistaFiltrada.RowFilter = filtroFinal;
                dgCampeonatos.ItemsSource = vistaFiltrada;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en filtro: " + ex.Message);
            }
        }

        private void txtBuscar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscar.Text == "Buscar campeonato...")
            {
                txtBuscar.Text = "";
                txtBuscar.Foreground = Brushes.Black;
            }
        }

        private void txtBuscar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.Text = "Buscar campeonato...";
                txtBuscar.Foreground = Brushes.Gray;
            }
        }
    }
}
