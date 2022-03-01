using System;
using System.Collections.Generic;

namespace salon_web_api.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Eventos = new HashSet<Evento>();
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Contrasenia { get; set; } = null!;
        public DateOnly FechaNacimiento { get; set; }
        public string CodigoPostal { get; set; } = null!;
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public bool? CambioContrasenia { get; set; }
        public bool? IsAdmin { get; set; }

        public virtual ICollection<Evento> Eventos { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
