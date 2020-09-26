using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public interface IAuditable
    {
        int CalcularDVH();
        int ObtenerDVHOriginal();
        string getDescripcion();
    }
}
