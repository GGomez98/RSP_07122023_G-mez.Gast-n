using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades.Files
{
    
    public static class FileManager
    {
        private static string path;

        static FileManager()
        {
            string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FileManager.path = escritorio + "\\20231207_Gómez.Gastón\\";
            FileManager.ValidarExistenciaDeDirectorio();
        }
        /// <summary>
        /// Guarda un archivo con informacion
        /// </summary>
        /// <param name="data">el dato a guardar dentro del archivo</param>
        /// <param name="nombreArchivo">el nombre del archivo</param>
        /// <param name="append">si se desea sobreescribir el archivo o seguir en la linea siguiente</param>
        /// <exception cref="FileManagerException"></exception>
        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            try
            {
                string destino = Path.Combine(path, nombreArchivo);
                using (StreamWriter sw = new StreamWriter(destino,append))
                {
                    sw.WriteLine(data);
                }
            }
            catch (Exception e)
            {
                throw new FileManagerException("Error al guardar archivo", e);
            }
        }
        /// <summary>
        /// Serializa un elemento a formato json
        /// </summary>
        /// <typeparam name="T">el tipo de elemento a serializar</typeparam>
        /// <param name="elemento">el elemento a serializar</param>
        /// <param name="nombreArchivo">el nombre del archivo json</param>
        /// <returns></returns>
        /// <exception cref="FileManagerException"></exception>
        public static bool Serializar<T>(T elemento, string nombreArchivo)
            where T : class
        {
            try
            {
                string destino = Path.Combine(path,nombreArchivo);
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;

                using (StreamWriter sw = new StreamWriter(destino))
                {
                    string elementoJson = JsonSerializer.Serialize(elemento, options);
                    sw.WriteLine(elementoJson);
                    return true;
                }
            }
            catch (Exception e)
            {
                FileManager.Guardar(e.Message, "logs.txt", true);
                throw new FileManagerException("Error al serializar archivo", e);
            }
        }
        /// <summary>
        /// Valida la existencia de un directorio y si no existe lo crea
        /// </summary>
        /// <exception cref="FileManagerException"></exception>
        private static void ValidarExistenciaDeDirectorio()
        {
           try
           {
                if (!Directory.Exists(FileManager.path))
                {
                    Directory.CreateDirectory(FileManager.path);
                }
           }
           catch (Exception e)
           {
                FileManager.Guardar(e.Message, "logs.txt", true);
                throw new FileManagerException("Error al crear directorio", e);
           }
        }
    }
}
