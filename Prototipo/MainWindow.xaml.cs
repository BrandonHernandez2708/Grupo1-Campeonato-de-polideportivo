using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Prototipo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_administrador(object sender, RoutedEventArgs e)
        {
            Window1 ventanaLogin = new Window1();
            ventanaLogin.Show();
            this.Close();

        }

        private void btn_usuario(object sender, RoutedEventArgs e)
        {
            Window1 ventanaLogin = new Window1();
            ventanaLogin.Show();
            this.Close();

        }
    }
}