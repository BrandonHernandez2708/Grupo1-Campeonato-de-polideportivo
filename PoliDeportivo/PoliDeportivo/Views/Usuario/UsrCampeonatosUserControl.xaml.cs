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
    /// Lógica de interacción para UsrCampeonatosUserControl.xaml
    /// </summary>
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
            string filtro = txtBuscar.Text.Trim().ToLower();

            if (campeonatosOriginal == null || filtro == "buscar campeonato...")
                return;

            DataView vistaFiltrada = campeonatosOriginal.DefaultView;
            vistaFiltrada.RowFilter = $"nombre LIKE '%{filtro}%'";
            dgCampeonatos.ItemsSource = vistaFiltrada;

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
