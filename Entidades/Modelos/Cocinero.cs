using Entidades.DataBase;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using System.ComponentModel.Design;

namespace Entidades.Modelos
{


    public delegate void DelegadoDemoraAtencion(double demora);
    public delegate void DelegadoNuevoIngreso(IComestible menu);
    public class Cocinero<T> where T : IComestible, new()
    {
        private int cantPedidosFinalizados;
        private string nombre;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private T pedidoEnPreparacion;
        private Mozo<T> mozo;
        private Queue<T> pedidos;
        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoNuevoIngreso OnIngreso;

        private Task tarea;





        public Cocinero(string nombre)
        {
            this.nombre = nombre;
        }

        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                    this.tarea.Status == TaskStatus.WaitingToRun ||
                    this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.IniciarIngreso();
                }
                else
                {
                    this.cancellation.Cancel();
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; set => this.cantPedidosFinalizados = value; }

        /// <summary>
        /// Inicia un hilo ejecutando las tareas para preparar el menu
        /// </summary>
        private void IniciarIngreso()
        {
            this.tarea = Task.Run(()=>{
                do
                {
                    this.NotificarNuevoIngreso();
                    this.EsperarProximoIngreso();
                    this.CantPedidosFinalizados++;
                    DataBaseManager.GuardarTicket(this.nombre, this.pedidoEnPreparacion);
                } while (!this.cancellation.IsCancellationRequested);
            }, this.cancellation.Token);
        }

        /// <summary>
        /// Crea un nuevo menu y lo notifica
        /// </summary>
        private void NotificarNuevoIngreso()
        {
            if (this.OnIngreso != null)
            {
                this.pedidoEnPreparacion = new T();
                this.pedidoEnPreparacion.IniciarPreparacion();
                this.OnIngreso.Invoke(this.pedidoEnPreparacion);
            }
        }
        /// <summary>
        /// Realiza un conteo entre el momento en que se finaliza un menu y el siguiente
        /// </summary>
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;

            if(this.OnDemora != null)
            {
                while(!this.cancellation.IsCancellationRequested && !this.pedidoEnPreparacion.Estado)
                {
                    this.OnDemora.Invoke(tiempoEspera);
                    Thread.Sleep(1000);
                    tiempoEspera++;
                }
            }

            this.demoraPreparacionTotal += tiempoEspera;
        }
    }
}
