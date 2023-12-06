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
        private T menu;
        public event DelegadoNuevoIngreso OnIngreso;
        public event DelegadoDemoraAtencion OnDemora;

        private Task tarea;





        public Cocinero(string nombre)
        {
            this.nombre = nombre;
            this.cantPedidosFinalizados = 0;
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

        private void IniciarIngreso()
        {
            this.tarea = Task.Run(()=>{
                do
                {
                    this.NotificarNuevoIngreso();
                    this.EsperarProximoIngreso();
                    this.CantPedidosFinalizados++;
                    DataBaseManager.GuardarTicket(this.Nombre, this.menu);
                } while (!this.cancellation.IsCancellationRequested);
            }, this.cancellation.Token);
        }

        private void NotificarNuevoIngreso()
        {
            if (this.OnIngreso != null)
            {
                this.menu = new T();
                this.menu.IniciarPreparacion();
                this.OnIngreso.Invoke(this.menu); 
                
            }
        }
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;

            if(this.OnDemora != null)
            {
                while(this.menu.Estado == false && !this.cancellation.IsCancellationRequested)
                {
                    tiempoEspera++;
                    Thread.Sleep(1000);
                    this.OnDemora.Invoke(tiempoEspera);
                }

                this.demoraPreparacionTotal += tiempoEspera;
            }
        }
    }
}
