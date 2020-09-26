using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class DigitoVerificador
    {
        private int codTabla = 0;
        private string tabla = string.Empty;
        private int resultado = 0;
        private int resultadoAnterior = 0;
        private DateTime fechaCalculo;
        private DateTime fechaCalculoAnterior;

        public int CodTabla { get { return codTabla; } set { codTabla = value; } }
        public string Tabla { get { return tabla; } set { tabla = value; } }
        public int Resultado { get { return resultado; } set { resultado = value; } } 
        public int ResultadoAnterior { get { return resultadoAnterior; } set { resultadoAnterior = value; } }
        public DateTime FechaCalculo { get { return fechaCalculo; } set { fechaCalculo = value; } }
        public DateTime FechaCalculoAnterior { get { return FechaCalculoAnterior; } set { FechaCalculoAnterior = fechaCalculoAnterior; } }

        public DigitoVerificador() { }
    }
}
