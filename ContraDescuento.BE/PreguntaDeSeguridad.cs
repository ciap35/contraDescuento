using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class PreguntaDeSeguridad
    {
        private int codPreguntaSeguridad;
        private string pregunta;
        private string respuesta;
        private int entradaCodPreguntaUsuario;
        private string entradaRespuestaUsuario;
        private DateTime fechaAlta;
        private DateTime fechaBaja;
        private DateTime fechaModificacion;

        public int CodPreguntaSeguridad { get { return codPreguntaSeguridad; } set { codPreguntaSeguridad = value; } }
        public string Pregunta { get { return pregunta; } set { pregunta= value;} }
        public string Respuesta { get { return respuesta; } set { respuesta = value;} }
        public int EntradaCodPreguntaUsuario { get { return entradaCodPreguntaUsuario; } set { entradaCodPreguntaUsuario = value; } }
        public string EntradaRespuestaUsuario { get { return entradaRespuestaUsuario; } set { entradaRespuestaUsuario = value; } }
        public DateTime FechaAlta { get { return fechaAlta; } set { fechaAlta = value; } }
        public DateTime FechaBaja { get { return fechaAlta; } set { fechaAlta = value; } }
        public DateTime FechaModificacion { get { return fechaAlta; } set { fechaAlta = value; } }

        public PreguntaDeSeguridad() { }

        public override string ToString()
        {
            return CodPreguntaSeguridad.ToString() + "-" + Pregunta +" - " + Respuesta;
        }
    }
}
