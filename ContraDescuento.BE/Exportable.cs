using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class Exportable : Attribute
    {

        private bool exportar = false;

        public virtual bool Exportar{get { return exportar; }set { exportar = value; } }
        public Exportable(bool export) { this.exportar = export; }
    }
}
