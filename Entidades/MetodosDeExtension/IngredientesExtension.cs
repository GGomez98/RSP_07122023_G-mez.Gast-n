using Entidades.Enumerados;


namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension
    {
        /// <summary>
        /// Realiza el calculo del coste final del menu tomando el costo base del mismo y sumando los porcentajes de los ingredientes
        /// </summary>
        /// <param name="ingredientes">la lista de ingredientes</param>
        /// <param name="costoInicial">el costo baste del menu (sin ingredientes)</param>
        /// <returns>el costo base con los porcentajes de los ingredientes sumados</returns>
        public static double CalcularCostoIngrediente(this List<EIngrediente> ingredientes, int costoInicial)
        {
            foreach (EIngrediente ingrediente in ingredientes)
            {
                costoInicial += (costoInicial / 100 * (int)ingrediente);
            }
            return costoInicial;
        }
        /// <summary>
        /// Genera una nueva lista a partir de un numero aleatorio el cual representa la cantidad de ingredientes
        /// </summary>
        /// <param name="rand">el numero aleatorio de ingredientes a tomar</param>
        /// <returns>la nueva lista generada</returns>
        public static List<EIngrediente> IngredientesAleatorios(this Random rand)
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>()
            {
                EIngrediente.QUESO,
                EIngrediente.PANCETA,
                EIngrediente.ADHERESO,
                EIngrediente.HUEVO,
                EIngrediente.JAMON,
            };
            int cantidad = rand.Next(1, ingredientes.Count + 1);

            return ingredientes.Take(cantidad).ToList();
        }

    }
}
