using PoliDeportivo.DataAccess;
using PoliDeportivo.Model;
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
    /// Lógica de interacción para EquiposUserControl.xaml
    /// </summary>
    public partial class EquiposUserControl : UserControl
    {
        private int estadoGuardado = 0; // 1 = nuevo, 2 = actualizar
        public EquiposUserControl()
        {
            InitializeComponent();
            CargarEquipos();
            ConfigurarBotonesEstadoInicial();
        }

        private void ConfigurarBotonesEstadoInicial()
        {
            boton_newdep_equipo.IsEnabled = true;
            boton_guardar_equipo.IsEnabled = false;
            boton_actualizar_equipo.IsEnabled = false;
            boton_eliminar_equipo.IsEnabled = false;
        }

        private void ConfigurarBotonesDespuesDeSeleccion()
        {
            boton_newdep_equipo.IsEnabled = false;
            boton_guardar_equipo.IsEnabled = true;
            boton_actualizar_equipo.IsEnabled = true;
            boton_eliminar_equipo.IsEnabled = true;
        }

        private void LimpiarCampos()
        {
            txtb_equipo_Id_pk.Clear();
            txtb_equipo_nombre.Clear();
            txtb_cant_integrantes.Clear();
            txtb_telefono_equipo.Clear();
            txtb_correo_equipo.Clear();

        }

        private void CargarEquipos()
        {
            try
            {
                D_equipos datos = new D_equipos();
                DataTable dt = datos.Listado_Equipo();
                DGV_equipo.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar equipos: " + ex.Message);
            }

        }



        private void btn_newdep_equipo(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; // Nuevo registro
            LimpiarCampos();
            ConfigurarBotonesDespuesDeSeleccion();

        }

        private void btn_guardar_equipo(object sender, RoutedEventArgs e)
        {
            try
            {
                Atrb_equipo equipo = new Atrb_equipo()
                {
                    pk_equipo_id = int.Parse(txtb_equipo_Id_pk.Text),
                    equipo_nombre = txtb_equipo_nombre.Text,
                    cant_integrantes = int.Parse(txtb_cant_integrantes.Text),
                    equipo_telefono = txtb_telefono_equipo.Text,
                    equipo_correo = txtb_correo_equipo.Text,
                    fk_deporte_id = int.Parse(txtb_id_deporte.Text),
                    fk_entrenador_id = int.Parse(txtb_id_entrenador.Text)
                };

                D_equipos datos = new D_equipos();
                string respuesta = datos.Guardar_Equipo(estadoGuardado, equipo);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarEquipos();
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
            CargarEquipos();

        }

        private void btn_actualizar_equipo(object sender, RoutedEventArgs e)
        {

                estadoGuardado = 2; // Actualizar
        }

        private void btn_eliminar_equipo(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtb_equipo_Id_pk.Text, out int id))
                {
                    D_equipos datos = new D_equipos();
                    string respuesta = datos.Eliminar_Equipo(id);

                    if (respuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarEquipos();
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
            CargarEquipos();

        }

        private void DGV_equipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_equipo.SelectedItem is DataRowView row)
            {
                txtb_equipo_Id_pk.Text = row["Código Equipo"]?.ToString();
                txtb_equipo_nombre.Text = row["Nombre del Equipo"]?.ToString();
                txtb_cant_integrantes.Text = row["Cantidad integrantes"]?.ToString();
                txtb_telefono_equipo.Text = row["Teléfono del Equipo"]?.ToString();
                txtb_correo_equipo.Text = row["Correo del Equipo"]?.ToString();
                txtb_id_deporte.Text = row["Código Deporte"]?.ToString();
                txtb_id_entrenador.Text = row["Código Entrenador"]?.ToString();
                
                ConfigurarBotonesDespuesDeSeleccion();
            }
        }


    }
}
