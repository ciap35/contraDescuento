using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public class DigitoVerificador
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        DAL.DigitoVerificador DigVerif = new DAL.DigitoVerificador();
        #endregion
        public DigitoVerificador() { }


        public List<BE.IAuditable> ValidarDigitosVerificadores()
        {
            try
            {
               return CalcularDVH(ObtenerRegistrosAuditables());
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        /// <summary>
        /// Se procede a obtener los registros a auditar
        /// </summary>
        private List<BE.IAuditable> ObtenerRegistrosAuditables()
        {
            try
            {
                DAL.DigitoVerificador digVerif = new DAL.DigitoVerificador();
                return digVerif.ObtenerRegistrosUsuario();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }


        /// <summary>
        /// Se procede a calcular el DVH para cada registro obtenido.
        /// Posteriormente se actualiza el DVV y se determina si la base esta corrupta.
        /// </summary>
        private List<BE.IAuditable> CalcularDVH(List<BE.IAuditable> ListObjAuditables)
        {
            List<BE.IAuditable> lstRegCorruptos = new List<BE.IAuditable>() ;
            int dvhOriginal = 0, dvhCalculado = 0, cantRegistrosCorruptos = 0,resultado=0;
            try
            {
                foreach (BE.IAuditable objAuditable in ListObjAuditables)
                {
                    dvhOriginal = objAuditable.ObtenerDVHOriginal();
                    dvhCalculado = objAuditable.CalcularDVH();
                    resultado += dvhCalculado;
                    if(dvhOriginal != dvhCalculado)
                    {
                        cantRegistrosCorruptos += 1;
                       lstRegCorruptos.Add(objAuditable);
                    }
                }
                ActualizarEstado(resultado);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return lstRegCorruptos;
        }

        private void ActualizarEstado(int resultado)
        {
            try
            {
                DAL.DigitoVerificador digVerif = new DAL.DigitoVerificador();
                DigVerif.ActualizarDVV(DateTime.Now, resultado);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
    }
}
