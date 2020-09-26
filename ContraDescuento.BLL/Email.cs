using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Text;
namespace ContraDescuento.BLL
{
    public class Email
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        TFL.SymetricCryptool SymCrypto = null;
        #endregion

        public Email() { }

        public void EnviarEmail(BE.Usuario usuario, BE.Enum_Tipo_Email enumTipoEmail)
        {
            try
            {
                //Configuro al destinatario.
                BE.Email email = new BE.Email();
                email.Destinatario = usuario.Email;
                email.TipoEmail.CodTipoEmail = (int)enumTipoEmail;
                //GeneroToken
                SymCrypto = new TFL.SymetricCryptool();
                string token = email.Destinatario.ToString() + DateTime.Now.ToString();
                email.Token = SymCrypto.Encrypt(ref token);
                email.Token = email.Token.Replace("+", "");
                email.Token = email.Token.Replace("-", "");
                email.Token = email.Token.Replace("/", "");
                email.Token = email.Token.Replace("\\", "");
                email.Token = email.Token.Replace("'", "");
                email.Token = email.Token.Replace("*", "");
                email.Token = email.Token.Replace("&", "");
                email.Token = email.Token.Replace("?", "");
                email.Token = email.Token.Replace("¿", "");
                //Obtengo el template
                BE.Email emailEnvio = ObtenerTemplate(enumTipoEmail);

                string cuenta = string.Empty;
                string pwd = string.Empty;
                cuenta = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["Email_Cuenta"]);
                pwd = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["Email_Password"]);
                //Envio el e-mail
                SmtpClient smtpCliente = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587, //25  587   465
                    //Host = "pop.gmail.com",
                    //Port = 995,
                    EnableSsl = true,
                    
                    Credentials = new NetworkCredential(cuenta, pwd)
                };
                //Guardo email con token generado.

                MailMessage mail = new MailMessage("TFI.44563.CIAP@gmail.com", email.Destinatario);
                mail.IsBodyHtml = true;
                mail.Subject = emailEnvio.Asunto;

                string URL = ConfigurationSettings.AppSettings["URL"].ToString();
                URL += "AccountManager.aspx?tipoOperacion="+email.TipoEmail.CodTipoEmail+"&token=" + email.Token;
                StringBuilder body = new StringBuilder();
                body.Append(@"<html>");
                body.Append(string.Format("<head><title>{0}</title></head>", emailEnvio.Asunto));
                body.Append("<body>");
                body.Append(string.Format("<span>Reestableza su contraseña realizando click desde aquí: {0}<span>", URL));
                body.Append("</body>");
                body.Append("</html>");
                mail.Body = body.ToString();

                smtpCliente.Send(mail);

                RegistrarEnvio(email);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        private void RegistrarEnvio(BE.Email email)
        {
            try
            {
                DAL.Email emailDAL = new DAL.Email();
                emailDAL.RegistrarEnvio(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private BE.Email ObtenerTemplate(BE.Enum_Tipo_Email enumTipoEmail)
        {
            try
            {
                DAL.Email emailDAL = new DAL.Email();
                BE.TipoEmail tipoEmail = new BE.TipoEmail() { CodTipoEmail = (int)enumTipoEmail };
                return emailDAL.ObtenerTemplate(tipoEmail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
