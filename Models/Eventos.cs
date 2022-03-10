using System;
using System.Collections.Generic;

namespace salon_web_api.Models
{
    public partial class Eventos
    {
        public Eventos()
        {
            Payment = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public virtual Usuario User { get; set; } = null!;
        public virtual ICollection<Payment> Payment { get; set; }
    }
}
