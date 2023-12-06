namespace Entidades.Interfaces
{
    public interface IComestible
    {
        public bool Estado { get; }
        public string Imagen { get; }
        public string Ticket { get; }

        void FinalizarPreparacion(string cocinero);
        void IniciarPreparacion();
    }
}
