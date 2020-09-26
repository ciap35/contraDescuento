using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class Bitacora
    {
        private int codMensaje = 0;
        private string mensaje = string.Empty;
        private string mensajePersonalizado = string.Empty;
        private string stack = string.Empty;
        private string proyecto = string.Empty;
        private string clase = string.Empty;
        private string metodo = string.Empty;
        private string contexto = string.Empty;
        private bool excepcionControlada = false;
        private DateTime fecha;

        public Bitacora() { }

        public Bitacora(Exception ex, bool ExcepcionControlada)
        {
            this.mensaje = ex.Message; //Mensaje Error
            this.stack = ex.StackTrace; //Stack Trace
            this.proyecto = ex.Source; //DLL
            if (ex.TargetSite != null)
                this.Metodo = Convert.ToString(ex.TargetSite); // Método
            if (ex.TargetSite != null)
                this.Clase = Convert.ToString(ex.TargetSite.DeclaringType.FullName); //Clase;
            this.ExcepcionControlada = ExcepcionControlada;
        }

        public Bitacora(Exception ex)
        {
            this.mensaje = ex.Message; //Mensaje Error
            this.stack = ex.StackTrace; //Stack Trace
            this.proyecto = ex.Source; //DLL
            if (ex.TargetSite != null)
                this.Metodo = Convert.ToString(ex.TargetSite); // Método
            if (ex.TargetSite != null)
                this.Clase = Convert.ToString(ex.TargetSite.DeclaringType.FullName); //Clase;
        }

        [Exportable(true)]
        public int CodMensaje { get { return codMensaje; } set { codMensaje = value; } }
        [Exportable(true)]
        public string Mensaje { get { return mensaje; } set { mensaje = value; } }
        [Exportable(false)]
        public string MensajePersonalizado { get { return mensajePersonalizado; } set { mensajePersonalizado = value; } }
        [Exportable(true)]
        public string Stack { get { return stack; } set { stack = value; } }
        [Exportable(true)]
        public string Proyecto { get { return proyecto; } set { proyecto = value; } }
        [Exportable(true)]
        public string Clase { get { return clase; } set { clase = value; } }
        [Exportable(false)]
        public string Metodo { get { return metodo; } set { metodo = value; } }
        [Exportable(false)]
        public string Contexto { get { return contexto; } set { contexto = value; } }
        [Exportable(true)]
        public bool ExcepcionControlada { get { return excepcionControlada; } set { excepcionControlada = value; } }
        [Exportable(true)]
        public DateTime Fecha { get { return fecha; } set { fecha = value; } }

        public override string ToString()
        {
            return Mensaje + "Stack: " + Stack + "\n\rProyecto " + Proyecto + "\n\rClase: " + Clase + "\n\rMétodo: " + Metodo;
        }

    }
}
