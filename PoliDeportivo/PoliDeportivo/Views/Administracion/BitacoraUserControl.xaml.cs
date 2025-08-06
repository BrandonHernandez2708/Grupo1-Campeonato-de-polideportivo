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

namespace PoliDeportivo.Views.Administracion
{
    /// <summary>
    /// Lógica de interacción para BitacoraUserControl.xaml
    /// </summary>
    public partial class BitacoraUserControl : UserControl
    {

        private DataTable BitacoraOriginal;
        public BitacoraUserControl()
        {
            InitializeComponent();
            CargarBitacora();
        }

        private void CargarBitacora()
        {
            try
            {
                D_Bitacora datos = new D_Bitacora();
                BitacoraOriginal = datos.Listado_Bitacora();
                DGV_Bitacora.ItemsSource = BitacoraOriginal.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar Bitacora: " + ex.Message);
            }
        }

        private void btn_buscar(object sender, RoutedEventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();

            if (BitacoraOriginal == null || filtro == "buscar usuario o tabla...")
                return;

            DataView vistaFiltrada = BitacoraOriginal.DefaultView;

            filtro = filtro.Replace("'", "''");

            vistaFiltrada.RowFilter = $"Usuario LIKE '%{filtro}%' OR Entidad LIKE '%{filtro}%'";

            DGV_Bitacora.ItemsSource = vistaFiltrada;
        }


        private void txtBuscar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscar.Text == "Buscar Bitacora...")
            {
                txtBuscar.Text = "";
                txtBuscar.Foreground = Brushes.Black;
            }
        }

        private void txtBuscar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.Text = "Buscar Bitacora...";
                txtBuscar.Foreground = Brushes.Gray;
            }
        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

