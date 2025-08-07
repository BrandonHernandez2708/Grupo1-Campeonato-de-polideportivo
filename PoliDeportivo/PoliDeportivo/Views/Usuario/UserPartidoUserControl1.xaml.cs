using PoliDeportivo.DataAccess;
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

namespace PoliDeportivo.Views.Usuario
{
    /// <summary>
    /// Lógica de interacción para UserPartidoUserControl1.xaml
    /// </summary>
    public partial class UserPartidoUserControl1 : UserControl
    {
        private DataTable campeonatosOriginal;

        public UserPartidoUserControl1()
        {
            InitializeComponent();
            CargarPartido();
        }

        private void CargarPartido()
        {
            try
            {
                D_Partidos datos = new D_Partidos();
                campeonatosOriginal = datos.Listado_Partidos();
                dgpartido.ItemsSource = campeonatosOriginal.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar partidos: " + ex.Message);
            }
        }

        private void btn_buscar(object sender, RoutedEventArgs e)
        {
            if (campeonatosOriginal == null)
                return;

            string filtroTexto = txtBuscar.Text.Trim();
            DateTime? fechaSeleccionada = dpFecha.SelectedDate;

            string filtroTextoFinal = string.Empty;

            if (!string.IsNullOrWhiteSpace(filtroTexto) && filtroTexto != "Buscar campeonato...")
            {
                if (int.TryParse(filtroTexto, out int codigo))
                {
                    filtroTextoFinal = $"[Código Partido] = {codigo}";
                }
                else
                {
                    filtroTextoFinal =
                        $"[Estado del Partido] LIKE '%{filtroTexto}%'" +
                        $" OR [Equipo 1] LIKE '%{filtroTexto}%'" +
                        $" OR [Equipo 2] LIKE '%{filtroTexto}%'" +
                        $" OR [Equipo Ganador] LIKE '%{filtroTexto}%'";
                }
            }

            string filtroFecha = string.Empty;
            if (fechaSeleccionada.HasValue)
            {
                // Rango desde el inicio hasta el final del día seleccionado
                string fechaInicio = $"#{fechaSeleccionada.Value:MM/dd/yyyy} 00:00:00#";
                string fechaFin = $"#{fechaSeleccionada.Value:MM/dd/yyyy} 23:59:59#";
                filtroFecha = $"[Fecha y Hora] >= {fechaInicio} AND [Fecha y Hora] <= {fechaFin}";
            }

            string filtroFinal = filtroTextoFinal;
            if (!string.IsNullOrEmpty(filtroFecha))
            {
                if (!string.IsNullOrEmpty(filtroFinal))
                    filtroFinal = $"({filtroFinal}) AND ";
                filtroFinal += filtroFecha;
            }

            try
            {
                DataView vistaFiltrada = campeonatosOriginal.DefaultView;
                vistaFiltrada.RowFilter = filtroFinal;
                dgpartido.ItemsSource = vistaFiltrada;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar: " + ex.Message);
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
