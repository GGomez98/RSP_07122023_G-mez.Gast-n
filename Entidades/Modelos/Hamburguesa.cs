using Entidades.Enumerados;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.MetodosDeExtension;
using System.Text;
using Entidades.DataBase;

namespace Entidades.Modelos
{
    public class Hamburguesa : IComestible
    {

        private static int costoBase;
        private bool esDoble;
        private double costo;
        private bool estado;
        private string imagen;
        List<EIngrediente> ingredientes;
        Random random;
        static Hamburguesa() => Hamburguesa.costoBase = 1500;


        public Hamburguesa() : this(false) { }
        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
        }

        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";
        public bool Estado => this.esDoble;
        public string Imagen => this.imagen;


        /// <summary>
        /// mediante el metodo extendido que genera una lista aleatoria de ingredientes se la asigna al atributo ingredientes de la hamburguesa
        /// </summary>
        private void AgregarIngredientes()
        {
            this.ingredientes = this.random.IngredientesAleatorios();
        }

        public override string ToString() => this.MostrarDatos();

        public void FinalizarPreparacion(string cocinero)
        {
            this.costo = this.ingredientes.CalcularCostoIngrediente(costoBase);
            this.estado = !this.estado;
        }

        /// <summary>
        /// Muestra los datos de la hamburguesa
        /// </summary>
        /// <returns>un string con los datos</returns>
        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));
            return stringBuilder.ToString();

        }

        /// <summary>
        /// Obtiene un tipo de hamburguesa de manera aleatoria y le agrega los ingredientes
        /// </summary>
        public void IniciarPreparacion()
        {
            if (!this.estado)
            {
                int idImgaen = this.random.Next(1, 9);
                string tipo = $"Hamburguesa_{idImgaen}";
                this.imagen = DataBaseManager.GetImagenComida(tipo);
                this.AgregarIngredientes();
            }
        }
    }
}