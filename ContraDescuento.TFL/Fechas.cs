using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.TFL
{
    public class Fechas
    {
        public Fechas() { }

        public int CalcularEdad(DateTime fechaNacimiento)
        {
            int edad = 0;
            try
            {
                edad = DateTime.Now.Year -fechaNacimiento.Year ;
            }
            catch(Exception ex)
            {
                throw new Exception("TFL.Fechas.CalcularEdad - Error al calcular la edad: " + ex.Message);
            }
            return edad;
        }
    }
}
