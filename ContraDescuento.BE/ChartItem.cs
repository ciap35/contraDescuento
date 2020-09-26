using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class ChartItem
    {
        #region Propiedades Privadas
            private string etiqueta = string.Empty;
            private string valor = string.Empty;
        #endregion

        #region Propiedades Públicas
        public string Etiqueta { get { return etiqueta; } set { etiqueta = value; } }
            public string Valor { get { return valor; } set { valor = value; } }
        #endregion
        public ChartItem() { }

        public ChartItem(string etiqueta, string valor)
        {
            this.Etiqueta = etiqueta;
            this.Valor= valor;
        }
    }
}
