using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class Chart
    {
        #region Propiedades Privadas
        private List<ChartItem> lstChartItems = new List<ChartItem>();
        #endregion

        #region Propiedades Públicas
        public List<ChartItem> LstChartItems { get { return lstChartItems; } set { lstChartItems = value; } }
        #endregion
        public Chart() { }
    }
}
