using System.Data.SqlClient;
using System.Xml.Linq;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Entidades.DataBase
{
    public class DataBaseManager
    {
        private static SqlConnection connection;
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
            string imagen = "";
            try
            {
                using (connection = new SqlConnection(stringConnection))
                {
                    string query = "SELECT * FROM comidas WHERE tipo_comida = @tipo_comida";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("tipo_comida", tipo);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.HasRows)
                    {
                        reader.Read();
                        imagen = reader.GetString(2);
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
            }
            catch (SqlException ex)
            {
                DataBaseManagerException dataEx = new DataBaseManagerException("Error al leer la base", ex);
                FileManager.Guardar(dataEx.Message, "logs.txt", true);
            }
            catch(Exception ex)
            {
                Exception e = new Exception("Ocurrio un error", ex);
                FileManager.Guardar(e.Message, "logs.txt", true);
            }

            return imagen;

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
            bool retorno = false;
            try
            {
                using (connection = new SqlConnection(stringConnection))
                {
                    string query = "INSERT INTO TICKETS (empleado,ticket)" + "VALUES (@empleado,@ticket)";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@empleado", nombreEmpleado);
                    cmd.Parameters.AddWithValue("@ticket", comida.Ticket);

                    connection.Open();
                    if(cmd.ExecuteNonQuery() > 0)
                    {
                        retorno = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                DataBaseManagerException dataEx = new DataBaseManagerException("Error al guardar en la base", ex);
                FileManager.Guardar(dataEx.Message, "logs.txt", true);
            }
            catch (Exception ex)
            {
                Exception e = new Exception("Ocurrio un error", ex);
                FileManager.Guardar(e.Message, "logs.txt", true);
            }

            return retorno;
        }

    }
}
