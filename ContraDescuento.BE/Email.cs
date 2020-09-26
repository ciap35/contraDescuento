using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class Email
    {
        #region Propiedades Privadas
        private string destinatario;
        private string origen;
        private string asunto;
        private string cuerpo;
        private string token;
        private BE.TipoEmail tipoEmail = new BE.TipoEmail();
        #endregion
        #region Propiedades Públicas
        public string Destinatario { get { return destinatario; } set { destinatario = value; } }
        public string Origen { get { return Origen; } set { origen = value; } }
        public string Asunto { get { return asunto; } set { asunto = value; } }
        public string Cuerpo { get { return cuerpo; } set { cuerpo = value; } }
        public string Token { get { return token;} set { token = value; } }
        #endregion  
        public BE.TipoEmail TipoEmail { get { return tipoEmail; } set { tipoEmail = value; } }
        public Email() { }
    }
}
