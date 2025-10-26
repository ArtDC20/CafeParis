using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace AccesoDatosCafe
{
    public class TiposServicioDAO
    {
        private readonly Conexion conexion = new Conexion();

        // Obtener todos los tipos
        public List<TipoServicio> ObtenerTiposServicio()
        {
            var lista = new List<TipoServicio>();

            using (var conn = conexion.ObtenerConexion())
            {
                string sql = "SELECT id, nombre, descripcion, sub_tipo, activo, creado_en FROM tipos_servicio";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new TipoServicio
                    {
                        Id = reader.GetInt32("id"),
                        Nombre = reader.GetString("nombre"),
                        Descripcion = reader.GetString("descripcion"),
                        SubTipo = reader.GetString("sub_tipo"),
                        Activo = reader.GetBoolean("activo"),
                        CreadoEn = reader.GetDateTime("creado_en")
                    });
                }
            }
            return lista;
        }

        // ✅ Nuevo método: Obtener por ID
        public TipoServicio ObtenerTipoServicioPorId(int id)
        {
            TipoServicio tipo = null;

            using (var conn = conexion.ObtenerConexion())
            {
                string sql = "SELECT id, nombre, descripcion, sub_tipo, activo, creado_en FROM tipos_servicio WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    tipo = new TipoServicio
                    {
                        Id = reader.GetInt32("id"),
                        Nombre = reader.GetString("nombre"),
                        Descripcion = reader.GetString("descripcion"),
                        SubTipo = reader.GetString("sub_tipo"),
                        Activo = reader.GetBoolean("activo"),
                        CreadoEn = reader.GetDateTime("creado_en")
                    };
                }
            }
            return tipo;
        }
    }
}
