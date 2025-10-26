using MySql.Data.MySqlClient;
using System.Data;

namespace AccesoDatosCafe
{
    public class Conexion
    {
        private readonly MySqlConnection conexion;

        public Conexion()
        {
            string cadena = "Server=localhost;Database=cafe_paris;User=root;Password=;";
            conexion = new MySqlConnection(cadena);
        }

        public MySqlConnection ObtenerConexion()
        {
            return conexion;
        }

        public void Abrir()
        {
            if (conexion.State == ConnectionState.Closed)
                conexion.Open();
        }

        public void Cerrar()
        {
            if (conexion.State == ConnectionState.Open)
                conexion.Close();
        }
    }
}
