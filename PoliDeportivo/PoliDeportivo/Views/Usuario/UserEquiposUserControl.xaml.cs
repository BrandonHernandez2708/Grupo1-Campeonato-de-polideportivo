using MySql.Data.MySqlClient;
using PoliDeportivo.DataAccess;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PoliDeportivo.Views.Usuario
{
    /// <summary>
    /// Lógica de interacción para UserEquiposUserControl.xaml
    /// </summary>
    public partial class UserEquiposUserControl : UserControl
    {
        private DataTable EquiposOriginal; 

        public UserEquiposUserControl()
        {
            InitializeComponent();
            CargarEquipos();
        }
        public DataTable Listado_Equipo()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"SELECT
                                    pk_equipo_id AS 'Código Equipo',
                                    equ_nombre AS 'Nombre del Equipo',
                                    equ_cant_integrantes AS 'Cantidad integrantes',
                                    equ_telefono AS 'Teléfono del Equipo',
                                    equ_correo AS 'Correo del Equipo',
                                    d.pk_deporte_id AS 'Código Deporte',
                                    d.dep_nombre AS 'Deporte',
                                    t.pk_entrenador_id AS 'Código Entrenador',
                                    t.ent_nombre AS 'Nombre Entrenador',
                                    t.ent_apellido AS 'Apellido Entrenador',
                                    tel.tel_entrenador AS 'Teléfono Entrenador',
                                    cor.correo_entrenador AS 'Correo Entrenador'
                                FROM tbl_equipo e
                                INNER JOIN tbl_deporte d ON e.fk_deporte_id = d.pk_deporte_id
                                INNER JOIN tbl_entrenador t ON e.fk_entrenador_id = t.pk_entrenador_id
                                LEFT JOIN tbl_telefono_entrenador tel ON t.pk_entrenador_id = tel.fk_entrenador_id
                                LEFT JOIN tbl_correo_entrenador cor ON t.pk_entrenador_id = cor.fk_entrenador_id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        Tabla.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener equipos: " + ex.Message, ex);
            }
            return Tabla;
        }

        private void CargarEquipos()
        {
            try
            {
                
                
                EquiposOriginal = Listado_Equipo();
                dgEquipos.ItemsSource = EquiposOriginal.DefaultView;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar equipos o entrenadores: " + ex.Message);
            }
        }

        private void btn_Buscar(object sender, RoutedEventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();

            if (EquiposOriginal == null || filtro == "buscar equipos...")
                return;

            try
            {
                DataView vistaFiltrada = EquiposOriginal.DefaultView;

                
                string columnaNombre = "Nombre del Equipo"; 

                
                string filtroSeguro = filtro.Replace("'", "''");

                // Aplicar filtro usando corchetes para evitar errores con espacios o caracteres especiales
                vistaFiltrada.RowFilter = $"[{columnaNombre}] LIKE '%{filtroSeguro}%'";
                dgEquipos.ItemsSource = vistaFiltrada;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al aplicar el filtro: " + ex.Message);
            }
        }

        private void txtBuscar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscar.Text == "Buscar equipos...")
            {
                txtBuscar.Text = "";
                txtBuscar.Foreground = Brushes.Black;
            }
        }

        private void txtBuscar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.Text = "Buscar equipos...";
                txtBuscar.Foreground = Brushes.Gray;
            }
        }
    }
}
