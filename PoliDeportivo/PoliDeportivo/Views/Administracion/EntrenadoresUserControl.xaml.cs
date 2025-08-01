using PoliDeportivo.DataAccess;
using PoliDeportivo.Model;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace PoliDeportivo.Views.Administracion
{
    public partial class EntrenadoresUserControl : UserControl
    {
        private int estadoGuardado = 0; 

        public EntrenadoresUserControl()
        {
            InitializeComponent();
            CargarEntrenadores();
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

        private void CargarEntrenadores()
        {
            try
            {
                D_Entrenadores datos = new D_Entrenadores();
                DataTable dt = datos.Listado_Entrenadores();
                DTGV_entrenador.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar entrenadores: " + ex.Message);
            }
        }

        private void btn_newdep(object sender, RoutedEventArgs e)
        {
            estadoGuardado = 1; 
            LimpiarCampos();
            boton_newdep.IsEnabled = false;
            boton_guardar.IsEnabled = true; 
            boton_actualizar.IsEnabled = false;
            boton_eliminar.IsEnabled = false;
        }

        private void btn_guardar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txt_Entredador_Id_pk.Text, out int id))
                {
                    MessageBox.Show("Por favor ingrese un ID válido.");
                    return;
                }

                Atrb_entrenadores entrenador = new Atrb_entrenadores()
                {
                    pk_entrenador_id = id,
                    ent_nombre = txt_Entredador_nombre.Text.Trim(),
                    ent_apellido = txt_Entredador_apellido.Text.Trim(),
                    tel_numero = txt_Entredador_telefono.Text.Trim(),
                    correo = txt_Entredador_correo.Text.Trim(),
                };

                D_Entrenadores datos = new D_Entrenadores();
                string respuesta = datos.Guardar_Entrenador(estadoGuardado, entrenador);

                if (respuesta == "OK")
                {
                    MessageBox.Show("Registro guardado correctamente");
                    CargarEntrenadores();
                    LimpiarCampos();
                    estadoGuardado = 0;
                    ConfigurarBotonesEstadoInicial(); 
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
            estadoGuardado = 2; 
        }

        private void btn_eliminar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txt_Entredador_Id_pk.Text, out int id))
                {
                    D_Entrenadores datos = new D_Entrenadores();
                    string respuesta = datos.Eliminar_Entrenador(id);

                    if (respuesta == "OK")
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        CargarEntrenadores();
                        LimpiarCampos();
                        estadoGuardado = 0;
                        ConfigurarBotonesEstadoInicial(); 
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
        }

        private void LimpiarCampos()
        {
            txt_Entredador_Id_pk.Clear();
            txt_Entredador_nombre.Clear();
            txt_Entredador_apellido.Clear();
            txt_Entredador_telefono.Clear();
            txt_Entredador_correo.Clear();
        }

        private void DTGV_entrenador_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DTGV_entrenador.SelectedItem is DataRowView row)
            {
                txt_Entredador_Id_pk.Text = row["id_entrenador"].ToString();
                txt_Entredador_nombre.Text = row["nombre"].ToString();
                txt_Entredador_apellido.Text = row["apellido"].ToString();
                txt_Entredador_telefono.Text = row["telefono"].ToString();
                txt_Entredador_correo.Text = row["correo"].ToString();

                ConfigurarBotonesDespuesDeSeleccion(); 
                estadoGuardado = 2; 
            }
        }
    }
}
