using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class Traduccion
    {
        private int codTraduccion;
        private string pagina;
        private string controlID;
        private string texto;

        [Exportable(true)]
        public int CodTraduccion { get { return codTraduccion; } set { codTraduccion = value; } }
        [Exportable(true)]
        public string Pagina { get { return pagina; } set { pagina = value; } }
        [Exportable(true)]
        public string ControlID { get { return controlID; } set { controlID = value; } }
        [Exportable(true)]
        public string Texto{ get { return texto; } set { texto = value; } }

        public Traduccion() { }

        public override string ToString()
        {
            return CodTraduccion+"-"+ Pagina + "-" + ControlID + "-" + Texto;
        }
    }
}
