using System;
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
}