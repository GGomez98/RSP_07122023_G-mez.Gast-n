using System.Data.SqlClient;
using System.Xml.Linq;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Files;
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
        /// <summary>
        /// Se obtiene la url de la imagen del tipo de la fila solicitada
        /// </summary>
        /// <param name="tipo">el tipo solicitado</param>
        /// <returns></returns>
        /// <exception cref="ComidaInvalidaExeption"></exception>
        /// <exception cref="DataBaseManagerException"></exception>
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
            catch (ComidaInvalidaExeption ex) 
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);
                throw new ComidaInvalidaExeption("No se encontro la comida");
            }
            catch (Exception ex)
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);
                throw new DataBaseManagerException("Ocurrio un Error", ex);
            }
        }
        /// <summary>
        /// Se carga un ticket a la base de datos
        /// </summary>
        /// <typeparam name="T">El tipo a cargar</typeparam>
        /// <param name="nombreEmpleado">el nombre del empleado</param>
        /// <param name="comida">el menu a cargar</param>
        /// <returns>un true indicando que la operacion fue exitosa</returns>
        /// <exception cref="DataBaseManagerException"></exception>
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
                FileManager.Guardar(ex.Message, "logs.txt", true);
                throw new DataBaseManagerException("Error al guardar el ticket", ex);
            }
        }

    }
}
