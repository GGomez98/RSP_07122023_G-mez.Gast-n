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

        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            try
            {
                string destino = Path.Combine(path, nombreArchivo);
                using (StreamWriter sw = new StreamWriter(FileManager.path, append))
                {
                    sw.WriteLine(data);
                }
            }
            catch (Exception e)
            {
                throw new FileManagerException("Error al guardar archivo", e);
            }
        }

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
                throw new FileManagerException("Error al serializar archivo", e);
            }
        }

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
                throw new FileManagerException("Error al crear directorio", e);
           }
        }
    }
}
