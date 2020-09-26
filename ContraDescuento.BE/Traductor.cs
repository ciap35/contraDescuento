using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public  class Traductor
    {
        private BE.Idioma idioma = new Idioma();
        private BE.Traduccion traduccion = new BE.Traduccion();


        public BE.Idioma Idioma { get { return idioma; } set { idioma = value; } }
        [Exportable(true)]
        public BE.Traduccion Traduccion { get { return traduccion; } set { traduccion=value; } }

        public Traductor() { }
    }
}
