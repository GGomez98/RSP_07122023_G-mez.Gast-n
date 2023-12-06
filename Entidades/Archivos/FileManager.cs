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
    
    public  class FileManager
    {
        private static string path;

        private FileManager()
        {
            string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FileManager.path = Path.Combine(escritorio, "20231207_Gómez.Gastón");
            FileManager.ValidarExistenciaDeDirectorio();
        }

        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            try
            {
                FileManager.path = Path.Combine(FileManager.path, nombreArchivo);
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
                FileManager.path = Path.Combine(FileManager.path, nombreArchivo);
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;

                using (StreamWriter sw = new StreamWriter(FileManager.path))
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
            if (!Directory.Exists(FileManager.path))
            {
                try
                {
                    string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    FileManager.path = Path.Combine(escritorio, "20231207_Gómez.Gastón");
                }
                catch (Exception e)
                {
                    throw new FileManagerException("Error al crear directorio", e);
                }
            }
        }
    }
}
