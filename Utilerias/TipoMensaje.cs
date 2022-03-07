using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using salon_web_api.ViewModels;

namespace salon_web_api.Utilerias
{
    public class TipoMensaje
    {
        public static MensajeViewModel MensajesError(string texto)
        {

            return new MensajeViewModel()
            {
                mensaje = texto,
                tipo = 1
            };
        }
        public static MensajeViewModel ErroresAtributos(string texto)
        {
            var error = "Error";

            return new MensajeViewModel()
            {
                mensaje = texto + error,
                tipo = 2
            };
        }
        public static MensajeViewModel Exitoso(string texto)
        {

            return new MensajeViewModel()
            {
                mensaje = texto,
                tipo = 3
            };
        }
        public static MensajeViewModel Informativo(string texto)
        {

            return new MensajeViewModel()
            {
                mensaje = texto,
                tipo = 4
            };
        }
    }

    public class EnvioCorreo
    {
        public static void SendGrid(string correo, string contrasenia)
        {
            string BODY = "<p><strong>&nbsp;Estimado usuario:" + correo + "</strong></p><p>&nbsp;</p><p><strong>Su contrasena se ha cambiado.</strong></p><p><strong>Una vez que ingrese le solicitara su cambio de contrase&ntilde;a:" + contrasenia + "</strong></p><p>&nbsp;</p><p><strong>Atentamente el equipo de Conc&aacute;</strong></p>";
            try
            {
                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("julio.santibanez@koottech.com", "Koottech");
                var subject = "Cambio de contraseña";
                var to = new EmailAddress(correo);
                var plainTextContent = " ";
                var htmlContent = BODY;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = client.SendEmailAsync(msg);

            }
            catch (Exception xe)
            {
            }
        }
    }
}