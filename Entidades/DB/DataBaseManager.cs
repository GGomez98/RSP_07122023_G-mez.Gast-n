﻿using System.Data.SqlClient;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public class DataBaseManager
    {
        private static SqlConnection? connection;
        private static string stringConnection;

        static DataBaseManager()
        {
            DataBaseManager.stringConnection = "Server=.;Database=20230622SP;Trusted_Connection=True;";

        }

        public static string GetImagenComida(string tipo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(stringConnection))
                {
                    string query = "SELECT * FROM comidas WHERE tipo_comida = @tipo_comida";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("tipo_comida", tipo);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader.GetString(2);
                    }
                    else
                    {
                        throw new ComidaInvalidaExeption("No se encontro la comida");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataBaseManagerException("Error al Acceder a la base", ex);
            }
        }
        public static bool GuardarTicket<T>(string nombreEmpleado, T comida) where T : IComestible, new()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(stringConnection))
                {
                    string query = "INSERT INTO TICKETS (empleado,ticket)" + "VALUES (@empleado,@ticket)";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("empleado", nombreEmpleado);
                    cmd.Parameters.AddWithValue("ticket", comida.Ticket);

                    connection.Open();
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new DataBaseManagerException("Error al guardar el ticket", ex);
            }
        }

    }
}
