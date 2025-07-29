using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Lógica de interacción para EquiposUserControl.xaml
    /// </summary>
    public partial class EquiposUserControl : UserControl
    {


        public EquiposUserControl()
        {
            InitializeComponent();
        }

        private void txtNombreEquipo_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Aquí puedes agregar la lógica que desees ejecutar cuando cambie el texto
        }

    }
}