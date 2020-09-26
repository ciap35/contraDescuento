using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.TFL
{
    public class Export
    {
        public Export() { }

        public void Escribir<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            //foreach (PropertyDescriptor prop in props)
            //{
            //    foreach (Attribute atr in prop.Attributes)
            //    {
            //        if (atr.GetType() == typeof(BE.Exportable))
            //        {
            //           if(((ContraDescuento.BE.Exportable)atr).Exportar)
            //            {
            //                output.Write(prop.DisplayName); // Nombre de la columna
            //                output.Write("\t");
            //            }
            //        }
            //    }
            //}
            //output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    foreach (Attribute atr in prop.Attributes)
                    {
                        if (atr.GetType() == typeof(BE.Exportable))
                        {
                            if (((ContraDescuento.BE.Exportable)atr).Exportar)
                            {
                                output.Write(prop.Converter.ConvertToString(prop.GetValue(item)));
                                output.Write("\t");
                            }
                        }
                     
                    }
                }
                output.WriteLine();
            }
        }
    }
}
